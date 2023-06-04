using System;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Objects.UnlockableObjectClasses
{
    public abstract partial class UnlockableObject
    {
        [SerializeField] public Button kokButton;
        [SerializeField] public GameObject toUnlockNext;

        [SerializeField] private Sprite availableKokButtonSprite;
        [SerializeField] private Sprite lockedKokButtonSprite;
        [SerializeField] private Sprite boughtKokButtonSprite;
        [SerializeField] private Sprite unknownKokButtonSprite;

        [SerializeField] public TextMeshProUGUI upgradePriceDisplay;

        protected ButtonStatus kokButtonStatus = ButtonStatus.UNKNOWN;
        protected int kokButtonUnlockPrice = 10;

        private void KokTreeButtonStart()
        {
            switch (kokButtonStatus)
            {
                case ButtonStatus.LOCKED:
                    LockButton();
                    break;
                case ButtonStatus.BOUGHT:
                    break;
                case ButtonStatus.AVAILABLE:
                    MakeButtonAvailable();
                    break;
                default:
                    MakeButtonUnknown();
                    break;
            }
        }

        protected abstract void UnlockAnotherButton();

        private void MakeButtonUnknown()
        {
            kokButtonStatus = ButtonStatus.UNKNOWN;
            kokButton.image.sprite = unknownKokButtonSprite;
            kokButton.enabled = false;
            upgradePriceDisplay.text = "??????";
        }

        public void LockButton()
        {
            kokButtonStatus = ButtonStatus.LOCKED;
            kokButton.image.sprite = lockedKokButtonSprite;
            kokButton.enabled = false;
            upgradePriceDisplay.text = kokButtonUnlockPrice + "$";
        }

        private void MakeButtonAvailable()
        {
            kokButtonStatus = ButtonStatus.AVAILABLE;
            kokButton.image.sprite = availableKokButtonSprite;
            upgradePriceDisplay.text = kokButtonUnlockPrice + "$";
        }

        public void BuyUpgrade()
        {
            if (MoneyManager.instance.SpendMoney(kokButtonUnlockPrice))
            {
                kokButtonStatus = ButtonStatus.BOUGHT;
                kokButton.image.sprite = boughtKokButtonSprite;
                kokButton.enabled = false;
                upgradePriceDisplay.text = "";
                UnlockShopButton();
                UnlockAnotherButton();
            }
        }
    }
}