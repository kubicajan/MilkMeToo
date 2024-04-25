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

        private float timer = 0f;
        private double allTimeMilked = 0;

        protected float interval = 1f;
        protected float productionPower = 0;
        protected string description = "";

        protected override void Start()
        {
            base.Start();
            originalPrice = shopButtonBuyPrice;

            effectInfo = "SHOP UPGRADE";
            ShopButtonStart();
        }

        protected void NabijeciSystemTepleVody()
        {
            base.FixedUpdate();
            float money = MoneyManagerSingleton.instance.GetMoney();
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
            float moneyMoney = objectCounter * productionPower;
            StartCoroutine(PlayMilkedCoroutine(transformMe,
                Helpers.GetObjectPositionRelativeToCanvas(transformMe.position), moneyMoney));
        }

        protected IEnumerator PlayMilkedCoroutine(Transform transformMe, Vector2 showMilkPosition, float moneyMoney)
        {
            yield return new WaitForSeconds(0.25f);
            animalNoiseAudioSource.PlayOneShot(animalNoise);
            float finalPoints = MoneyManagerSingleton.instance.AddMoney(moneyMoney);
            AddToAllTimeMilked(finalPoints);
            MilkMoneySingleton.instance.HandleMilkMoneyShow(finalPoints, showMilkPosition);
            
            ParticleSystem pSystem = Instantiate(system, transformMe);
            
            //this needs to be not converted whereas the other positions need to be converted
            pSystem.transform.position = transformMe.position;
            Destroy(pSystem.gameObject, 1.25f);
        }

        private void AddToAllTimeMilked(float points)
        {
            allTimeMilked += points; //(float.Parse(allTimeMilked) + points).ToString();
        }
    }
}