using Managers;
using PopUps;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utilities;

namespace Objects.Abstract.UnlockableObjectClasses
{
    public abstract partial class ActiveKokTreeObject : KokTreeObject
    {
        [SerializeField] public Button primalSpriteButton;
        [SerializeField] public Sprite shopButtonSprite;

        private Vector2 spriteCanvasPosition;
        private float timer = 0f;
        private string allTimeMilked = "0.0";
        private bool clickedInfo;

        protected float interval = 1f;
        protected float productionPower = 0;
        protected string description = "";

        protected override void Start()
        {
            base.Start();
            InformationPopUp.OnSetInactiveTriggered += OnSetInactiveTriggeredHandler;
            InitiateFields();
            primalSpriteButton.gameObject.SetActive(false);
            effectInfo = "SHOP UPGRADE";
            ShopButtonStart();
            clickedInfo = false;
        }

        private void InitiateFields()
        {
            spriteCanvasPosition = Helpers.GetObjectPositionRelativeToCanvas(primalSpriteButton.gameObject);
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

        public void Clicked()
        {
            clickedInfo = true;
            InformationPopUp.instance.ShowPopUp(objectName, description, allTimeMilked,
                primalSpriteButton.image.sprite, objectCounter.ToString());
        }

        private void OnSetInactiveTriggeredHandler()
        {
            clickedInfo = false;
        }
    }
}