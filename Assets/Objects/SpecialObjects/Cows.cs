using Managers;
using Objects.Abstract.ActiveObjectClasses;
using UnityEngine;
using UnityEngine.UI;

namespace Objects.SpecialObjects
{
    public class Cows : ActiveKokTreeObject
    {
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioClip click;
        [SerializeField] private Button anotherCow;
        [SerializeField] private Button anotherAnotherCow;

        protected override void ActivateThings(int value)
        {
            if (value > 1)
            {
                objectCounter = value;
                primalSpriteButton.gameObject.SetActive(true);

                if (value > 2)
                {
                    anotherCow.gameObject.SetActive(true);
                    if (value > 3)
                    {
                        anotherAnotherCow.gameObject.SetActive(true);
                    }
                }
            }
            else
            {
                objectCounter = 1;
                primalSpriteButton.gameObject.SetActive(false);
                anotherCow.gameObject.SetActive(false);
                anotherAnotherCow.gameObject.SetActive(false);
            }
        }

        protected override void Start()
        {
            base.Start();
            audioSource.clip = click;
            anotherCow.gameObject.SetActive(false);
            anotherAnotherCow.gameObject.SetActive(false);
        }

        public Cows()
        {
            objectCounter = 1;
            effectInfo = "CLICK UPGRADE";
            objectName = "Cows";
            description = "It is like you are milking them all at once";
            kokButtonDescription = "You can get more of them?";
            shopButtonBuyPrice = 15;
            kokButtonUnlockPrice = 5;
            productionPower = 1;
        }

        public void BirthACow(int value)
        {
            ObjectCount += value;
        }

        public void MilkMe()
        {
            audioSource.PlayOneShot(click);
            float money = (productionPower * ObjectCount);
            MoneyManagerSingleton.instance.AddMoney(money);
            AddToAllTimeMilked(money);

            // PlayMilkedCoroutine(money ,this.transform.position);
        }

        protected override void ProduceMilk()
        {
        }
    }
}