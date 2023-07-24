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
        private TextMeshProUGUI counter;

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
            counter.rectTransform.anchoredPosition =
                new Vector2(spriteCanvasPosition.x + 300, spriteCanvasPosition.y - 150);
            primalSpriteButton.gameObject.SetActive(false);
            counter.text = "";
            effectInfo = "SHOP UPGRADE";
            ShopButtonStart();
            clickedInfo = false;
        }

        private void InitiateFields()
        {
            spriteCanvasPosition = Helpers.GetObjectPositionRelativeToCanvas(primalSpriteButton.gameObject);
            counter = primalSpriteButton.transform.Find("Counter").GetComponent<TextMeshProUGUI>();
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
            float finalPoints = MoneyManagerSingleton.instance.AddMoney(_count * productionPower);
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
                primalSpriteButton.image.sprite);
        }

        private void OnSetInactiveTriggeredHandler()
        {
            clickedInfo = false;
        }
    }
}