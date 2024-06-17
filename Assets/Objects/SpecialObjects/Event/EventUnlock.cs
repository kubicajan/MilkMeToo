using Managers;
using Objects.Abstract;
using PopUps;
using UnityEngine;

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
                LevelUp(1);
            }
        }

        private void LevelUp(int level = 1)
        {
            toUnlockNext.transform.position = gameObject.transform.position;
            toUnlockNext.gameObject.SetActive(true);
            //TODO:    Social.ReportProgress(GPGSIds.achievement_the_idol, 100.0f, (bool success) => { });
            StartCoroutine(EventManager.instance.LevelUpCoroutine());
            gameObject.SetActive(false);
        }
    }
}