using System;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Objects.UnlockableObjectClasses
{
    public abstract partial class UnlockableObject
    {
        [SerializeField] public Button shopButton;

        protected int shopButtonBuyPrice = 10;

        private int _count = 0;

        private void UpdateShop(float money)
        {
            if (kokButtonStatus == ButtonStatus.BOUGHT)
            {
                if (money >= shopButtonBuyPrice)
                {
                    UnlockShopButton();
                }
                else
                {
                    NoMoneyShopButton(money);
                }
            }
        }

        private int ObjectCount
        {
            get => _count;
            set
            {
                if (value > 0)
                {
                    _count = value;
                    sprite.SetActive(true);
                    counter.text = _count.ToString();
                }
                else
                {
                    _count = 0;
                    sprite.SetActive(false);
                    counter.text = "";
                }
            }
        }

        private void ShopButtonStart()
        {
            shopButton.enabled = false;
            shopButton.GetComponentInChildren<TextMeshProUGUI>().text = "unavailable";
            shopButton.image.color = Color.gray;
        }

        private void UnlockShopButton()
        {
            shopButton.enabled = true;
            shopButton.GetComponentInChildren<TextMeshProUGUI>().text =
                "<sprite=0>" + "                         buy " + objectName + " " + shopButtonBuyPrice + "$";
            shopButton.image.color = Color.cyan;
        }


        private void NoMoneyShopButton(float money)
        {
            shopButton.enabled = false;
            shopButton.GetComponentInChildren<TextMeshProUGUI>().text =
                "<sprite=0>" + "                         MAKE "
                             + (shopButtonBuyPrice - money) + "$ MORE TO BUY " + objectName;
            shopButton.image.color = Color.green;
        }

        public void BuyObject()
        {
            if (MoneyManager.instance.SpendMoney(shopButtonBuyPrice))
            {
                ObjectCount++;
            }
        }

        private void RemoveBoughtObject(int amount)
        {
            ObjectCount -= amount;
        }
    }
}