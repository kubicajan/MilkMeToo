using System.Collections;
using Managers;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

namespace Objects.UnlockableObjectClasses
{
    public abstract partial class UnlockableObject : MonoBehaviour
    {
        [SerializeField] public GameObject sprite;
        [SerializeField] public TextMeshProUGUI counter;
        private float timer = 0f;
        protected float interval = 1f;
        protected string objectName = "";
        protected float productionPower = 0;

        private void Update()
        {
            float money = MoneyManager.instance.GetMoney();
            UpdateKokTree(money);
            UpdateShop(money);

            if (sprite.activeSelf && IsItTime())
            {
                ProduceMilk();
            }
        }

        private void Start()
        {
            sprite.SetActive(false);
            counter.text = "";
            ShopButtonStart();
            KokTreeButtonStart();
        }

        private bool IsItTime()
        {
            timer += Time.deltaTime;
            return timer >= interval;
        }

        private void ProduceMilk()
        {
            MoneyManager.instance.AddMoney(_count * productionPower);
            timer = 0f;
        }
    }
}