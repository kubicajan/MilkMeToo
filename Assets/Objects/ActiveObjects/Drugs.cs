using Objects.Abstract.UnlockableObjectClasses;

namespace Objects
{
    public class Drugs : ActiveKokTreeObject
    {
        public Drugs()
        {
            objectName = "Drugs";
            description = "NOM";
            kokButtonDescription = "Everyone should experience it at least once. Or twice. Or all the time.";
            shopButtonBuyPrice = 15;
            kokButtonUnlockPrice = 5;
            productionPower = 1f;
            interval = 1f;
        }
    }
}