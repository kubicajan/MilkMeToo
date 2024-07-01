using System;
using Managers;
using Objects.Abstract;
using Objects.SpecialObjects.Event;

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
            kokButtonUnlockPrice = 100000;
            multiplicationBonus = 0;
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
        }

        public override void BuyUpgrade()
        {
            timeBonus += timerMultiplication;
            counter++;
            SaveManager.instance.UpdateCountBoughtWrapper(this.GetType().ToString(), counter);
            kokButtonUnlockPrice *= 2;
            UpdateUpgradePriceDisplayText(kokButtonUnlockPrice);
            SaveManager.instance.UpdateShopBuyPriceWrapper(this.GetType().ToString(), kokButtonUnlockPrice);
            kokButtonDescription =
                $"Warp time and space itself. \n \n Events appear {timerMultiplication}s faster per upgrade! \n \n  Event every: {EventManager.instance.interval - (timeBonus)}s";
            effectInfo = $"{counter}/{maxBumbo} bought";

            if (counter >= maxBumbo)
            {
                base.BuyUpgrade();
            }
        }

        protected override void UnlockAnotherButton()
        {
            return;
        }
    }
}