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
            kokButtonUnlockPrice = 500;
            effectInfo = "SPECIAL EVENTS";
            kokButtonStatus = ButtonStatus.AVAILABLE;
            availableParticleName = "SuckParticle";
            boughtParticleName = "VoidParticle";
        }

        protected override void Start()
        {
            base.Start();
            gameObject.SetActive(false);
        }

        public override void BuyUpgrade()
        {
            base.BuyUpgrade();
            StartCoroutine(EventManager.instance.LevelUpCoroutine());
        }
    }
}