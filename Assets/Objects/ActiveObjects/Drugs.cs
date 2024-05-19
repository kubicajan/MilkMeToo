using System.Collections;
using Managers;
using Objects.Abstract.ActiveObjectClasses;
using UnityEngine;
using UnityEngine.UI;

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
        private int MAX_SLIDER_VALUE = 10;
        protected ParticleSystem milkExplosion;

        public Drugs()
        {
            objectName = "Drugs";
            description = "NOM";
            kokButtonDescription = "Everyone should experience it at least once. Or twice. Or all the time. \n \n" +
                                   "Gives temporary boost to production";
            shopButtonBuyPrice = 15;
            kokButtonUnlockPrice = 5;
            productionPower = 1f;
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

        protected override void FixedUpdate()
        {
            NabijeciSystemTepleVody();
            float money = MoneyManagerSingleton.instance.GetMoney();
            UpdateShop(money);
        }

        public override void BuyObject()
        {
            if (MoneyManagerSingleton.instance.SpendMoney(shopButtonBuyPrice))
            {
                ObjectCount++;
                StartCoroutine(StartBonusProduction());
                turnItOn = true;
                shopButtonBuyPrice = CalculatePrice();
                SongManager.instance.PlayPurchase();
                SaveManager.instance.UpdateCountBoughtWrapper(this.GetType().ToString(), 1);
                SaveManager.instance.UpdateShopBuyPriceWrapper(this.GetType().ToString(), shopButtonBuyPrice);
            }
        }

        protected override void UpdateShop(float money)
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

        private IEnumerator StartBonusProduction()
        {
            primalSpriteButton.gameObject.SetActive(true);

            audioSource.Play();
            MoneyManagerSingleton.instance.RaiseMultiplicationBy(bonus);

            float timer = 0f;
            bonusIsOn = true;
            while (timer <= MAX_SLIDER_VALUE)
            {
                timer += Time.deltaTime;
                slider.value = timer;
                yield return null;
            }

            MoneyManagerSingleton.instance.RaiseMultiplicationBy(-bonus);
            ObjectCount = 0;
            bonusIsOn = false;
            audioSource.Stop();
            primalSpriteButton.gameObject.SetActive(false);
        }
    }
}