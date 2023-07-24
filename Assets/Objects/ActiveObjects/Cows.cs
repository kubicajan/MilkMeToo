using Objects.Abstract.UnlockableObjectClasses;

namespace Objects.ActiveObjects
{
    public class Cows : ActiveKokTreeObject
    {
        public Cows()
        {
            objectName = "Cows";
            description = "Interesting";
            kokButtonDescription = "You can get more of them?";
            shopButtonBuyPrice = 15;
            kokButtonUnlockPrice = 5;
            productionPower = 1f;
            interval = 1f;
        }
    }
}