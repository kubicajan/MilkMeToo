using System;
using Managers;
using Objects.Abstract;

namespace Objects.PassiveObjects
{
    public class MultiplierBonus : PassiveKokTreeObjects
    {
        public static double permanentBonus = 10;
        private double permaMultiplier = 1.2;
        private int counter = 0;
        private Decimal originalPrice;
        private int maxBumbo = 5;

        public MultiplierBonus()
        {
            objectName = "Eat concrete";
            kokButtonUnlockPrice = 35000;
            multiplicationBonus = 20;
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

            double tmpBonus = permaMultiplier * counter;
            if (tmpPrice != 0)
            {
                permanentBonus = tmpBonus;
            }

            effectInfo = $"{counter}/{maxBumbo} bought";
            kokButtonDescription =
                $"Make it last! \n \n Gain {permaMultiplier}% of event multiplier permanently, per upgrade! \n \n Currently {permanentBonus}% ";
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
            SaveManager.instance.UpdateShopBuyPriceWrapper(this.GetType().ToString(), kokButtonUnlockPrice);
            effectInfo = $"{counter}/{maxBumbo} bought";

            // kokButtonUnlockPrice = originalkokUnlockPrice;
            // kokButtonUnlockPrice = originalkokUnlockPrice *
            //                        (Mommy.magicResetValue * SaveManager.instance.wrapper.timesProud);
            // kokButtonUnlockPrice = kokButtonUnlockPrice + ((kokButtonUnlockPrice * 20) / 100);
            // SaveManager.instance.RestartCountBoughtWrapper(this.GetType().ToString());
            // primalSpriteButton.SetActive(false);
            // this.StopAllCoroutines();
        }

        public override void BuyUpgrade()
        {
            permanentBonus += permaMultiplier;
            counter++;
            SaveManager.instance.UpdateCountBoughtWrapper(this.GetType().ToString(), counter);
            kokButtonUnlockPrice *= 2;
            UpdateUpgradePriceDisplayText(kokButtonUnlockPrice);
            SaveManager.instance.UpdateShopBuyPriceWrapper(this.GetType().ToString(), kokButtonUnlockPrice);
            kokButtonDescription =
                $"Make it last! \n \n Keep extra {permaMultiplier}% of event multiplier permanently per upgrade! \n \n Currently {permanentBonus}% ";
            effectInfo = $"{counter}/{maxBumbo} bought";
            UpdateUpgradePriceDisplayText(kokButtonUnlockPrice);

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