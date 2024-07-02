using System;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utilities;

namespace Objects.Abstract.ActiveObjectClasses
{
    public abstract partial class ActiveKokTreeObject
    {
        [SerializeField] public Button shopButton;
        [SerializeField] private Sprite questionMarkBasicShop;

        protected int objectCounter = 0;
        protected string shopDefaultName;
        public Decimal originalPrice = 0;

        protected Decimal shopButtonBuyPrice = 0;

        protected int ObjectCount
        {
            get => objectCounter;
            set => ActivateThings(value);
        }

        protected int freeObjects = 0;

        protected int FreeObjects
        {
            get => freeObjects;
            set => ActivateFreeThings(value);
        }

        protected override void ResetHandler()
        {
            base.ResetHandler();
            ShopButtonStart();
            ObjectCount = 0;
            shopButtonBuyPrice = originalPrice;
            // CalculatePrice();
            shopButton.transform.Find("Image").GetComponent<Image>().sprite = questionMarkBasicShop;
            if (SaveManager.instance.GetTimesProud() != 0)
            {
                shopButtonBuyPrice *= Mommy.magicResetValue * SaveManager.instance.GetTimesProud();
            }

            SaveManager.instance.UpdateShopBuyPriceWrapper(this.GetType().ToString(), shopButtonBuyPrice);
        }

        protected virtual void ActivateFreeThings(int value)
        {
            if (value > 0)
            {
                freeObjects = value;
                primalSpriteButton.gameObject.SetActive(true);
                SaveManager.instance.UpdateFreeCountWrapper(this.GetType().ToString(), value);
            }
            else
            {
                freeObjects = 0;
                SaveManager.instance.UpdateFreeCountWrapper(this.GetType().ToString(), freeObjects);
                ObjectCount += value;
            }
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
            SaveManager.instance.SetUpdateCountBoughtWrapper(this.GetType().ToString(), value);
            shopButtonBuyPrice = CalculatePrice();
            SaveManager.instance.UpdateShopBuyPriceWrapper(this.GetType().ToString(), shopButtonBuyPrice);
        }

        protected virtual void UpdateShop(Decimal money)
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

        protected Decimal CalculatePrice()
        {
            var r = ObjectCount;
            var g = originalPrice;
            var gg = Math.Pow(1.15f, ObjectCount);
            var ggg = originalPrice * (Decimal)Math.Pow(1.15f, ObjectCount);
            return (originalPrice * (Decimal)Math.Pow(1.15f, ObjectCount));
        }

        private void ShopButtonStart()
        {
            shopDefaultName = shopButton.transform.Find("ButtonName").GetComponent<TextMeshProUGUI>().text;
            UpdateShopButton(false, "???", "");
        }

        private void UnlockShopButton()
        {
            UpdateShopButton(true, shopDefaultName, Helpers.ConvertNumbersToString(shopButtonBuyPrice));
        }

        private void NoMoneyShopButton()
        {
            UpdateShopButton(false, shopDefaultName, Helpers.ConvertNumbersToString(shopButtonBuyPrice));
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
                SongManager.instance.PlayPurchase();
            }
        }

        public void AddFreeObject(int amount)
        {
            FreeObjects += amount;
        }
    }
}