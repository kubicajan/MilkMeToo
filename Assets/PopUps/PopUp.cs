using Managers;
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
        protected Transform holdingImageTransform;

        protected Image animatedImage;
        private CanvasGroup canvasGroup;

        public static T instance;

        protected virtual void Awake()
        {
            holdingImageTransform = transform.Find("HoldingImage");
            descriptionText = holdingImageTransform.Find("DescriptionBackground")
                .Find("Description").GetComponent<TextMeshProUGUI>();
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

        public virtual void SetInactiveByClick()
        {
            SongManager.instance.PlayClick();
            SetInactive();
        }

        public virtual void SetInactive()
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