using Managers;
using Objects.Abstract;
using PopUps;

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
    
        public override void Clicked()
        {
            base.Clicked();
            gameObject.SetActive(false);
        }
        
        protected override void ResetHandler()
        {
            base.ResetHandler();
            kokButtonStatus = ButtonStatus.AVAILABLE;
            RevertUpgrade();
            KokTreeButtonStart();
        }
        
        private void RevertUpgrade()
        {
            gameObject.SetActive(true);
            JsonParser.instance.ConfigureLevelOne();
            toUnlockNext.gameObject.SetActive(false);
            EventManager.instance.FirstConfigure();
            EventManager.instance.ShutItAllDown();
        }
        
        public override void BuyUpgrade()
        {
            base.BuyUpgrade();
            LevelUp();
        }

        protected override void KokTreeButtonStart()
        {
            base.KokTreeButtonStart();
            if (this.kokButtonStatus == ButtonStatus.BOUGHT)
            {
                LevelUp();

            }
        }

        private void LevelUp()
        {
            toUnlockNext.transform.position = gameObject.transform.position;
            toUnlockNext.gameObject.SetActive(true);
            StartCoroutine(EventManager.instance.LevelUpCoroutine());
            gameObject.SetActive(false);
        }
    }
}