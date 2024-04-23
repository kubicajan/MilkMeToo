using System;
using System.Threading.Tasks;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;

namespace Managers
{
    public class GoogleLoginManager : MonoBehaviour
    {
        public string GooglePlayToken;
        public string GooglePlayError;

        public async Task Authenticate()
        {
            PlayGamesPlatform.Activate();
            await UnityServices.InitializeAsync();
            PlayGamesPlatform.Instance.Authenticate(success =>
            {
                if (success == SignInStatus.Success)
                {
                    Debug.Log("successful login");
                    PlayGamesPlatform.Instance.RequestServerSideAccess(true, code =>
                    {
                        Debug.Log($"Auth code is {code}");
                        GooglePlayToken = code;
                    });
                }
                else
                {
                    GooglePlayError = "failed to retreieve GPG auth code";
                    Debug.Log("login unsuccessful");
                }
            });

            await AuthenticateWithUnity();

        }

        private async Task AuthenticateWithUnity()
        {
            try
            {
                await AuthenticationService.Instance.SignInWithGoogleAsync(GooglePlayToken);
            }
            catch (AuthenticationException e)
            {
                Debug.LogException(e);
                throw;
            }
            catch (RequestFailedException e)
            {
                Debug.LogException(e);
                throw;
            }
        }
        
    }

}


