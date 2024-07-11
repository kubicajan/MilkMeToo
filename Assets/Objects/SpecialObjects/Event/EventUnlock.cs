using System;
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
        [SerializeField] public GameObject anotherToUnlockNext;

        public EventUnlock()
        {
            objectName = "NASTENKA";
            kokButtonDescription = "Is this a witcher 1 reference?";
            kokButtonUnlockPrice = 1500;
            effectInfo = "UNLOCKS EVENTS";
            kokButtonStatus = ButtonStatus.LOCKED;
            showTheLine = false;
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
            this.transform.position = new Vector2(-1980, -500);
            UpdateUpgradePriceDisplayText("");

            // base.ResetHandler();
            // kokButtonStatus = ButtonStatus.AVAILABLE;
            // RevertUpgrade();
            //KokTreeButtonStart();
        }

        protected override void Start()
        {
            base.Start();
            if (SaveManager.instance.GetTimesProud() >= 1)
            {
                this.gameObject.SetActive(false);
            }
            else if (this.kokButtonStatus != ButtonStatus.BOUGHT)
            {
                this.gameObject.SetActive(true);
            }
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
            anotherToUnlockNext.GetComponent<KokTreeObject>().LockButton();
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

            if (SaveManager.instance.GetTimesProud() >= 1)
            {
                this.toUnlockNext.SetActive(false);
                ;
            }
            else
            {
                this.toUnlockNext.SetActive(true);
                ;
            }

            gameObject.SetActive(false);
        }
    }
}