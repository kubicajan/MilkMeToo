using System;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Objects.Abstract.ActiveObjectClasses
{
    public abstract partial class ActiveKokTreeObject
    {
        [SerializeField] public Button shopButton;
        [SerializeField] private Sprite questionMarkBasicShop;

        protected int objectCounter = 0;
        protected string shopDefaultName;
        private float originalPrice = 0;

        protected float shopButtonBuyPrice = 0;

        protected int ObjectCount
        {
            get => objectCounter;
            set => ActivateThings(value);
        }

        protected override void ResetHandler()
        {
            base.ResetHandler();
            ShopButtonStart();
            CalculatePrice();
            shopButton.transform.Find("Image").GetComponent<Image>().sprite = questionMarkBasicShop;
            ObjectCount = 0;
            shopButtonBuyPrice = originalPrice;
            shopButtonBuyPrice *= Mommy.magicResetValue;
        }

        protected virtual void ActivateThings(int value)
        {
            if (value > 0)
            {
                objectCounter = value;
                primalSpriteButton.gameObject.SetActive(true);
            }
            else
            {
                objectCounter = 0;
                primalSpriteButton.gameObject.SetActive(false);
            }
        }

        protected virtual void UpdateShop(float money)
        {
            if (kokButtonStatus == ButtonStatus.BOUGHT)
            {
                shopButton.transform.Find("Image").GetComponent<Image>().sprite = shopButtonSprite;

                if (money >= shopButtonBuyPrice)
                {
                    UnlockShopButton();
                }
                else
                {
                    NoMoneyShopButton();
                }
            }
        }

        private float CalculatePrice()
        {
            return (int)(shopButtonBuyPrice * (float)Math.Pow(1.15f, objectCounter));
        }

        private void ShopButtonStart()
        {
            shopDefaultName = shopButton.transform.Find("ButtonName").GetComponent<TextMeshProUGUI>().text;
            UpdateShopButton(false, "???", "");
        }

        private void UnlockShopButton()
        {
            UpdateShopButton(true, shopDefaultName, shopButtonBuyPrice.ToString());
        }

        private void NoMoneyShopButton()
        {
            UpdateShopButton(false, shopDefaultName, shopButtonBuyPrice.ToString());
        }

        protected void UpdateShopButton(bool interactable, string buttonNameText, string buttonPriceText)
        {
            buttonPriceText = buttonPriceText == "ACTIVATED" ? buttonPriceText : buttonPriceText + "$";
            shopButton.interactable = interactable;
            shopButton.transform.Find("ButtonName").GetComponent<TextMeshProUGUI>().text = buttonNameText;
            shopButton.transform.Find("ButtonPrice").GetComponent<TextMeshProUGUI>().text = buttonPriceText;
        }

        public virtual void BuyObject()
        {
            if (MoneyManagerSingleton.instance.SpendMoney(shopButtonBuyPrice))
            {
                ObjectCount++;
                shopButtonBuyPrice = CalculatePrice();
                SongManager.instance.PlayPurchase();
            }
        }

        public void AddBoughtObject(int amount)
        {
            ObjectCount += amount;
            shopButtonBuyPrice = CalculatePrice();
        }
    }
}