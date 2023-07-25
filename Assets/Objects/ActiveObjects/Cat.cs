
using Objects.Abstract.ActiveObjectClasses;

namespace Objects.ActiveObjects
{
    public class Cat : ActiveKokTreeObject
    {
        public Cat()
        {
            kokButtonStatus = ButtonStatus.AVAILABLE;
            shopButtonBuyPrice = 2;
            kokButtonUnlockPrice = 3;
            objectName = "Jeremy";
            description =
                "Look at this cat and don't mind that your competition started disappearing. You" +
                " can get more of his friends to help.";
            kokButtonDescription = "You find a little cat. Try to take it home, but it requires a fee..." +
                                   "\n \n What use is coin for him?";
            productionPower = 0.5f;
            interval = 0.5f;
        }
    }
}