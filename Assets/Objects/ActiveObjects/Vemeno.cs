using Objects.Abstract.UnlockableObjectClasses;

namespace Objects.ActiveObjects
{
    public class Vemeno : ActiveKokTreeObject
    {
        public Vemeno()
        {
            objectName = "Vemeno";
            description = "ANOTHER ONE?";
            kokButtonDescription = "This does not seem natural";
            shopButtonBuyPrice = 15;
            kokButtonUnlockPrice = 5;
            productionPower = 1f;
            interval = 1f;
        }
    }
}