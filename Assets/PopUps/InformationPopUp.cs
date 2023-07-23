using UnityEngine;

namespace PopUps
{
    public class InformationPopUp : PopUp<InformationPopUp>
    {
        public override void ShowPopUp(string spriteName, string description, string amountMilked, Sprite primalSprite)
        {
            SetActive();
            nameText.text = spriteName;
            descriptionText.text = description;
            amountMilkedText.text = "Amount milked: " + amountMilked;
            if (animatedImage.overrideSprite != primalSprite)
            {
                animatedImage.overrideSprite = primalSprite;
                Debug.Log("changes");
            }
        }
    }
}