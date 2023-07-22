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
            productionPower = 0.5f;
            interval = 0.5f;
        }

        protected override void UnlockAnotherButton()
        {
            toUnlockNext.GetComponent<Capy>().LockButton();
        }
    }
}