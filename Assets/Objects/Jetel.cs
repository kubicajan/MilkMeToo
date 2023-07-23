using Objects.UnlockableObjectClasses;

namespace Objects
{
    public class Jetel : UnlockableObject
    {
        public Jetel()
        {
            kokButtonStatus = ButtonStatus.AVAILABLE;
            objectName = "Jetel";
            description = "nom nom";
            kokButtonDescription = "Maybe it is time to start feeding your cows";
            shopButtonBuyPrice = 15;
            kokButtonUnlockPrice = 5;
            productionPower = 1f;
            interval = 1f;
        }
    }
}