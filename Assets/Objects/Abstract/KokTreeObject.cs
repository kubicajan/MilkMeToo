using System;
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
        private ParticleSystem particleSystem;

        protected ButtonStatus kokButtonStatus = ButtonStatus.UNKNOWN;
        protected string kokButtonDescription = "it is depressed";
        protected int kokButtonUnlockPrice = 10;
        protected string objectName = "";
        protected string effectInfo = "You become depressed";
        protected Vector2 spriteCanvasPosition;
        protected bool clickedInfo;
        protected string availableParticleName = "AvailableParticle";
        protected string boughtParticleName = "BoughtParticle";

        protected virtual void Start()
        {
            Mommy.OnRestart += ResetHandler;
            InitialPopUp.OnSetInactiveTriggered += OnSetInactiveTriggeredHandler;

            primalSpriteButton.gameObject.SetActive(false);
            spriteCanvasPosition =
                Helpers.GetObjectPositionRelativeToCanvas(primalSpriteButton.gameObject.transform.position);
            KokTreePopUp.OnSetInactiveTriggered += OnSetInactiveTriggeredHandler;
            KokTreePopUp.OnBuyUpgradeTriggered += OnBuyUpgradeTriggeredHandler;
            clickedInfo = false;

            upgradePriceDisplay = transform.Find("Price").GetComponent<TextMeshProUGUI>();
            kokButton = transform.GetComponent<Button>();
            KokTreeButtonStart();
        }

        protected virtual void ResetHandler()
        {
            MakeButtonUnknown();
            kokButtonUnlockPrice *= Mommy.magicResetValue;
            primalSpriteButton.gameObject.SetActive(false);
        }

        protected virtual void FixedUpdate()
        {
            bool enoughMoney = MoneyManagerSingleton.instance.IsEnoughFunds(kokButtonUnlockPrice);
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
            SongManager.instance.PlayClick();

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
            Destroy(particleSystem);
        }

        public virtual void LockButton()
        {
            kokButtonStatus = ButtonStatus.LOCKED;
            kokButton.image.sprite = lockedKokButtonSprite;
            kokButton.enabled = true;
            upgradePriceDisplay.text = kokButtonUnlockPrice + "$";
            Destroy(particleSystem);
        }

        protected virtual void MakeButtonAvailable()
        {
            kokButton.enabled = true;
            kokButtonStatus = ButtonStatus.AVAILABLE;
            kokButton.image.sprite = availableKokButtonSprite;
            upgradePriceDisplay.text = kokButtonUnlockPrice + "$";
            SwitchToAvailableParticle();
        }

        public virtual void BuyUpgrade()
        {
            kokButtonStatus = ButtonStatus.BOUGHT;
            kokButton.image.sprite = boughtKokButtonSprite;
            kokButton.enabled = true;
            upgradePriceDisplay.text = "";
            //   UnlockShopButton();
            UnlockAnotherButton();
            SwitchToBoughtParticle();
        }

        protected void SwitchToAvailableParticle()
        {
            SwitchParticleSystem(GameObject.Find(availableParticleName).GetComponent<ParticleSystem>());
        }

        private void SwitchToBoughtParticle()
        {
            SwitchParticleSystem(GameObject.Find(boughtParticleName).GetComponent<ParticleSystem>());
        }

        private void SwitchParticleSystem(ParticleSystem newParticles)
        {
            if (particleSystem != null)
            {
                Destroy(particleSystem);
            }

            particleSystem = Instantiate(newParticles, gameObject.transform, false);
            particleSystem.transform.position = gameObject.transform.position;
            particleSystem.Play();
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
                if (MoneyManagerSingleton.instance.SpendMoney(kokButtonUnlockPrice))
                {
                    BuyUpgrade();
                    SongManager.instance.PlayPurchase();
                }
            }
        }
    }
}