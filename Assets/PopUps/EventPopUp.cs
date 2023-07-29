using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace PopUps
{
    public class EventPopUp : PopUp<EventPopUp>
    {
        protected override void Awake()
        {
            base.Awake();
            gameObject.transform.position = new Vector2(0, 0);
        }

        public void ShowPopUp()
        {
            // effectInfoText.text = effectInfo;
            // currentType = incomingType;
            // nameText.text = spriteName;
            // descriptionText.text = description;
            // priceText.text = price;
            // EnableButton(buttonEnable, incomingType);

            // if (animatedImage.overrideSprite != primalSprite)
            // {
            //     animatedImage.overrideSprite = primalSprite;
            // }

            SetActive();
        }
    }
}