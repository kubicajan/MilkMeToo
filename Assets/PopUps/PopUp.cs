using TMPro;
using UnityEngine;

namespace PopUps
{
    public abstract class PopUp : MonoBehaviour
    {
        //todo: figure this out dynamically
        public static PopUp instance;
        public delegate void OnSetInactiveTriggeredDelegate();
        public static event OnSetInactiveTriggeredDelegate OnSetInactiveTriggered;
        public delegate void OnShowPopUpTriggeredDelegate();
        public static event OnShowPopUpTriggeredDelegate OnShowPopUpTriggered;

        protected TextMeshProUGUI nameText;
        protected TextMeshProUGUI descriptionText;
        protected TextMeshProUGUI amountMilkedText;
        protected CanvasGroup canvasGroup;


        private void Awake()
        {
            nameText = transform.Find("Name").GetComponent<TextMeshProUGUI>();
            descriptionText = transform.Find("Description").GetComponent<TextMeshProUGUI>();
            amountMilkedText = transform.Find("AmountMilked").GetComponent<TextMeshProUGUI>();
            gameObject.transform.position = new Vector2(0, 0);

            Canvas canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
            canvasGroup = canvas.GetComponent<CanvasGroup>();

            if (instance != null && instance != this)
            {
                Destroy(this.gameObject);
            }
            else
            {
                instance = this;
            }

            gameObject.SetActive(false);
        }

        private void SetInactive()
        {
            OnSetInactiveTriggered?.Invoke();
            gameObject.SetActive(false);
            canvasGroup.interactable = true;
        }

        protected void SetActive()
        {
            OnShowPopUpTriggered?.Invoke();
            gameObject.SetActive(true);
            canvasGroup.interactable = false;
        }

        public abstract void ShowPopUp(string spriteName, string description, string amountMilked);
    }
}