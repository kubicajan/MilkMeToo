using Managers;
using Objects.Abstract;
using UnityEngine;
using UnityEngine.UI;

namespace Objects
{
    public class Mommy : PassiveKokTreeObjects
    {
        [SerializeField] private ParticleSystem rain;
        [SerializeField] private Image image;

        public delegate void OnRestartDelegate();

        public static event OnRestartDelegate OnRestart;

        public static int magicResetValue = 100;
        private int timesRestarted = 0;
        public static int unlockCounter = 0;

        public Mommy()
        {
            objectName = "Mommy";
            kokButtonDescription =
                $"She will finally be proud of you. \n \n <b> <color=red> This will reset your progress.</color> </b>  \n \n {timesRestarted} times proud so far.";
            kokButtonUnlockPrice = 2000000000;
        }

        protected override void Start()
        {
            unlockCounter = SaveManager.instance.GetMommyUnlockCounter();

            base.Start();
            effectInfo = $"{magicResetValue}% EXTRA PRODUCTION";
            timesRestarted = SaveManager.instance.GetTimesProud();
            kokButtonDescription =
                $"She will finally be proud of you. \n \n <b> <color=red> This will restart your progress.</color> </b>  \n \n {timesRestarted} times proud so far.";
            if (timesRestarted == 0)
            {
                rain.Stop();
            }
            else
            {
                image.GetComponent<Image>().color = new Color32(0, 146, 255, 255);
                rain.Play();
            }

            if (SaveManager.instance.wrapper.timesProud > 0)
            {
                this.kokButtonUnlockPrice = originalkokUnlockPrice *
                                            (Mommy.magicResetValue * SaveManager.instance.wrapper.timesProud);
                kokButtonUnlockPrice = kokButtonUnlockPrice + ((kokButtonUnlockPrice * 20) / 100);
            }
            else
            {
                kokButtonUnlockPrice = 2000000000;
            }
        }

        public override void LockButton()
        {
            unlockCounter++;
            SaveManager.instance.UpdateMommyUnlockCounter(unlockCounter);
            if (unlockCounter >= 2)
            {
                base.LockButton();
            }
        }

        public override void BuyUpgrade()
        {
            Social.ReportProgress(GPGSIds.achievement_you_did_it_she_is_proud_of_you, 100.0f, (bool success) => { });

            RestartEverything();
            unlockCounter = 0;
            SaveManager.instance.UpdateMommyUnlockCounter(0);
            MoneyManagerSingleton.instance.ResetMoney();
            MoneyManagerSingleton.instance.ResetMultiplicationAndAddToIt(magicResetValue);

            if (SaveManager.instance.wrapper.timesProud > 0)
            {
                this.kokButtonUnlockPrice = originalkokUnlockPrice *
                                            (Mommy.magicResetValue * SaveManager.instance.wrapper.timesProud);
                kokButtonUnlockPrice = kokButtonUnlockPrice + ((kokButtonUnlockPrice * 20) / 100);
            }
            toUnlockNext.gameObject.SetActive(true);
        }

        private void RestartEverything()
        {
            magicResetValue = 100 * (timesRestarted + 1);
            timesRestarted++;
            SaveManager.instance.UpdateTimesProud(1);
            kokButtonDescription =
                $"She will finally be proud of you. \n \n <b> <color=red> This will restart your progress.</color> </b>  \n \n {timesRestarted} times proud so far.";
            OnRestart?.Invoke();
            image.GetComponent<Image>().color = new Color32(0, 146, 255, 255);
            rain.Play();
        }
    }
}