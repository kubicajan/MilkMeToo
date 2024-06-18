using System;
using Managers;
using Objects.Abstract.ActiveObjectClasses;
using Utilities;
using UnityEngine;
using Random = UnityEngine.Random;


namespace Objects.ActiveObjects
{
    public class Cat : ActiveKokTreeObject
    {
        [SerializeField] private AudioClip animalNoise2;

        [SerializeField] private AudioClip animalNoise3;

        [SerializeField] private AudioClip animalNoise4;

        public Cat()
        {
            kokButtonStatus = ButtonStatus.AVAILABLE;

            shopButtonBuyPrice = 5;
            kokButtonUnlockPrice = 3;
            objectName = "Jeremy";
            description =
                "Look at this cat and don't mind that your competition started disappearing. You" +
                " can get more of his friends to help.";
            kokButtonDescription = "You find a little cat. Try to take it home, but it requires a fee..." +
                                   "\n \n What use is coin for him?";
            productionPower = 0.1m;
            interval = 0.5f;
        }

        protected override void ResetHandler()
        {
            base.ResetHandler();
            kokButtonStatus = ButtonStatus.AVAILABLE;
            KokTreeButtonStart();
        }

        protected override void PlayNoise()
        {
            int randomNumber = Random.Range(1, 5); // 5 is exclusive, so it will generate numbers from 1 to 4 inclusive

            switch (randomNumber)
            {
                case 1:
                    animalNoiseAudioSource.PlayOneShot(animalNoise);
                    break;
                case 2:
                    animalNoiseAudioSource.PlayOneShot(animalNoise2);
                    break;
                case 3:
                    animalNoiseAudioSource.PlayOneShot(animalNoise3);
                    break;
                case 4:
                    animalNoiseAudioSource.PlayOneShot(animalNoise4);
                    break;
            }
        }

        protected override void ConfigureAndPlayMilked(Transform transformMe)
        {
            Decimal moneyMoney = (Decimal)objectCounter * productionPower *
                                 (Decimal)(MoneyManagerSingleton.instance.numberOfTitties + 1);
            StartCoroutine(PlayMilkedCoroutine(transformMe,
                Helpers.GetObjectPositionRelativeToCanvas(transformMe.position), moneyMoney));
        }
    }
}