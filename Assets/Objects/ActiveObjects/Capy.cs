using Objects.Abstract.ActiveObjectClasses;
using UnityEngine;
using UnityEngine.UI;
using Utilities;

namespace Objects.ActiveObjects
{
    public class Capy : ActiveKokTreeObject
    {
        [SerializeField] private Button anotherCapy;
        [SerializeField] private Button yetAnotherCapy;
        [SerializeField] private Button yetAnotherAnotherCapy;

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

        protected override void Start()
        {
            base.Start();
            anotherCapy.gameObject.SetActive(false);
            yetAnotherCapy.gameObject.SetActive(false);
            yetAnotherAnotherCapy.gameObject.SetActive(false);
        }

        protected override void ActivateThings(int value)
        {
            base.ActivateThings(value);

            yetAnotherAnotherCapy.gameObject.SetActive(value > 6);
            yetAnotherCapy.gameObject.SetActive(value > 4);
            anotherCapy.gameObject.SetActive(value > 2);
        }

        public override void PlayMilked(int? number)
        {
            if (number != null)
            {
                switch (number)
                {
                    case 1:
                        ConfigureAndPlayMilked(anotherCapy.transform);
                        break;
                    case 2:
                        ConfigureAndPlayMilked(yetAnotherCapy.transform);
                        break;
                    case 3:
                        ConfigureAndPlayMilked(yetAnotherAnotherCapy.transform);
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