using Managers;
using Objects.Abstract.ActiveObjectClasses;

namespace Objects.SpecialObjects
{
    public class Cows : ActiveKokTreeObject
    {
        protected override int ObjectCount
        {
            get => objectCounter;
            set
            {
                if (value > 1)
                {
                    objectCounter = value;
                    primalSpriteButton.gameObject.SetActive(true);
                }
                else
                {
                    objectCounter = 1;
                    primalSpriteButton.gameObject.SetActive(false);
                }
            }
        }

        public Cows()
        {
            objectCounter = 1;
            effectInfo = "CLICK UPGRADE";
            objectName = "Cows";
            description = "It is like you are milking them all at once";
            kokButtonDescription = "You can get more of them?";
            shopButtonBuyPrice = 15;
            kokButtonUnlockPrice = 5;
            productionPower = 1;
        }

        public void BirthACow(int value)
        {
            ObjectCount += value;
        }

        public void MilkMe()
        {
            float money = (productionPower * ObjectCount);
            MoneyManagerSingleton.instance.AddMoney(money);
            AddToAllTimeMilked(money);
        }

        protected override void ProduceMilk()
        {
        }
    }
}