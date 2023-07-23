using Objects.UnlockableObjectClasses;

namespace Objects
{
    public class Bedding : UnlockableObject
    {
        public Bedding()
        {
            objectName = "Bedding";
            description = "I SLEP";
            kokButtonDescription = "Such luxury - they can sleep on their food";
            shopButtonBuyPrice = 15;
            kokButtonUnlockPrice = 5;
            productionPower = 1f;
            interval = 1f;
        }
    }
}