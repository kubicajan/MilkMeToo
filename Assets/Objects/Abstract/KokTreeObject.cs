using Managers;
using PopUps;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utilities;

namespace Objects.Abstract
{
    public abstract class KokTreeObject : MonoBehaviour
    {
        [SerializeField] public GameObject toUnlockNext;

        [SerializeField] private Sprite availableKokButtonSprite;
        [SerializeField] private Sprite lockedKokButtonSprite;
        [SerializeField] private Sprite boughtKokButtonSprite;
        [SerializeField] private Sprite unknownKokButtonSprite;
        [SerializeField] public Button primalSpriteButton;


        private Button kokButton;
        private TextMeshProUGUI upgradePriceDisplay;
        private bool clickedKokInfo;

        protected ButtonStatus kokButtonStatus = ButtonStatus.UNKNOWN;
        protected string kokButtonDescription = "it is depressed";
        protected int kokButtonUnlockPrice = 10;
        protected string objectName = "";
        protected string effectInfo = "You become depressed";
        protected Vector2 spriteCanvasPosition;
        protected bool clickedInfo;

        protected virtual void Start()
        {
            primalSpriteButton.gameObject.SetActive(false);
            spriteCanvasPosition = Helpers.GetObjectPositionRelativeToCanvas(primalSpriteButton.gameObject);
            KokTreePopUp.OnSetInactiveTriggered += OnSetInactiveTriggeredHandler;
            KokTreePopUp.OnBuyUpgradeTriggered += OnBuyUpgradeTriggeredHandler;
            clickedInfo = false;

            upgradePriceDisplay = transform.Find("Price").GetComponent<TextMeshProUGUI>();
            kokButton = transform.GetComponent<Button>();
            KokTreeButtonStart();
        }

        protected virtual void FixedUpdate()
        {
            bool enoughMoney = MoneyManagerSingleton.instance.IsEnoughFunds(kokButtonUnlockPrice);

            if (KokTreePopUp.instance.isActiveAndEnabled && clickedKokInfo)
            {
                this.ClickKokButton();
            }
            

            // UpdatePopUpButton(enoughMoney);
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
        
        public virtual void Clicked()
        {
            clickedInfo = true;
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
            bool unlock = MoneyManagerSingleton.instance.IsEnoughFunds(kokButtonUnlockPrice);
            string money = $"Price \n {kokButtonUnlockPrice.ToString()}$";
            string name = objectName;
            clickedKokInfo = true;

            if (kokButtonStatus == ButtonStatus.BOUGHT)
            {
                money = "BOUGHT";
                unlock = false;
            }

            if (kokButtonStatus == ButtonStatus.LOCKED)
            {
                name = "???";
            }

            KokTreePopUp.instance.ShowPopUp(name, kokButtonDescription, money, kokButton.image.sprite, GetType(),
                unlock, effectInfo);
        }

        // private void UpdatePopUpButton(bool enoughMoney)
        // {
        //     KokTreePopUp.instance.EnableButton(enoughMoney, GetType());
        // }

        private void MakeButtonUnknown()
        {
            kokButtonStatus = ButtonStatus.UNKNOWN;
            kokButton.image.sprite = unknownKokButtonSprite;
            kokButton.enabled = false;
            upgradePriceDisplay.text = "";
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
                kokButton.enabled = true;
                upgradePriceDisplay.text = "";
                //   UnlockShopButton();
                UnlockAnotherButton();
            }
        }

        protected virtual void UnlockAnotherButton()
        {
            toUnlockNext.GetComponent<KokTreeObject>().LockButton();
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