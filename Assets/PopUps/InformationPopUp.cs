using TMPro;
using UnityEngine;

namespace PopUps
{
    public class InformationPopUp : PopUp<InformationPopUp>
    {
        private TextMeshProUGUI amountMilkedText;

        protected override void Awake()
        {
            base.Awake();
            amountMilkedText = transform.Find("AmountMilked").GetComponent<TextMeshProUGUI>();
            gameObject.transform.position = new Vector2(0, 0);
        }

        public void ShowPopUp(string spriteName, string description, string amountMilked, Sprite primalSprite)
        {
            SetActive();
            nameText.text = spriteName;
            descriptionText.text = description;
            amountMilkedText.text = "Amount milked: " + amountMilked;

            if (animatedImage.overrideSprite != primalSprite)
            {
                animatedImage.overrideSprite = primalSprite;
            }
        }
    }
}