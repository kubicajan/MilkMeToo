using System.Linq;
using GooglePlayGames;
using Objects.Abstract.ActiveObjectClasses;
using UnityEngine;
using UnityEngine.UI;

namespace Objects.ActiveObjects
{
    public class Gingers : ActiveKokTreeObject
    {
        [SerializeField] private Button anotherGinger;

        [SerializeField] private AudioClip animalNoise2;

        [SerializeField] private AudioClip animalNoise3;

        [SerializeField] private AudioClip animalNoise4;

        [SerializeField] private AudioClip animalNoise5;

        [SerializeField] private AudioClip animalNoise6;

        [SerializeField] private AudioClip animalNoise7;

        [SerializeField] private AudioClip animalNoise8;

        public Gingers()
        {
            objectName = "Gingers";
            description = "Pact with the devil";
            kokButtonDescription = "They do not have souls anyway";
            shopButtonBuyPrice = 1000000;
            kokButtonUnlockPrice = 2000000;
            productionPower = 800;
            interval = 10f;
        }

        protected override void ActivateThings(int value)
        {
            base.ActivateThings(value);

            if (!mamho && value > 0)
            {
                Social.ReportProgress(GPGSIds.achievement_its_a_zoo, 100.0f, (bool success) => { });
            }

            if (value > 2)
            {
                anotherGinger.gameObject.SetActive(true);
            }
            else
            {
                anotherGinger.gameObject.SetActive(false);
            }
        }

        private bool mamho = false;

        protected override void Start()
        {
            anotherGinger.gameObject.SetActive(false);
            base.Start();

            PlayGamesPlatform.Instance
                .LoadAchievements(achievements =>
                {
                    mamho = achievements
                        .Where(achivement => achivement.id == GPGSIds.achievement_its_a_zoo)
                        .Any(ach => ach.completed);
                });
        }

        public override void PlayMilked(int? number)
        {
            if (number != null)
            {
                switch (number)
                {
                    case 1:
                        ConfigureAndPlayMilked(anotherGinger.transform);
                        break;
                }
            }
            else
            {
                ConfigureAndPlayMilked(primalSpriteButton.gameObject.transform);
            }
        }

        protected override void PlayNoise()
        {
            Random.seed = System.DateTime.Now.Millisecond;
            int randomNumber = Random.Range(1, 9); // 5 is exclusive, so it will generate numbers from 1 to 4 inclusive

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
                case 5:
                    animalNoiseAudioSource.PlayOneShot(animalNoise5);
                    break;
                case 6:
                    animalNoiseAudioSource.PlayOneShot(animalNoise6);
                    break;
                case 7:
                    animalNoiseAudioSource.PlayOneShot(animalNoise7);
                    break;
                case 8:
                    animalNoiseAudioSource.PlayOneShot(animalNoise8);
                    break;
            }
        }
    }
}