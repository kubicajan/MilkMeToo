using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Objects.Abstract.UnlockableObjectClasses
{
    public abstract partial class ActiveKokTreeObject
    {
        [SerializeField] public Button shopButton;

        protected int shopButtonBuyPrice = 10;

        protected int objectCounter = 0;

        private void UpdateShop(float money)
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

        private int ObjectCount
        {
            get => objectCounter;
            set
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
        }

        private void ShopButtonStart()
        {
            UpdateShopButton(false, "???", "");
        }

        private void UnlockShopButton()
        {
            UpdateShopButton(true, objectName, shopButtonBuyPrice.ToString());
        }

        private void NoMoneyShopButton()
        {
            UpdateShopButton(false, objectName, shopButtonBuyPrice.ToString());
        }

        private void UpdateShopButton(bool interactable, string buttonNameText, string buttonPriceText)
        {
            shopButton.interactable = interactable;
            shopButton.transform.Find("ButtonName").GetComponent<TextMeshProUGUI>().text = buttonNameText;
            shopButton.transform.Find("ButtonPrice").GetComponent<TextMeshProUGUI>().text = buttonPriceText;
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