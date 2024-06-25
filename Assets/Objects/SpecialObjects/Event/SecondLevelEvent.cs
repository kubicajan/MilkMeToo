using Managers;
using Objects.Abstract;
using UnityEngine;

namespace Objects.SpecialObjects.Event
{
    public class SecondLevelEvent : KokTreeObject
    {
        private ParticleSystem suckParticleSystem;

        public SecondLevelEvent()
        {
            objectName = "God among men";
            kokButtonDescription = "The fabric of reality shatters, what is HE doing here?";
            kokButtonUnlockPrice = 850000;
            effectInfo = "SPECIAL EVENTS";
            kokButtonStatus = ButtonStatus.UNKNOWN;
            availableParticleName = "SuckParticle";
            boughtParticleName = "VoidParticle";
        }
        
        protected override void Start()
        {
            // gameObject.SetActive(true);
            base.Start();
        }

        protected override void ResetHandler()
        {
            base.ResetHandler();
            kokButtonStatus = ButtonStatus.AVAILABLE;
            KokTreeButtonStart();
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


        protected override void UnlockAnotherButton()
        {
            return;
        }

        protected override void KokTreeButtonStart()
        {
            base.KokTreeButtonStart();
            if (this.kokButtonStatus == ButtonStatus.BOUGHT)
            {
                EventManager.instance.martyr();
            }
        }
    }
}