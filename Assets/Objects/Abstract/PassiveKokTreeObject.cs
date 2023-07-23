using System;
using Managers;
using Objects.Abstract.UnlockableObjectClasses;
using PopUps;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Objects.Abstract
{
    public abstract class PassiveKokTreeObject : MonoBehaviour
    {
        [SerializeField] public GameObject toUnlockNext;

        [SerializeField] private Sprite availableKokButtonSprite;
        [SerializeField] private Sprite lockedKokButtonSprite;
        [SerializeField] private Sprite boughtKokButtonSprite;
        [SerializeField] private Sprite unknownKokButtonSprite;

        private Button kokButton;
        private TextMeshProUGUI upgradePriceDisplay;
        private bool clickedKokInfo;

        protected ButtonStatus kokButtonStatus = ButtonStatus.UNKNOWN;
        protected string kokButtonDescription = "it is depressed";
        protected int kokButtonUnlockPrice = 10;
        protected string objectName = "";

        protected virtual void Start()
        {
            KokTreePopUp.OnSetInactiveTriggered += OnSetInactiveTriggeredHandler;
            KokTreePopUp.OnBuyUpgradeTriggered += OnBuyUpgradeTriggeredHandler;

            upgradePriceDisplay = transform.Find("Price").GetComponent<TextMeshProUGUI>();
            kokButton = transform.GetComponent<Button>();
            KokTreeButtonStart();
        }

        protected virtual void Update()
        {
            bool enoughMoney = MoneyManagerSingleton.instance.IsEnoughFunds(kokButtonUnlockPrice);

            if (KokTreePopUp.instance.isActiveAndEnabled && clickedKokInfo)
            {
                this.ClickKokButton();
            }

            UpdatePopUpButton(enoughMoney);
            UpdateKokTree(enoughMoney);
        }

        private void UpdateKokTree(bool enoughMoney)
        {
            if (kokButtonStatus == ButtonStatus.LOCKED && enoughMoney)
            {
                MakeButtonAvailable();
            }

            if (kokButtonStatus == ButtonStatus.AVAILABLE && !enoughMoney)
            {
                LockButton();
            }
        }

        protected void KokTreeButtonStart()
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

        public void ClickKokButton()
        {
            clickedKokInfo = true;
            KokTreePopUp.instance.ShowPopUp(objectName, kokButtonDescription, kokButtonUnlockPrice.ToString(),
                kokButton.image.sprite, GetType());
        }

        private void UpdatePopUpButton(bool enoughMoney)
        {
            KokTreePopUp.instance.EnableButton(enoughMoney, GetType());
        }

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
            kokButton.enabled = true;
            upgradePriceDisplay.text = kokButtonUnlockPrice + "$";
        }

        private void MakeButtonAvailable()
        {
            kokButton.enabled = true;
            kokButtonStatus = ButtonStatus.AVAILABLE;
            kokButton.image.sprite = availableKokButtonSprite;
            upgradePriceDisplay.text = kokButtonUnlockPrice + "$";
        }

        public virtual void BuyUpgrade()
        {
            if (MoneyManagerSingleton.instance.SpendMoney(kokButtonUnlockPrice))
            {
                kokButtonStatus = ButtonStatus.BOUGHT;
                kokButton.image.sprite = boughtKokButtonSprite;
                kokButton.enabled = false;
                upgradePriceDisplay.text = "";
                //   UnlockShopButton();
                UnlockAnotherButton();
            }
        }

        private void UnlockAnotherButton()
        {
            toUnlockNext.GetComponent<PassiveKokTreeObject>().LockButton();
        }

        private void OnSetInactiveTriggeredHandler()
        {
            clickedKokInfo = false;
        }

        private void OnBuyUpgradeTriggeredHandler()
        {
            if (KokTreePopUp.instance.GetCallingType() == GetType())
            {
                BuyUpgrade();
            }
        }
    }
}