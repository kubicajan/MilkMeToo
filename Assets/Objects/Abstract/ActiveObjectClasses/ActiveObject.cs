using Managers;
using PopUps;
using UnityEngine;
using Utilities;

namespace Objects.Abstract.ActiveObjectClasses
{
    public abstract partial class ActiveKokTreeObject : KokTreeObject
    {
        [SerializeField] public Sprite shopButtonSprite;

        private float timer = 0f;
        private string allTimeMilked = "0.0";

        protected float interval = 1f;
        protected float productionPower = 0;
        protected string description = "";

        protected override void Start()
        {
            base.Start();
            effectInfo = "SHOP UPGRADE";
            ShopButtonStart();
        }


        protected override void Update()
        {
            base.Update();
            float money = MoneyManagerSingleton.instance.GetMoney();
            UpdateShop(money);

            if (primalSpriteButton.gameObject.activeSelf && IsItTime())
            {
                ProduceMilk();
            }
            
            if (InformationPopUp.instance.isActiveAndEnabled && clickedInfo)
            {
                this.Clicked();
            }
            else
            {
                clickedInfo = false;
            }
        }

        private bool IsItTime()
        {
            timer += Time.deltaTime;
            return timer >= interval;
        }

        public override void Clicked()
        {
            base.Clicked();
            InformationPopUp.instance.ShowPopUp(objectName, description, $"Amount milked:\n{allTimeMilked}",
                primalSpriteButton.image.sprite, $"{objectCounter}x");
        }

        private void ProduceMilk()
        {
            float finalPoints = MoneyManagerSingleton.instance.AddMoney(objectCounter * productionPower);
            AddToAllTimeMilked(finalPoints);
            MilkMoneySingleton.instance.HandleMilkMoneyShow(finalPoints, spriteCanvasPosition);
            timer = 0f;
        }

        private void AddToAllTimeMilked(float points)
        {
            allTimeMilked = (float.Parse(allTimeMilked) + points).ToString();
        }
    }
}