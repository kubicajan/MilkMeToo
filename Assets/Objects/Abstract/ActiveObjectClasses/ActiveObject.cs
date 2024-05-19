using System;
using System.Collections;
using Managers;
using PopUps;
using UnityEngine;
using Utilities;

namespace Objects.Abstract.ActiveObjectClasses
{
    public abstract partial class ActiveKokTreeObject : KokTreeObject
    {
        [SerializeField] public Sprite shopButtonSprite;
        [SerializeField] public ParticleSystem system;
        [SerializeField] private AudioClip animalNoise;
        [SerializeField] private AudioSource animalNoiseAudioSource;

        protected Decimal allTimeMilked = 0;
        protected float interval = 1f;
        protected Decimal productionPower = 0;
        protected string description = "";

        protected override void Start()
        {
            base.Start();

            effectInfo = "SHOP UPGRADE";
            ShopButtonStart();
            LoadData();
            originalPrice = shopButtonBuyPrice;
        }

        protected virtual void LoadData()
        {
            VyjimecnyElan data = SaveManager.instance.GetItemToUpdate(this.GetType().ToString());
            this.ObjectCount = data.CountBought;
            Decimal gggg = Decimal.Parse(data.AmountMilked);
            this.allTimeMilked = gggg;
            Decimal ggtmp = Decimal.Parse(data.ShopBuyPrice);

            if (ggtmp != 0)
            {
                this.shopButtonBuyPrice = ggtmp;
            }
        }

        protected void NabijeciSystemTepleVody()
        {
            base.FixedUpdate();
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
        }

        public override void Clicked()
        {
            clickedInfo = true;
            InformationPopUp.instance.ShowPopUp(objectName, description, "Amount milked:\n" + allTimeMilked,
                primalSpriteButton.image.sprite, $"{objectCounter}x");
        }

        public virtual void PlayMilked(int? number)
        {
            ConfigureAndPlayMilked(primalSpriteButton.gameObject.transform);
        }

        protected virtual void ConfigureAndPlayMilked(Transform transformMe)
        {
            Decimal moneyMoney = objectCounter * productionPower;
            StartCoroutine(PlayMilkedCoroutine(transformMe,
                Helpers.GetObjectPositionRelativeToCanvas(transformMe.position), moneyMoney));
        }

        protected IEnumerator PlayMilkedCoroutine(Transform transformMe, Vector2 showMilkPosition, Decimal moneyMoney)
        {
            yield return new WaitForSeconds(0.25f);
            animalNoiseAudioSource.PlayOneShot(animalNoise);

            Decimal finalPoints = MoneyManagerSingleton.instance.AddMoney(moneyMoney);
            SaveManager.instance.UpdateAmountMilkedWrapper(this.GetType().ToString(), finalPoints);
            AddToAllTimeMilked(finalPoints);
            MilkMoneySingleton.instance.HandleMilkMoneyShow(finalPoints, showMilkPosition);

            ParticleSystem pSystem = Instantiate(system, transformMe);

            //this needs to be not converted whereas the other positions need to be converted
            pSystem.transform.position = transformMe.position;
            Destroy(pSystem.gameObject, 1.25f);
        }

        private void AddToAllTimeMilked(Decimal points)
        {
            allTimeMilked += (int)points; //(float.Parse(allTimeMilked) + points).ToString();
        }
    }
}