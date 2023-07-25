
using Objects.Abstract.ActiveObjectClasses;

namespace Objects
{
    public class EventUnlock : ActiveKokTreeObject
    {
        public EventUnlock()
        {
            objectName = "Event";
            description = "HUUUUUU";
            kokButtonDescription = "EventUnlock";
            shopButtonBuyPrice = 15;
            kokButtonUnlockPrice = 5;
            productionPower = 1f;
            interval = 1f;
        }
    }
}