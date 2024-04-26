using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PopUps
{
    public class InformationPopUp : PopUp<InformationPopUp>
    {
        private TextMeshProUGUI amountMilkedText;
        private TextMeshProUGUI counter;

        protected override void Start()
        {
            base.Start();
            nameText = holdingImageTransform.Find("NameBackground").Find("Name").GetComponent<TextMeshProUGUI>();
            animatedImage = holdingImageTransform.Find("NameBackground").Find("AnimatedImage").GetComponent<Image>();
            counter = transform.Find("PlateBackground").Find("Plate").Find("Counter").GetComponent<TextMeshProUGUI>();
            amountMilkedText = holdingImageTransform.Find("AmountMilkedBackground").Find("AmountMilked")
                .GetComponent<TextMeshProUGUI>();
            gameObject.transform.position = new Vector2(0, 0);
        }

        public void ShowPopUp(string spriteName, string description, string amountMilked, Sprite primalSprite,
            string objectCounter)
        {
            counter.text = objectCounter;
            nameText.text = spriteName;
            descriptionText.text = description;
            amountMilkedText.text = amountMilked;

            if (animatedImage.overrideSprite != primalSprite)
            {
                animatedImage.overrideSprite = primalSprite;
            }

            SetActive();
        }
    }
}