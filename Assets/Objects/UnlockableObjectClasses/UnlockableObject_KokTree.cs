using System;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Objects.UnlockableObjectClasses
{
    public partial class UnlockableObject
    {
        [SerializeField] public Button kokButton;

        [SerializeField] private Sprite availableKokButtonSprite;
        [SerializeField] private Sprite lockedKokButtonSprite;
        [SerializeField] private Sprite unlockedKokButtonSprite;

        protected ButtonStatus kokButtonStatus = ButtonStatus.LOCKED;
        protected int kokButtonUnlockPrice = 10;

        private void KokTreeButtonStart()
        {
            switch (kokButtonStatus)
            {
                case ButtonStatus.BOUGHT:
                    //TODO: is this even necessary
                    break;
                case ButtonStatus.AVAILABLE:
                    MakeButtonAvailable();
                    break;
                default:
                    LockButton();
                    break;
            }
        }

        private void LockButton()
        {
            kokButtonStatus = ButtonStatus.LOCKED;
            kokButton.image.sprite = lockedKokButtonSprite;
            kokButton.enabled = false;
        }

        public void MakeButtonAvailable()
        {
            kokButtonStatus = ButtonStatus.AVAILABLE;
            kokButton.image.sprite = availableKokButtonSprite;
        }

        public void BuyUpgrade()
        {
            if (MoneyManager.instance.SpendMoney(kokButtonUnlockPrice))
            {
                kokButtonStatus = ButtonStatus.BOUGHT;
                kokButton.image.sprite = unlockedKokButtonSprite;
                UnlockShopButton();
            }
        }
    }
}