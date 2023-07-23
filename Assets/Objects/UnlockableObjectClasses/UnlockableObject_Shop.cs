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
                    primalSpriteButton.gameObject.SetActive(true);
                    counter.text = _count.ToString();
                }
                else
                {
                    _count = 0;
                    primalSpriteButton.gameObject.SetActive(false);
                    counter.text = "";
                }
            }
        }

        private void ShopButtonStart()
        {
            UpdateShopButton(false, "unavailable", "", Color.gray);
        }

        private void UnlockShopButton()
        {
            UpdateShopButton(true, objectName, shopButtonBuyPrice.ToString(), Color.cyan);
        }
        
        private void NoMoneyShopButton(float money)
        {
            UpdateShopButton(false, "NOT enough funds", shopButtonBuyPrice.ToString(), Color.magenta);
        }

        private void UpdateShopButton(bool enabled, string buttonNameText, string buttonPriceText, Color colour)
        {
            shopButton.enabled = enabled;
            shopButton.transform.Find("ButtonName").GetComponent<TextMeshProUGUI>().text = buttonNameText;
            shopButton.transform.Find("ButtonPrice").GetComponent<TextMeshProUGUI>().text = buttonPriceText;
            shopButton.image.color = colour;
        }

        public void BuyObject()
        {
            if (MoneyManagerSingleton.instance.SpendMoney(shopButtonBuyPrice))
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