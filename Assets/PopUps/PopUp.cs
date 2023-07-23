using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PopUps
{
    public abstract class PopUp<T> : MonoBehaviour where T : PopUp<T>
    {
        public delegate void OnSetInactiveTriggeredDelegate();

        public static event OnSetInactiveTriggeredDelegate OnSetInactiveTriggered;

        public delegate void OnShowPopUpTriggeredDelegate();

        public static event OnShowPopUpTriggeredDelegate OnShowPopUpTriggered;

        protected TextMeshProUGUI nameText;
        protected TextMeshProUGUI descriptionText;
        protected TextMeshProUGUI amountMilkedText;
        protected Image animatedImage;
        private CanvasGroup canvasGroup;

        public static T instance;

        protected void Awake()
        {
            nameText = transform.Find("Name").GetComponent<TextMeshProUGUI>();
            descriptionText = transform.Find("Description").GetComponent<TextMeshProUGUI>();
            amountMilkedText = transform.Find("AmountMilked").GetComponent<TextMeshProUGUI>();
            animatedImage = transform.Find("AnimatedImage").GetComponent<Image>();
            gameObject.transform.position = new Vector2(0, 0);

            Canvas canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
            canvasGroup = canvas.GetComponent<CanvasGroup>();
            CreateSingleton();
            gameObject.SetActive(false);
        }

        private void CreateSingleton()
        {
            if (instance != null && instance != this)
            {
                Destroy(this.gameObject);
            }
            else
            {
                instance = this as T;
            }
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

        public abstract void ShowPopUp(string spriteName, string description, string amountMilked, Sprite primalSprite);
    }
}