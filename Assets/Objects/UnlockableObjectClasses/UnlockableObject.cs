using System;
using Managers;
using TMPro;
using UnityEngine;

namespace Objects.UnlockableObjectClasses
{
    public abstract partial class UnlockableObject : MonoBehaviour
    {
        [SerializeField] public GameObject sprite;
        [SerializeField] public TextMeshProUGUI counter;

        protected string objectName = "";

        private void Update()
        {
            int money = MoneyManager.instance.GetMoney();
            UpdateKokTree(money);
            UpdateShop(money);
        }

        private void Start()
        {
            sprite.SetActive(false);
            counter.text = "";
            ShopButtonStart();
            KokTreeButtonStart();
        }
    }
}