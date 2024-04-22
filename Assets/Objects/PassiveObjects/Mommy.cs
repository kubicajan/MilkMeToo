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
        private int unlockCounter = 0;

        public Mommy()
        {
            objectName = "Mommy";
            kokButtonDescription =
                $"She will finally be proud of you. \n \n <b> <color=red> This will restart your progress.</color> </b>  \n \n {timesRestarted} times proud so far.";
            kokButtonUnlockPrice = 5;
        }

        protected override void Start()
        {
            base.Start();
            effectInfo = $"{magicResetValue}% EXTRA PRODUCTION";
            rain.Stop();

        }

        public override void LockButton()
        {
            unlockCounter++;
            if (unlockCounter >= 2)
            {
                base.LockButton();
            }
        }

        public override void BuyUpgrade()
        {
            RestartEverything();
            unlockCounter = 0;
            MoneyManagerSingleton.instance.ResetMoney();
            MoneyManagerSingleton.instance.ResetMultiplicationAndAddToIt(magicResetValue);
        }

        private void RestartEverything()
        {
            magicResetValue = 100 * (timesRestarted + 1);
            timesRestarted++;
            kokButtonDescription =
                $"She will finally be proud of you. \n \n <b> <color=red> This will restart your progress.</color> </b>  \n \n {timesRestarted} times proud so far.";
            OnRestart?.Invoke();
            image.GetComponent<Image>().color = new Color32(0, 146, 255, 255);
            rain.Play();
        }
    }
}