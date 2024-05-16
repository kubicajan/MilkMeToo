using Managers;

namespace Objects.Abstract
{
    public abstract class PassiveKokTreeObjects : KokTreeObject
    {
        private int multiplicationBonus = 10;

        protected override void Start()
        {
            base.Start();
            effectInfo = $"{multiplicationBonus}% EXTRA PRODUCTION";
            if (SaveManager.instance.GetItemToUpdate(this.GetType().ToString()).KokTreeStatus == ButtonStatus.BOUGHT)
            {
                primalSpriteButton.gameObject.SetActive(true);
            }
        }

        protected override void UnlockAnotherButton()
        {
            toUnlockNext.GetComponent<KokTreeObject>().LockButton();
            MoneyManagerSingleton.instance.RaiseMultiplicationBy(multiplicationBonus);
        }

        // public override void Clicked()
        // {
        //     //todo: DO THIS ONLY FOR THOSE WHO ARE CLOSE TO COW
        //     MoneyManagerSingleton.instance.AddMoney(1);
        // }

        public override void BuyUpgrade()
        {
            base.BuyUpgrade();
            ActivateAllOfThem();
        }

        protected virtual void ActivateAllOfThem()
        {
            primalSpriteButton.gameObject.SetActive(true);
        }
    }
}