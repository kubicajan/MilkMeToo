using UnityEngine;
using UnityEngine.SceneManagement;

namespace Managers
{
    public class SongManager : MonoBehaviour
    {
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioClip click;
        [SerializeField] private AudioClip purchase;

        public static SongManager instance;

        private void Awake()
        {
//            DontDestroyOnLoad(transform.gameObject);

            if (instance != null && instance != this)
            {
                Destroy(this.gameObject);
            }
            else
            {
                instance = this;
            }
        }

        public void PlayMusic()
        {
            if (audioSource.isPlaying) return;
            audioSource.Play();
        }

        public void StopMusic()
        {
            audioSource.Stop();
        }

        public void PlayClick()
        {
            audioSource.PlayOneShot(click);
        }

        public void PlayPurchase()
        {
            audioSource.PlayOneShot(purchase);
        }
    }
}