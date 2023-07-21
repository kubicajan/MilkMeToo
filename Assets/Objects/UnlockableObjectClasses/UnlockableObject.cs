using Managers;
using TMPro;
using UnityEngine;
using Utilities;

namespace Objects.UnlockableObjectClasses
{
    public abstract partial class UnlockableObject : MonoBehaviour
    {
        [SerializeField] public GameObject sprite;
        [SerializeField] public TextMeshProUGUI counter;

        private Vector2 spriteCanvasPosition;
        private float timer = 0f;

        protected float interval = 1f;
        protected string objectName = "";
        protected float productionPower = 0;

        private void Start()
        {
            spriteCanvasPosition = Helpers.GetObjectPositionRelativeToCanvas(sprite);
            //TODO: DYNAMIC SETTING OF POSITION
            counter.rectTransform.anchoredPosition = new Vector2(spriteCanvasPosition.x + 300, spriteCanvasPosition.y - 150);
            sprite.SetActive(false);
            counter.text = "";
            ShopButtonStart();
            KokTreeButtonStart();
        }

        private void Update()
        {
            float money = MoneyManagerSingleton.instance.GetMoney();
            UpdateKokTree(money);
            UpdateShop(money);

            if (sprite.activeSelf && IsItTime())
            {
                ProduceMilk();
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
            MoneyManagerSingleton.instance.AddMoney(points);
            MilkMoneySingleton.instance.HandleMilkMoneyShow(points, spriteCanvasPosition);
            timer = 0f;
        }
    }
}