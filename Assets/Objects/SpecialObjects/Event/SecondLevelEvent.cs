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
            kokButtonDescription = "The fabric of reality shatters, what is he doing here?";
            kokButtonUnlockPrice = 5;
            effectInfo = "SPECIAL EVENTS";
            kokButtonStatus = ButtonStatus.AVAILABLE;
            availableParticleName = "SuckParticle";
            boughtParticleName = "VoidParticle";
        }

        protected override void Start()
        {
            base.Start();
            // CreateSuckParticle();
            gameObject.SetActive(false);
        }

        public override void BuyUpgrade()
        {
            base.BuyUpgrade();
            StartCoroutine(EventManager.instance.LevelUpCoroutine());
        }


        protected override void MakeButtonAvailable()
        {
            base.MakeButtonAvailable();
            //   CreateSuckParticle();
        }

        // private void CreateSuckParticle()
        // {
        //     Destroy(suckParticleSystem);
        //     ParticleSystem particleSystemToCopy = GameObject.Find("SuckParticle").GetComponent<ParticleSystem>();
        //     suckParticleSystem = Instantiate(particleSystemToCopy, gameObject.transform, false);
        //     suckParticleSystem.transform.position = gameObject.transform.position;
        //     suckParticleSystem.Play();
        // }
    }
}