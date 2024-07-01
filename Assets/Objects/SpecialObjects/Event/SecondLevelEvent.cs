using Managers;
using Objects.Abstract;
using Objects.PassiveObjects;
using UnityEngine;

namespace Objects.SpecialObjects.Event
{
    public class SecondLevelEvent : KokTreeObject
    {
        private ParticleSystem suckParticleSystem;

        public SecondLevelEvent()
        {
            objectName = "God among men";
            kokButtonUnlockPrice = 850000;
            effectInfo = "SPECIAL EVENTS";
            kokButtonStatus = ButtonStatus.UNKNOWN;
            availableParticleName = "SuckParticle";
            boughtParticleName = "VoidParticle";
        }

        protected override void Start()
        {
            base.Start();
            kokButtonDescription =
                $"The fabric of reality shatters, what is HE doing here? \n \n Upgrades events.";
            if (SaveManager.instance.GetTimesProud() >= 1)
            {
                this.gameObject.SetActive(false);;
            }
            else
            {
                this.gameObject.SetActive(true);;
            }
        }

        protected override void ResetHandler()
        {
            this.transform.position = new Vector2(-1980, -500);
            UpdateUpgradePriceDisplayText("");

            // base.ResetHandler();
            // kokButtonStatus = ButtonStatus.AVAILABLE;
            // KokTreeButtonStart();
        }

        private bool mamho = false;

        public override void BuyUpgrade()
        {
            if (!mamho)
            {
                Social.ReportProgress(GPGSIds.achievement_the_idol, 100.0f, (bool success) => { });
                mamho = true;
            }

            base.BuyUpgrade();
            EventManager.instance.martyr();
        }

        protected override void KokTreeButtonStart()
        {
            base.KokTreeButtonStart();
            if (this.kokButtonStatus == ButtonStatus.BOUGHT)
            {
                EventManager.instance.martyr();
            }
            if (SaveManager.instance.GetTimesProud() >= 1)
            {
                this.gameObject.SetActive(false);;
            }
            else
            {
                this.gameObject.SetActive(true);;
            }
        }
    }
}