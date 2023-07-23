using Objects.UnlockableObjectClasses;

namespace Objects
{
    public class TheLover : UnlockableObject
    {
        public TheLover()
        {
            objectName = "The Lover";
            description = "He looks like a gentleman";
            kokButtonDescription = "THAT is what I call a MALE";
            shopButtonBuyPrice = 15;
            kokButtonUnlockPrice = 5;
            productionPower = 1f;
            interval = 1f;
        }
    }
}