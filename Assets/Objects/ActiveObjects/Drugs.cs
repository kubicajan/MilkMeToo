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

        private int bonus = 100;
        private bool bonusIsOn;
        private int MAX_SLIDER_VALUE = 15;

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
            milkExplosion.Play();
            slider.maxValue = MAX_SLIDER_VALUE;
        }

        protected override void FixedUpdate()
        {
            NabijeciSystemTepleVody();
            float money = MoneyManagerSingleton.instance.GetMoney();
            UpdateShop(money);
            if (!bonusIsOn && this.ObjectCount > 0)
            {
                StartCoroutine(StartBonusProduction());
            }
        }

        protected override void UpdateShop(float money)
        {
            if (objectCounter > 0)
            {
                UpdateShopButton(false, shopDefaultName, "ACTIVATED");
            }
            else
            {
                base.UpdateShop(money);
            }
        }

        IEnumerator StartBonusProduction()
        {
            audioSource.Play();
            bonusIsOn = true;
            MoneyManagerSingleton.instance.RaiseMultiplicationBy(bonus);

            float timer = 0f;
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
        }
    }
}