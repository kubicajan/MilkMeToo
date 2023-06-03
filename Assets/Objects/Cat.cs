using Objects.UnlockableObjectClasses;

namespace Objects
{
    public class Cat : UnlockableObject
    {
        public Cat()
        {
            kokButtonStatus = ButtonStatus.AVAILABLE;
            shopButtonBuyPrice = 2;
            kokButtonUnlockPrice = 3;
            objectName = "cat";
        }
    }
}