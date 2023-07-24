
using Objects.Abstract.UnlockableObjectClasses;

namespace Objects
{
    public class Cat : ActiveKokTreeObject
    {
        public Cat()
        {
            kokButtonStatus = ButtonStatus.AVAILABLE;
            shopButtonBuyPrice = 2;
            kokButtonUnlockPrice = 3;
            objectName = "Jeremy";
            description = "He is here to help you make you more profit. Don't mind the barrels he brought with him." +
                          " Or his friends. They are not robbing you.";
            kokButtonDescription = "You find a little cat. You try to take it home, but it requires a fee." +
                                   " What use is coin for him?";
            productionPower = 0.5f;
            interval = 0.5f;
        }
    }
}