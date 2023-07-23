
using Objects.Abstract.UnlockableObjectClasses;

namespace Objects
{
    public class Capy : ActiveKokTreeObject
    {
        public Capy()
        {
            objectName = "Capy";
            description = "The chillest animal on the block";
            kokButtonDescription = "not meow?";
            shopButtonBuyPrice = 15;
            kokButtonUnlockPrice = 5;
            productionPower = 1f;
            interval = 1f;
        }
    }
}