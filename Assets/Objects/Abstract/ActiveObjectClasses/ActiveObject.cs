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

        private float timer = 0f;
        private double allTimeMilked = 0;

        protected float interval = 1f;
        protected float productionPower = 0;
        protected string description = "";
        protected ParticleSystem milkExplosion;

        protected override void Start()
        {
            base.Start();
            milkExplosion = Instantiate(system, primalSpriteButton.transform);
            milkExplosion.transform.position = primalSpriteButton.transform.position;
            effectInfo = "SHOP UPGRADE";
            milkExplosion.Play();

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
            ProduceMilk();
        }

        private bool IsItTime()
        {
            timer += Time.deltaTime;
            if (timer > interval)
            {
                timer = 0;
                return true;
            }

            return false;
        }

        public override void Clicked()
        {
            clickedInfo = true;
            InformationPopUp.instance.ShowPopUp(objectName, description, "Amount milked:\n" + allTimeMilked,
                primalSpriteButton.image.sprite, $"{objectCounter}x");
        }

        protected virtual void ProduceMilk()
        {
            if (primalSpriteButton.gameObject.activeSelf)
            {
                if (IsItTime())
                {
                    float finalPoints = MoneyManagerSingleton.instance.AddMoney(objectCounter * productionPower);
                    AddToAllTimeMilked(finalPoints);
                    MilkMoneySingleton.instance.HandleMilkMoneyShow(finalPoints, spriteCanvasPosition);
                    timer = 0f;
                    milkExplosion.Play();
                }
            }
        }

        protected void AddToAllTimeMilked(float points)
        {
            allTimeMilked += points; //(float.Parse(allTimeMilked) + points).ToString();
        }
    }
}