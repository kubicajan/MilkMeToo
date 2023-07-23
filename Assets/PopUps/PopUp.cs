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
        protected Image animatedImage;
        private CanvasGroup canvasGroup;

        public static T instance;

        protected virtual void Awake()
        {
            nameText = transform.Find("Name").GetComponent<TextMeshProUGUI>();
            descriptionText = transform.Find("Description").GetComponent<TextMeshProUGUI>();
            animatedImage = transform.Find("AnimatedImage").GetComponent<Image>();

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

        public void SetInactive()
        {
            OnSetInactiveTriggered?.Invoke();
            gameObject.SetActive(false);
            canvasGroup.interactable = true;
        }

        public void SetActive()
        {
            OnShowPopUpTriggered?.Invoke();
            gameObject.SetActive(true);
            canvasGroup.interactable = false;
        }
    }
}