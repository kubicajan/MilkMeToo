using UnityEngine;

namespace PopUps
{
    public class InformationPopUp : PopUp
    {
        public override void SetInactive()
        {
            gameObject.SetActive(false);
        }
        
        public override void ShowPopUp(string spriteName, string description, string amountMilked)
        {
            gameObject.SetActive(true);
            nameText.text = spriteName;
            descriptionText.text = description;
            amountMilkedText.text = "Amount milked: " + amountMilked;
        }
    }
}