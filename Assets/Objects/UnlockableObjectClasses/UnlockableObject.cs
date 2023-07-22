using System.Globalization;
using Managers;
using PopUps;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utilities;

namespace Objects.UnlockableObjectClasses
{
    public abstract partial class UnlockableObject : MonoBehaviour
    {
        [SerializeField] public Button primalSprite;
        [SerializeField] public TextMeshProUGUI counter;

        private Vector2 spriteCanvasPosition;
        private float timer = 0f;
        private string allTimeMilked = "0.0";
        private bool clickedInfo;

        protected float interval = 1f;
        protected string objectName = "";
        protected float productionPower = 0;
        protected string description = "";

        private void Start()
        {
            InformationPopUp.OnSetInactiveTriggered += OnSetInactiveTriggeredHandler;
            spriteCanvasPosition = Helpers.GetObjectPositionRelativeToCanvas(primalSprite.gameObject);
            counter.rectTransform.anchoredPosition =
                new Vector2(spriteCanvasPosition.x + 300, spriteCanvasPosition.y - 150);
            primalSprite.gameObject.SetActive(false);
            counter.text = "";
            ShopButtonStart();
            KokTreeButtonStart();
            clickedInfo = false;
        }

        private void Update()
        {
            float money = MoneyManagerSingleton.instance.GetMoney();
            UpdateKokTree(money);
            UpdateShop(money);

            if (primalSprite.gameObject.activeSelf && IsItTime())
            {
                ProduceMilk();
            }

            //todo: HOW to distinguish which object called it??? 
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
            float points = _count * productionPower;
            AddToAllTimeMilked(points);
            MoneyManagerSingleton.instance.AddMoney(points);
            MilkMoneySingleton.instance.HandleMilkMoneyShow(points, spriteCanvasPosition);
            timer = 0f;
        }

        private void AddToAllTimeMilked(float points)
        {
            allTimeMilked = (float.Parse(allTimeMilked) + points).ToString();
        }

        public void Clicked()
        {
            clickedInfo = true;
            InformationPopUp.instance.ShowPopUp(objectName, description, allTimeMilked);
        }

        private void OnSetInactiveTriggeredHandler()
        {
            clickedInfo = false;
        }
    }
}