using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PopUps
{
    public class KokTreePopUp : PopUp<KokTreePopUp>
    {
        private TextMeshProUGUI priceText;
        private Button buyButton;
        private Type currentType;

        public delegate void OnBuyUpgradeTriggeredDelegate();

        public static event OnBuyUpgradeTriggeredDelegate OnBuyUpgradeTriggered;

        protected override void Awake()
        {
            base.Awake();
            priceText = transform.Find("Price").GetComponent<TextMeshProUGUI>();
            buyButton = gameObject.transform.Find("BuyButton").GetComponent<Button>();
            gameObject.transform.position = new Vector2(0, 0);
        }

        public Type GetCallingType()
        {
            return currentType;
        }

        public void BuyUpgrade()
        {
            OnBuyUpgradeTriggered?.Invoke();
            SetInactive();
        }

        public void EnableButton(bool enable, Type incomingType)
        {
            if (incomingType == currentType)
            {
                buyButton.enabled = enable;
            }
        }

        public void ShowPopUp(string spriteName, string description, string price, Sprite primalSprite,
            Type incomingType)
        {
            SetActive();
            currentType = incomingType;
            nameText.text = spriteName;
            descriptionText.text = description;
            priceText.text = price;

            if (animatedImage.overrideSprite != primalSprite)
            {
                animatedImage.overrideSprite = primalSprite;
            }
        }
    }
}