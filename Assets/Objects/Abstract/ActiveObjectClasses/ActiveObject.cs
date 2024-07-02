using System;
using System.Collections;
using System.Globalization;
using System.Numerics;
using Managers;
using PopUps;
using UnityEngine;
using UnityEngine.UI;
using Utilities;
using Vector2 = UnityEngine.Vector2;

namespace Objects.Abstract.ActiveObjectClasses
{
    public abstract partial class ActiveKokTreeObject : KokTreeObject
    {
        [SerializeField] public Sprite shopButtonSprite;
        [SerializeField] public ParticleSystem system;
        [SerializeField] public AudioClip animalNoise;
        [SerializeField] public AudioSource animalNoiseAudioSource;

        protected Decimal allTimeMilked = 0;
        protected float interval = 1f;
        protected Decimal productionPower = 0;
        protected string description = "";

        protected override void Start()
        {
            base.Start();
            originalPrice = shopButtonBuyPrice;
            effectInfo = "SHOP UPGRADE";
            ShopButtonStart();
            LoadData();
        }

        protected virtual void LoadData()
        {
            VyjimecnyElan data = SaveManager.instance.GetItemToUpdate(this.GetType().ToString());
            this.ObjectCount = data.CountBought;
            this.FreeObjects = data.FreeCount;
            Decimal gggg = Decimal.Parse(data.AmountMilked);
            this.allTimeMilked = gggg;
            Decimal ggtmp = Decimal.Parse(data.ShopBuyPrice);

            if (ggtmp != 0)
            {
                this.shopButtonBuyPrice = ggtmp;
            }
        }

        private void NabijeciSystemTepleVody()
        {
            Decimal money = MoneyManagerSingleton.instance.GetMoney();
            UpdateShop(money);

            if (InformationPopUp.instance.isActiveAndEnabled && clickedInfo)
            {
                this.Clicked();
            }
            else
            {
                clickedInfo = false;
            }
        }

        protected override void FixedUpdate()
        {
            NabijeciSystemTepleVody();
            base.FixedUpdate();
        }

        public override void Clicked()
        {
            clickedInfo = true;
            InformationPopUp.instance.ShowPopUp(objectName, description,
                "Amount milked:\n" + Helpers.ConvertNumbersToString(allTimeMilked),
                primalSpriteButton.GetComponent<Image>().sprite, $"{objectCounter + freeObjects}x");
        }

        public virtual void PlayMilked(int? number)
        {
            ConfigureAndPlayMilked(primalSpriteButton.gameObject.transform);
        }

        protected virtual void ConfigureAndPlayMilked(Transform transformMe)
        {
            Decimal moneyMoney = (objectCounter + freeObjects) * productionPower;
            StartCoroutine(PlayMilkedCoroutine(transformMe,
                Helpers.GetObjectPositionRelativeToCanvas(transformMe.position), moneyMoney));
        }

        protected IEnumerator PlayMilkedCoroutine(Transform transformMe, Vector2 showMilkPosition, Decimal moneyMoney)
        {
            yield return new WaitForSeconds(0.25f);
            PlayNoise();

            Decimal finalPoints = MoneyManagerSingleton.instance.AddMoney(moneyMoney);
            AddToAllTimeMilked(finalPoints);
            MilkMoneySingleton.instance.HandleMilkMoneyShow(finalPoints, showMilkPosition);

            ParticleSystem pSystem = Instantiate(system, transformMe);

            //this needs to be not converted whereas the other positions need to be converted
            pSystem.transform.position = transformMe.position;
            Destroy(pSystem.gameObject, 1.25f);
        }

        protected virtual void PlayNoise()
        {
            animalNoiseAudioSource.PlayOneShot(animalNoise);
        }

        private void AddToAllTimeMilked(Decimal points)
        {
            SaveManager.instance.UpdateAmountMilkedWrapper(this.GetType().ToString(), points);
            allTimeMilked += points; //(float.Parse(allTimeMilked) + points).ToString();
        }
    }
}