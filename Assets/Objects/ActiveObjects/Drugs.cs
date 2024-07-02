using System;
using System.Collections;
using Managers;
using Objects.Abstract.ActiveObjectClasses;
using UnityEngine;
using UnityEngine.UI;
using Utilities;
using Object = System.Object;
using Random = UnityEngine.Random;

namespace Objects.ActiveObjects
{
    public class Drugs : ActiveKokTreeObject
    {
        [SerializeField] public Slider slider;
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioClip jingle;

        private int bonus = 30;
        private bool bonusIsOn;
        private bool turnItOn;
        private int MAX_SLIDER_VALUE = 12;
        protected ParticleSystem milkExplosion;
        public bool onMilkingScreen = false;

        public Drugs()
        {
            objectName = "Drugs";
            description = "NOM";
            kokButtonDescription = "Everyone should experience it at least once. Or twice. Or all the time. \n \n" +
                                   "Gives temporary boost to production";
            shopButtonBuyPrice = 5000;
            kokButtonUnlockPrice = 15000;
            productionPower = 1;
            interval = 1f;
        }

        protected override void Start()
        {
            audioSource.clip = jingle;
            base.Start();
            milkExplosion = Instantiate(system, primalSpriteButton.transform);
            milkExplosion.transform.position = primalSpriteButton.transform.position;
            milkExplosion.Play();
            slider.maxValue = MAX_SLIDER_VALUE;
        }

        // protected override void FixedUpdate()
        // {
        //     NabijeciSystemTepleVody();
        //     Decimal money = MoneyManagerSingleton.instance.GetMoney();
        //     UpdateShop(money);
        // }

        public override void BuyObject()
        {
            if (MoneyManagerSingleton.instance.SpendMoney(shopButtonBuyPrice))
            {
                ObjectCount++;
                bonusIsOn = true;
                turnItOn = true;
                shopButtonBuyPrice = CalculatePrice();
                SongManager.instance.PlayPurchase();
                SaveManager.instance.UpdateCountBoughtWrapper(this.GetType().ToString(), 1);
                SaveManager.instance.UpdateShopBuyPriceWrapper(this.GetType().ToString(), shopButtonBuyPrice);

                StartCoroutine(WaitUntilOnCowMilkingScreen());
            }
        }

        IEnumerator WaitUntilOnCowMilkingScreen()
        {
            yield return new WaitUntil(() => onMilkingScreen);
            StartCoroutine(StartBonusProduction());
            StartCoroutine(AddMultiplierCoroutine());
        }


        protected override void UpdateShop(Decimal money)
        {
            if (bonusIsOn)
            {
                UpdateShopButton(false, shopDefaultName, "ACTIVATED");
            }
            else
            {
                base.UpdateShop(money);
            }
        }

        protected override void ActivateThings(int value)
        {
            objectCounter = value > 0 ? value : 0;
        }


        IEnumerator AddMultiplierCoroutine()
        {
            double tmpBonus = bonus + (ObjectCount * 5);
            double stepBonus = (tmpBonus / (MAX_SLIDER_VALUE)) / 4;
            double count = 0;

            while (bonusIsOn)
            {
                MoneyManagerSingleton.instance.RaiseTemporaryMultiplication2(stepBonus);
                MilkMoneySingleton.instance.HandleMilkMoneyShow((decimal)stepBonus, MoveItABit(
                    Helpers.GetObjectPositionRelativeToCanvas(primalSpriteButton.gameObject.transform.position)), "X");

                count = stepBonus + count;
                yield return new WaitForSeconds(0.25f);
            }

            yield return new WaitForSeconds(0.4f);

            MilkMoneySingleton.instance.HandleMilkMoneyShow((decimal)-tmpBonus,
                Helpers.GetObjectPositionRelativeToCanvas(primalSpriteButton.gameObject.transform.position), "X");
            MoneyManagerSingleton.instance.RaiseTemporaryMultiplication2(-tmpBonus);
            MoneyManagerSingleton.instance.SettemporaryMultiplicationPilulkyToZero();
            MoneyManagerSingleton.instance.ClearStreakDisplayColour();
        }


        private Vector2 MoveItABit(Vector2 position)
        {
            Random.seed = System.DateTime.Now.Millisecond;

            float randomY = Random.Range(5f, 25f);
            float randomX = Random.Range(10f, 100f);
            float makeNegativeOrNotY = Random.Range(0f, 1f) < 0.5 ? -1 : 1;
            float makeNegativeOrNotX = Random.Range(0f, 1f) < 0.5 ? -1 : 1;

            float moveY = randomY * makeNegativeOrNotY;
            float moveX = randomX * makeNegativeOrNotX;

            return position - new Vector2(+35f + (moveX), -140f + (moveY));
        }

        private IEnumerator StartBonusProduction()
        {
            primalSpriteButton.gameObject.SetActive(true);

            audioSource.Play();

            float timer = 0f;
            while (timer <= MAX_SLIDER_VALUE)
            {
                timer += Time.deltaTime;
                slider.value = timer;
                yield return null;
            }

            // ObjectCount = 0;
            bonusIsOn = false;
            audioSource.Stop();
            primalSpriteButton.gameObject.SetActive(false);
        }
    }
}