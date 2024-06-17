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

        public Gingers()
        {
            objectName = "Gingers";
            description = "Pact with the devil";
            kokButtonDescription = "They do not have souls anyway";
            shopButtonBuyPrice = 15;
            kokButtonUnlockPrice = 5;
            productionPower = 1;
            interval = 10f;
        }

        protected override void ActivateThings(int value)
        {
            base.ActivateThings(value);

            if (!mamho)
            {
         //todo
               // Social.ReportProgress(GPGSIds.achievement_its_a_zoo, 100.0f, (bool success) => { });
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
 
            //todo:
            // PlayGamesPlatform.Instance
            //     .LoadAchievements(achievements =>
            //     {
            //         mamho = achievements
            //             .Where(achivement => achivement.id == GPGSIds.achievement_its_a_zoo)
            //             .Any(ach => ach.completed);
            //     });
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
    }
}