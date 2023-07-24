using Objects.Abstract.UnlockableObjectClasses;

namespace Objects
{
    public class Capy : ActiveKokTreeObject
    {
        public Capy()
        {
            objectName = "Capy";
            description =
                "He brought his whole family. They demand confiscation of your property, but will keep working while" +
                "you keep importing more of them. For now.";
            kokButtonDescription =
                "The chillest animal on the block. He sometimes utters something about a grand revolution and the " +
                "bourgeoisie. He will join you when you share your wealth with him.";
            shopButtonBuyPrice = 15;
            kokButtonUnlockPrice = 5;
            productionPower = 1f;
            interval = 1f;
        }
    }
}