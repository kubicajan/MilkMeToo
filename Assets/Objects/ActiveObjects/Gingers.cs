using Objects.Abstract.ActiveObjectClasses;
using UnityEngine;
using UnityEngine.UI;

namespace Objects.ActiveObjects
{
    public class Gingers : ActiveKokTreeObject
    {
        [SerializeField] private Button anotherGinger;

        protected override void ActivateThings(int value)
        {
            base.ActivateThings(value);
            
            if (value > 2)
            {
                anotherGinger.gameObject.SetActive(true);
            }
            else if (value <= 0)
            {
                anotherGinger.gameObject.SetActive(false);
            }
        }

        protected override void Start()
        {
            base.Start();
            anotherGinger.gameObject.SetActive(false);
        }

        public Gingers()
        {
            objectName = "Gingers";
            description = "Pact with the devil";
            kokButtonDescription = "They do not have souls anyway";
            shopButtonBuyPrice = 15;
            kokButtonUnlockPrice = 5;
            productionPower = 1f;
            interval = 1f;
        }
    }
}