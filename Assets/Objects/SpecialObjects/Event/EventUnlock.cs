using Managers;
using Objects.Abstract;

namespace Objects.SpecialObjects.Event
{
    public class EventUnlock : KokTreeObject
    {
        public EventUnlock()
        {
            objectName = "NASTENKA";
            kokButtonDescription = "Is this a witcher 1 reference";
            kokButtonUnlockPrice = 5;
            effectInfo = "UNLOCKS EVENTS";
            kokButtonStatus = ButtonStatus.AVAILABLE;
        }

        public override void BuyUpgrade()
        {
            toUnlockNext.transform.position = gameObject.transform.position;
            toUnlockNext.gameObject.SetActive(true);
            base.BuyUpgrade();
            EventManager.instance.LevelUp();
            gameObject.SetActive(false);
        }
    }
}