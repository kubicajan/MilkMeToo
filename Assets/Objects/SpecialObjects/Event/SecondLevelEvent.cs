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
            kokButtonUnlockPrice = 50000;
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

        public override void BuyUpgrade()
        {
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