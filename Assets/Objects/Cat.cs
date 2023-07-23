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
            objectName = "Cat";
            description = "I am a kitty cat meow meow meow, give me your milk";
            kokButtonDescription = "meow";
            productionPower = 0.5f;
            interval = 0.5f;
        }
    }
}