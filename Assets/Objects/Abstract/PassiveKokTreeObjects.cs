using Managers;

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
    }
}