using Managers;
using Objects.Abstract;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Objects
{
    public class Mommy : PassiveKokTreeObjects
    {
        [SerializeField] private ParticleSystem rain;
        [SerializeField] private Image image;

        public Sprite ggsprite;

        public delegate void OnRestartDelegate();

        public static event OnRestartDelegate OnRestart;

        public static int magicResetValue = 100;
        private int timesRestarted = 0;
        public static int unlockCounter = 0;

        public Mommy()
        {
            objectName = "Mommy";
            kokButtonDescription =
                $"She will finally be proud of you. \n \n <b> <color=red> Restarts your progress but gives upgrades. Keeps multiplier.</color> </b>";
            kokButtonUnlockPrice = 2000000000;
            showTheLine = false;
        }

        protected override void Start()
        {
            unlockCounter = SaveManager.instance.GetMommyUnlockCounter();

            base.Start();
            effectInfo = $"{magicResetValue}% EXTRA PRODUCTION";
            timesRestarted = SaveManager.instance.GetTimesProud();
            kokButtonDescription =
                $"She will finally be proud of you. \n \n <b> <color=red> Restarts your progress. You keep all multipliers and event upgrades, buffs pills</color> </b>";
            if (timesRestarted == 0)
            {
                rain.Stop();
                kokButtonUnlockPrice = 2000000000;
            }
            else
            {
                this.availableKokButtonSprite = ggsprite;
                effectInfo = $"BOSS FIGHT";
                kokButtonUnlockPrice = 1;
                image.GetComponent<Image>().color = new Color32(0, 146, 255, 255);
                rain.Play();
                kokButtonDescription = "A hidden door appears and behind them, a magnificent beast. What does it want? \n It requires a small donation to open.";
                toUnlockNext.gameObject.SetActive(true);

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
            if (SaveManager.instance.wrapper.timesProud > 0)
            {
                SceneManager.LoadScene("Boss");

            }
            else
            {
                Social.ReportProgress(GPGSIds.achievement_you_did_it_she_is_proud_of_you, 100.0f, (bool success) => { });
                kokButton.image.sprite = ggsprite;
                effectInfo = $"BOSS FIGHT";
                kokButtonUnlockPrice = 0;
                image.GetComponent<Image>().color = new Color32(0, 146, 255, 255);
                rain.Play();
                kokButtonDescription = "A hidden door appears and behind them, a magnificent beast. What does it want? \n It requires a small donation to open.";

                unlockCounter = 0;
                SaveManager.instance.UpdateMommyUnlockCounter(0);
                MoneyManagerSingleton.instance.ResetMoney();
                MoneyManagerSingleton.instance.ResetMultiplicationAndAddToIt(magicResetValue);
                toUnlockNext.gameObject.SetActive(true);
                kokButtonUnlockPrice = 1;
                UpdateUpgradePriceDisplayText(kokButtonUnlockPrice);
                RestartEverything();
            }
        }

        private void RestartEverything()
        {
            magicResetValue = 100 * (timesRestarted + 1);
            timesRestarted++;
            SaveManager.instance.UpdateTimesProud(1);
            kokButtonDescription = "A hidden door appears and behind them, a magnificent beast. What does it want? \n It requires a small donation to open.";
            OnRestart?.Invoke();
            image.GetComponent<Image>().color = new Color32(0, 146, 255, 255);
            rain.Play();
        }
    }
}