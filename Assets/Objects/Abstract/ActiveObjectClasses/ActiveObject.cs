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

        protected override void FixedUpdate()
        {
            base.FixedUpdate();
            float money = MoneyManagerSingleton.instance.GetMoney();
            UpdateShop(money);
            ProduceMilk();

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
            InformationPopUp.instance.ShowPopUp(objectName, description, $"Amount milked:\n{allTimeMilked}",
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
                }
            }
        }

        protected void AddToAllTimeMilked(float points)
        {
            allTimeMilked = (float.Parse(allTimeMilked) + points).ToString();
        }
    }
}