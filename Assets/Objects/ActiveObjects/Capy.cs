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

        private ParticleSystem milkExplosion2;
        private ParticleSystem milkExplosion3;
        private ParticleSystem milkExplosion4;

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
            milkExplosion2 = Instantiate(system, anotherCapy.transform);
            milkExplosion2.transform.position = anotherCapy.transform.position;

            milkExplosion3 = Instantiate(system, yetAnotherCapy.transform);
            milkExplosion3.transform.position = yetAnotherCapy.transform.position;

            milkExplosion4 = Instantiate(system, yetAnotherAnotherCapy.transform);
            milkExplosion4.transform.position = yetAnotherAnotherCapy.transform.position;

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
                        StartCoroutine(PlayMilkedCoroutine(milkExplosion2,
                            Helpers.GetObjectPositionRelativeToCanvas(anotherCapy.transform.position)));
                        break;
                    case 2:
                        StartCoroutine(PlayMilkedCoroutine(milkExplosion3,
                            Helpers.GetObjectPositionRelativeToCanvas(yetAnotherCapy.transform.position)));
                        break;
                    case 3:
                        StartCoroutine(PlayMilkedCoroutine(milkExplosion4,
                            Helpers.GetObjectPositionRelativeToCanvas(yetAnotherAnotherCapy.transform.position)));
                        break;
                }
            }
            else
            {
                base.PlayMilked(null);
            }
        }
    }
}