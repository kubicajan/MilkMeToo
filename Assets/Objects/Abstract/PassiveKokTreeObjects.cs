using Managers;
using PopUps;

namespace Objects.Abstract
{
    public abstract class PassiveKokTreeObjects : KokTreeObject
    {
        private int multiplicationBonus = 10;

        protected override void Start()
        {
            base.Start();
            effectInfo = "EXTRA PRODUCTION";
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
            primalSpriteButton.gameObject.SetActive(true);
        }
    }
}