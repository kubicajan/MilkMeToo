using Objects.Abstract;
using UnityEngine;
using UnityEngine.UI;

namespace Objects.SpecialObjects.Event
{
    public class EventUnlock : KokTreeObject
    {
        [SerializeField] public Button secondLevel;

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
            base.BuyUpgrade();
            gameObject.SetActive(false);
            secondLevel.transform.position = gameObject.transform.position;
            secondLevel.gameObject.SetActive(true);
        }
    }
}