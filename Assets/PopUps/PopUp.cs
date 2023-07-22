using System;
using TMPro;
using UnityEngine;

namespace PopUps
{
    public abstract class PopUp : MonoBehaviour
    {
        public static PopUp instance;
        protected TextMeshProUGUI nameText;
        protected TextMeshProUGUI descriptionText;
        protected TextMeshProUGUI amountMilkedText;

        private void Awake()
        {
            nameText = transform.Find("Name").GetComponent<TextMeshProUGUI>();
            descriptionText = transform.Find("Description").GetComponent<TextMeshProUGUI>();
            amountMilkedText = transform.Find("AmountMilked").GetComponent<TextMeshProUGUI>();
            gameObject.transform.position = new Vector2(0, 0);

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

        public abstract void SetInactive();

        public abstract void ShowPopUp(string name, string description, string amountMilked);
    }
}