using System.Linq;
using GooglePlayGames;
using Managers;
using Objects.Abstract;
using PopUps;
using UnityEngine;

namespace Objects.SpecialObjects.Event
{
    public class EventUnlock : KokTreeObject
    {
        private bool mamho = false;

        public EventUnlock()
        {
            objectName = "NASTENKA";
            kokButtonDescription = "Is this a witcher 1 reference";
            kokButtonUnlockPrice = 500;
            effectInfo = "UNLOCKS EVENTS";
            kokButtonStatus = ButtonStatus.AVAILABLE;
            // PlayGamesPlatform.Instance
            //     .LoadAchievements(achievements =>
            //     {
            //         mamho = achievements
            //             .Where(achivement => achivement.id == GPGSIds.achievement_the_idol)
            //             .Any(ach => ach.completed);
            //     });
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
            EventManager.instance.SpawnEvent();
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

            StartCoroutine(EventManager.instance.LevelUpCoroutine());
            gameObject.SetActive(false);
        }
    }
}