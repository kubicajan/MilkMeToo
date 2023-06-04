using Objects.UnlockableObjectClasses;
using Unity.VisualScripting;

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

        protected override void UnlockAnotherButton()
        {
            toUnlockNext.GetComponent<Capy>().LockButton();
        }
    }
}