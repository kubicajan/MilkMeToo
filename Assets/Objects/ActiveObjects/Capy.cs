
using Objects.Abstract.ActiveObjectClasses;
using UnityEngine;
using UnityEngine.UI;

namespace Objects.ActiveObjects
{
    public class Capy : ActiveKokTreeObject
    {
        
        [SerializeField] private Button anotherCapy;

        protected override void ActivateThings(int value)
        {
            base.ActivateThings(value);
            
            if (value > 2)
            {
                anotherCapy.gameObject.SetActive(true);
            }
            else if (value <= 0)
            {
                anotherCapy.gameObject.SetActive(false);
            }
        }

        protected override void Start()
        {
            base.Start();
            anotherCapy.gameObject.SetActive(false);
        }

        public Capy()
        {
            objectName = "Just a little boy";
            description =
                "He brought his own family to where animals are equal. You can bring more of them.";
            kokButtonDescription =
                "The chillest animal on the block. Has funny rambles about some grand revolution and the " +
                "bourgeoisie.\n \n Perhaps he joins you for some of your wealth.";
            shopButtonBuyPrice = 15;
            kokButtonUnlockPrice = 5;
            productionPower = 1f;
            interval = 1f;
        }
    }
}