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
            effectInfo = $"{counter}/5 bought";
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
                $"Warp time and space itself - events appear {timerMultiplication}s faster per upgrade! \n \n  Currently every: {EventManager.instance.interval - (timeBonus)}s";
            effectInfo = $"{counter}/5 bought";

            if (counter >= 5)
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