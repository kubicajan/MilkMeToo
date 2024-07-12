using System;
using Managers;
using Objects.Abstract;
using PopUps;
using Unity.VisualScripting;
using UnityEngine;
using Utilities;

namespace Objects.PassiveObjects
{
    public class Toilet : PassiveKokTreeObjects
    {
        private Decimal flushed;

        public Toilet()
        {
            kokButtonStatus = ButtonStatus.LOCKED;
            objectName = "Toilet";
            // kokButtonDescription = $"Flush everything down  \n \n {flushed}$ flushed";
            kokButtonUnlockPrice = 100;
            multiplicationBonus = 0;
            effectInfo = "???";
            showTheLine = false;
        }

        protected override void OnBuyUpgradeTriggeredHandler()
        {
            if (KokTreePopUp.instance.GetCallingType() == GetType())
            {
                BuyUpgrade();
                SongManager.instance.PlayPurchase();
            }
        }

        public override void LockButton()
        {
            base.LockButton();
            UpdateUpgradePriceDisplayText("");
        }

        protected override void MakeButtonAvailable()
        {
            base.MakeButtonAvailable();
            UpdateUpgradePriceDisplayText("");
        }

        protected override void LoadAllAssets()
        {
            availableParticleName = "AvailableParticle2";
            flushed = SaveManager.instance.GetFlushed();
            kokButtonDescription = $"Flush everything down!  \n \n {Helpers.ConvertNumbersToString(flushed)}$ flushed.";

            if (SaveManager.instance.GetTimesProud() >= 1)
            {
                base.LoadAllAssets();
                this.gameObject.SetActive(true);
            }
            else
            {
                this.gameObject.SetActive(false);
            }

            UpdateUpgradePriceDisplayText("");
            effectInfo = "???";
        }

        protected override void ResetHandler()
        {
        }

        public override void ClickKokButton()
        {
            bool unlock = MoneyManagerSingleton.instance.IsEnoughFunds(kokButtonUnlockPrice);
            string objName = objectName;
            SongManager.instance.PlayClick();

            if (kokButtonStatus == ButtonStatus.BOUGHT)
            {
                unlock = false;
            }

            if (kokButtonStatus == ButtonStatus.LOCKED)
            {
                objName = "???";
            }

            What(objName, "Why is this a feature??", unlock);
        }

        public override void BuyUpgrade()
        {
            SaveManager.instance.UpdateFlushed(MoneyManagerSingleton.instance.GetMoney());
            flushed = SaveManager.instance.GetFlushed();
            Social.ReportScore((long)(SaveManager.instance.GetFlushed() / 1000000),
                GPGSIds.leaderboard_flushed_in_mil, success => { });
            MoneyManagerSingleton.instance.SpendMoney(MoneyManagerSingleton.instance.GetMoney());
            kokButtonDescription = $"Flush everything down  \n \n {Helpers.ConvertNumbersToString(flushed)}$ flushed";
            UpdateUpgradePriceDisplayText("");
        }
    }
}