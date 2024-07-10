using System;
using Managers;
using Objects.Abstract;
using UnityEngine;
using Utilities;

namespace Objects.PassiveObjects
{
    public class Toilet : PassiveKokTreeObjects
    {
        private Decimal flushed;

        public Toilet()
        {
            kokButtonStatus = ButtonStatus.AVAILABLE;
            objectName = "Toilet";
            // kokButtonDescription = $"Flush everything down  \n \n {flushed}$ flushed";
            kokButtonUnlockPrice = 100;
            multiplicationBonus = 0;
            effectInfo = "???";
            showTheLine = false;
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
            flushed += MoneyManagerSingleton.instance.GetMoney();
            SaveManager.instance.UpdateFlushed(flushed);
            Social.ReportScore((long)(SaveManager.instance.GetFlushed()/1000000),
                GPGSIds.leaderboard_flushed, success => { });
            MoneyManagerSingleton.instance.SpendMoney(MoneyManagerSingleton.instance.GetMoney());
            kokButtonDescription = $"Flush everything down  \n \n {Helpers.ConvertNumbersToString(flushed)}$ flushed";
            UpdateUpgradePriceDisplayText("");
        }
    }
}