using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PopUps
{
    public class KokTreePopUp : PopUp<KokTreePopUp>
    {
        private TextMeshProUGUI effectInfoText;
        private TextMeshProUGUI priceText;
        private Button buyButton;
        private Type currentType;

        public delegate void OnBuyUpgradeTriggeredDelegate();

        public static event OnBuyUpgradeTriggeredDelegate OnBuyUpgradeTriggered;

        protected override void Awake()
        {
            base.Awake();
            priceText = holdingImageTransform.Find("PriceBackground").Find("Price").GetComponent<TextMeshProUGUI>();
            effectInfoText = holdingImageTransform.Find("DescriptionBackground").Find("EffectInfo")
                .GetComponent<TextMeshProUGUI>();
            buyButton = holdingImageTransform.Find("BuyButton").GetComponent<Button>();
            transform.position = new Vector2(0, 0);
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
                buyButton.interactable = enable;
            }
        }

        public void ShowPopUp(string spriteName, string description, string price, Sprite primalSprite,
            Type incomingType, bool buttonEnable, string effectInfo)
        {
            effectInfoText.text = effectInfo;
            currentType = incomingType;
            nameText.text = spriteName;
            descriptionText.text = description;
            priceText.text = price;
            EnableButton(buttonEnable, incomingType);

            if (animatedImage.overrideSprite != primalSprite)
            {
                animatedImage.overrideSprite = primalSprite;
            }

            SetActive();
        }
    }
}