using System;
using Managers;
using Objects.Abstract;
using UnityEngine;

namespace Objects.PassiveObjects
{
    public class TimeBonus : PassiveKokTreeObjects
    {
        public static float timeBonus = 0;
        private int timerMultiplication = 2;
        private int counter = 0;
        private Decimal originalPrice = 0;
        private int maxBumbo = 5;

        public TimeBonus()
        {
            objectName = "Another dimension";
            kokButtonUnlockPrice = 30000;
            multiplicationBonus = 0;
            showTheLine = false;
        }

        protected override void Start()
        {
            base.Start();
            originalPrice = kokButtonUnlockPrice;
            VyjimecnyElan data = SaveManager.instance.GetItemToUpdate(this.GetType().ToString());
            counter = data.CountBought;
            Decimal tmpPrice = Decimal.Parse(data.ShopBuyPrice);

            if (tmpPrice != 0)
            {
                kokButtonUnlockPrice = tmpPrice;
            }

            timeBonus = timerMultiplication * counter;

            kokButtonDescription =
                $"Warp time and space itself. \n \n Events appear {timerMultiplication}s faster per upgrade! \n \n  Event every: {EventManager.instance.interval - (timeBonus)}s";
            effectInfo = $"{counter}/{maxBumbo} bought";
        }

        protected override void LoadAllAssets()
        {
            base.LoadAllAssets();
            if (SaveManager.instance.GetTimesProud() >= 1)
            {
                maxBumbo = 10;
            }
        }
        
        protected override void ResetHandler()
        {
            LockButton();
            maxBumbo = 10;
            kokButtonUnlockPrice = kokButtonUnlockPrice * 100;
            effectInfo = $"{counter}/{maxBumbo} bought";

        }

        public override void BuyUpgrade()
        {
            timeBonus += timerMultiplication;
            counter++;
            SaveManager.instance.UpdateCountBoughtWrapper(this.GetType().ToString(), counter);
            kokButtonUnlockPrice *= 1.5m;
            UpdateUpgradePriceDisplayText(kokButtonUnlockPrice);
            SaveManager.instance.UpdateShopBuyPriceWrapper(this.GetType().ToString(), kokButtonUnlockPrice);
            kokButtonDescription =
                $"Warp time and space itself. \n \n Events appear {timerMultiplication}s faster per upgrade! \n \n  Event every: {EventManager.instance.interval - (timeBonus)}s";
            effectInfo = $"{counter}/{maxBumbo} bought";
            bool enoughMoney = MoneyManagerSingleton.instance.IsEnoughFunds(kokButtonUnlockPrice);
            UpdateKokTree(enoughMoney);
            UpdateUpgradePriceDisplayText(kokButtonUnlockPrice);

            if (counter >= maxBumbo)
            {
                base.BuyUpgrade();
                if (maxBumbo == 10)
                {
                    Social.ReportProgress(GPGSIds.achievement_time_lord, 100.0f, (bool success) => { });
                }
            }
        }

        protected override void UnlockAnotherButton()
        {
            return;
        }
    }
}