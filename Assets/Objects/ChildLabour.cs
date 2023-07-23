using Objects.UnlockableObjectClasses;

namespace Objects
{
    public class ChildLabour : UnlockableObject
    {
        public ChildLabour()
        {
            objectName = "Child Labour";
            description = "This is legal";
            kokButtonDescription = "Let everyone help";
            shopButtonBuyPrice = 15;
            kokButtonUnlockPrice = 5;
            productionPower = 1f;
            interval = 1f;
        }
    }
}