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

            if (value > 2)
            {
                anotherGinger.gameObject.SetActive(true);
            }
            else if (value <= 2)
            {
                anotherGinger.gameObject.SetActive(false);
            }
        }

        protected override void Start()
        {
            base.Start();
            anotherGinger.gameObject.SetActive(false);
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