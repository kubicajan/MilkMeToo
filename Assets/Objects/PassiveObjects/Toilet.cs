using Managers;
using Objects.Abstract;
using Unity.VisualScripting;

namespace Objects.PassiveObjects
{
    public class Toilet : PassiveKokTreeObjects
    {
        public Toilet()
        {
            kokButtonStatus = ButtonStatus.AVAILABLE;
            objectName = "Toilet";
            kokButtonDescription = "Flush everything down";
            kokButtonUnlockPrice = 0;
            multiplicationBonus = 0;
        }

        protected override void LoadAllAssets()
        {

            if (SaveManager.instance.GetTimesProud() >= 1)
            {
                base.LoadAllAssets();
                this.gameObject.SetActive(true);;
            }
            else
            {
                this.gameObject.SetActive(false);;
            }
        }

        protected override void ResetHandler()
        {
        }

        public override void BuyUpgrade()
        {
            MoneyManagerSingleton.instance.SpendMoney(MoneyManagerSingleton.instance.GetMoney());
        }
    }
}