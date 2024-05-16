using System;
using System.Reflection;
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
        private ParticleSystem myParticleSystem;

        protected ButtonStatus kokButtonStatus = ButtonStatus.UNKNOWN;
        protected string kokButtonDescription = "it is depressed";
        protected int kokButtonUnlockPrice = 10;
        protected string objectName = "";
        protected string effectInfo = "You become depressed";
        protected bool clickedInfo;
        protected string availableParticleName = "AvailableParticle";
        protected string boughtParticleName = "BoughtParticle";

        protected virtual void Start()
        {
            Mommy.OnRestart += ResetHandler;
            InitialPopUp.OnSetInactiveTriggered += OnSetInactiveTriggeredHandler;

            primalSpriteButton.gameObject.SetActive(false);
            KokTreePopUp.OnSetInactiveTriggered += OnSetInactiveTriggeredHandler;
            KokTreePopUp.OnBuyUpgradeTriggered += OnBuyUpgradeTriggeredHandler;
            clickedInfo = false;

            upgradePriceDisplay = transform.Find("Price").GetComponent<TextMeshProUGUI>();
            kokButton = transform.GetComponent<Button>();
            Load();
            KokTreeButtonStart();
        }

        private void Load()
        {
            VyjimecnyElan data = SaveManager.instance.GetItemToUpdate(this.GetType().ToString());

            if (data != null)
            {
                kokButtonStatus = data.KokTreeStatus;
            }
        }

        protected virtual void ResetHandler()
        {
            MakeButtonUnknown();
            kokButtonUnlockPrice *= Mommy.magicResetValue;
            primalSpriteButton.gameObject.SetActive(false);
            this.StopAllCoroutines();
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
                    MakeButtonBought();
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
            SaveManager.instance.UpdateKokTreeStatusWrapper(this.GetType().ToString(), ButtonStatus.UNKNOWN);
            kokButtonStatus = ButtonStatus.UNKNOWN;
            kokButton.image.sprite = unknownKokButtonSprite;
            kokButton.enabled = false;
            upgradePriceDisplay.text = "";
            Destroy(myParticleSystem);
        }

        public virtual void LockButton()
        {
            SaveManager.instance.UpdateKokTreeStatusWrapper(this.GetType().ToString(), ButtonStatus.LOCKED);
            kokButtonStatus = ButtonStatus.LOCKED;
            kokButton.image.sprite = lockedKokButtonSprite;
            kokButton.enabled = true;
            upgradePriceDisplay.text = kokButtonUnlockPrice + "$";
            Destroy(myParticleSystem);
        }

        protected virtual void MakeButtonAvailable()
        {
            SaveManager.instance.UpdateKokTreeStatusWrapper(this.GetType().ToString(), ButtonStatus.AVAILABLE);
            kokButton.enabled = true;
            kokButtonStatus = ButtonStatus.AVAILABLE;
            kokButton.image.sprite = availableKokButtonSprite;
            upgradePriceDisplay.text = kokButtonUnlockPrice + "$";
            SwitchToAvailableParticle();
        }

        public virtual void BuyUpgrade()
        {
            MakeButtonBought();
            UnlockAnotherButton();
        }

        private void MakeButtonBought()
        {
            SaveManager.instance.UpdateKokTreeStatusWrapper(this.GetType().ToString(), ButtonStatus.BOUGHT);
            kokButtonStatus = ButtonStatus.BOUGHT;
            kokButton.image.sprite = boughtKokButtonSprite;
            kokButton.enabled = true;
            upgradePriceDisplay.text = "";
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
            if (myParticleSystem != null)
            {
                Destroy(myParticleSystem);
            }

            myParticleSystem = Instantiate(newParticles, gameObject.transform, false);
            myParticleSystem.transform.position = gameObject.transform.position;
            myParticleSystem.Play();
        }

        protected virtual void UnlockAnotherButton()
        {
            toUnlockNext.GetComponent<KokTreeObject>().LockButton();
        }

        private void OnSetInactiveTriggeredHandler()
        {
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