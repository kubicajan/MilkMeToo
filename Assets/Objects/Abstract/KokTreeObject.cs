using System;
using System.Numerics;
using Managers;
using Objects.ActiveObjects;
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
        [SerializeField] public GameObject primalSpriteButton;
        protected Decimal originalkokUnlockPrice = 0;

        private Button kokButton;
        private TextMeshProUGUI upgradePriceDisplay;
        private bool clickedKokInfo;
        private ParticleSystem myParticleSystem;
        protected GameObject ConnectingTissueGameObject;

        protected ButtonStatus kokButtonStatus = ButtonStatus.UNKNOWN;
        protected string kokButtonDescription = "it is depressed";
        protected Decimal kokButtonUnlockPrice = 10;
        protected string objectName = "";
        protected string effectInfo = "You become depressed";
        protected bool clickedInfo;
        protected string availableParticleName = "AvailableParticle";
        protected string boughtParticleName = "BoughtParticle";
        protected bool showTheLine = true;

        protected virtual void Start()
        {
            Mommy.OnRestart += ResetHandler;
            InitialPopUp.OnSetInactiveTriggered += OnSetInactiveTriggeredHandler;
            originalkokUnlockPrice = kokButtonUnlockPrice;

            primalSpriteButton.SetActive(false);
            KokTreePopUp.OnSetInactiveTriggered += OnSetInactiveTriggeredHandler;
            KokTreePopUp.OnBuyUpgradeTriggered += OnBuyUpgradeTriggeredHandler;
            clickedInfo = false;

            upgradePriceDisplay = transform.Find("Price").GetComponent<TextMeshProUGUI>();
            kokButton = transform.GetComponent<Button>();

            if (SaveManager.instance.wrapper.timesProud > 0)
            {
                this.kokButtonUnlockPrice = originalkokUnlockPrice *
                                            (Mommy.magicResetValue * SaveManager.instance.wrapper.timesProud);
                kokButtonUnlockPrice = kokButtonUnlockPrice + ((kokButtonUnlockPrice * 20) / 100);
            }

            ConnectingTissueGameObject = GameObject.Find("ConnectingTissue");

            Load();
            ConnectToNextUnlock();
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
            UpdateKokTree(MoneyManagerSingleton.instance.IsEnoughFunds(kokButtonUnlockPrice));
            kokButtonUnlockPrice = originalkokUnlockPrice;
            kokButtonUnlockPrice = originalkokUnlockPrice *
                                   (Mommy.magicResetValue * SaveManager.instance.wrapper.timesProud);
            kokButtonUnlockPrice = kokButtonUnlockPrice + ((kokButtonUnlockPrice * 20) / 100);
            SaveManager.instance.RestartCountBoughtWrapper(this.GetType().ToString());
            primalSpriteButton.SetActive(false);
            this.StopAllCoroutines();
        }

        protected virtual void FixedUpdate()
        {
            if (!Drugs.onMilkingScreen && IsItTime())
            {
                bool enoughMoney = MoneyManagerSingleton.instance.IsEnoughFunds(kokButtonUnlockPrice);
                UpdateKokTree(enoughMoney);
            }
        }

        private float timer = 0f;
        public float interval23 = 0.2f;

        protected bool IsItTime()
        {
            timer += Time.deltaTime;

            if (timer > (interval23))
            {
                timer = 0;
                return true;
            }

            return false;
        }

        public void UpdateKokTree(bool enoughMoney)
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

        protected virtual void KokTreeButtonStart()
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

        protected LineRenderer br = null;

        protected virtual void ConnectToNextUnlock()
        {
            if (showTheLine)
            {
                GameObject copiedObject = Instantiate(ConnectingTissueGameObject, this.gameObject.transform, false);
                copiedObject.transform.position = this.gameObject.transform.position;
                br = copiedObject.GetComponent<LineRenderer>();
                br.positionCount = 2;
                br.SetPosition(0, UnityEngine.Vector3.zero);
                UnityEngine.Vector3 localPositionOfToUnlockNext =
                    this.gameObject.transform.InverseTransformPoint(toUnlockNext.transform.position);

                br.SetPosition(1, localPositionOfToUnlockNext);
            }
        }

        private UnityEngine.Vector2 reee(UnityEngine.Vector3 position)
        {
            UnityEngine.Vector3 screenPosition = RectTransformUtility.WorldToScreenPoint(null, position);
            RectTransform canvasRectTransform = GameObject.Find("KokTreeCanvas").GetComponent<RectTransform>();

            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRectTransform, screenPosition, null,
                out UnityEngine.Vector2 canvasPosition);

            return canvasPosition;
        }

        protected void What(string uhName, string money, bool unlock)
        {
            KokTreePopUp.instance.ShowPopUp(uhName, kokButtonDescription, money, kokButton.image.sprite, GetType(),
                unlock, effectInfo);
        }

        public virtual void ClickKokButton()
        {
            bool unlock = MoneyManagerSingleton.instance.IsEnoughFunds(kokButtonUnlockPrice);
            string money = $"Price \n {Helpers.ConvertNumbersToString((Decimal)kokButtonUnlockPrice)}$";
            string objName = objectName;
            SongManager.instance.PlayClick();

            if (kokButtonStatus == ButtonStatus.BOUGHT)
            {
                money = "BOUGHT";
                unlock = false;
            }

            if (kokButtonStatus == ButtonStatus.LOCKED)
            {
                objName = "???";
            }

            What(objName, money, unlock);
        }

        // private void UpdatePopUpButton(bool enoughMoney)
        // {
        //     KokTreePopUp.instance.EnableButton(enoughMoney, GetType());
        // }

        protected void MakeButtonUnknown()
        {
            SaveManager.instance.UpdateKokTreeStatusWrapper(this.GetType().ToString(), ButtonStatus.UNKNOWN);
            kokButtonStatus = ButtonStatus.UNKNOWN;
            kokButton.image.sprite = unknownKokButtonSprite;
            kokButton.enabled = false;
            upgradePriceDisplay.text = "";
            if (br != null)
            {
                ColorUtility.TryParseHtmlString("#000000", out Color parsedColor);
                br.startColor = parsedColor;
            }

            Destroy(myParticleSystem);
        }

        public virtual void LockButton()
        {
            SaveManager.instance.UpdateKokTreeStatusWrapper(this.GetType().ToString(), ButtonStatus.LOCKED);
            kokButtonStatus = ButtonStatus.LOCKED;
            kokButton.image.sprite = lockedKokButtonSprite;
            kokButton.enabled = true;
            upgradePriceDisplay.text = Helpers.ConvertNumbersToString((Decimal)kokButtonUnlockPrice) + "$";
            if (br != null)
            {
                ColorUtility.TryParseHtmlString("#000000", out Color parsedColor);
                br.startColor = parsedColor;
            }
            Destroy(myParticleSystem);
        }

        protected virtual void MakeButtonAvailable()
        {
            SaveManager.instance.UpdateKokTreeStatusWrapper(this.GetType().ToString(), ButtonStatus.AVAILABLE);
            kokButton.enabled = true;
            kokButtonStatus = ButtonStatus.AVAILABLE;
            kokButton.image.sprite = availableKokButtonSprite;
            upgradePriceDisplay.text = Helpers.ConvertNumbersToString((Decimal)kokButtonUnlockPrice) + "$";
            if (br != null)
            {
                ColorUtility.TryParseHtmlString("#00BBFF", out Color parsedColor);
                br.startColor = parsedColor;
            }

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

            if (br != null)
            {
                ColorUtility.TryParseHtmlString("#FFDA00", out Color parsedColor);
                br.startColor = parsedColor;
            }

            UpdateUpgradePriceDisplayText("");
            SwitchToBoughtParticle();
        }

        protected void UpdateUpgradePriceDisplayText(string value)
        {
            upgradePriceDisplay.text = value;
        }

        protected void UpdateUpgradePriceDisplayText(Decimal value)
        {
            UpdateUpgradePriceDisplayText(Helpers.ConvertNumbersToString(value));
        }

        protected void SwitchToAvailableParticle()
        {
            SwitchParticleSystem(GameObject.Find(availableParticleName).GetComponent<ParticleSystem>());
        }

        public void SwitchToBoughtParticle()
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
            var g = toUnlockNext.GetComponent<KokTreeObject>();
            g.LockButton();
            g.UpdateKokTree(MoneyManagerSingleton.instance.IsEnoughFunds(g.kokButtonUnlockPrice));
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