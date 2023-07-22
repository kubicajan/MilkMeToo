namespace PopUps
{
    public class InformationPopUp : PopUp
    {
        public override void ShowPopUp(string spriteName, string description, string amountMilked)
        {
            SetActive();
            nameText.text = spriteName;
            descriptionText.text = description;
            amountMilkedText.text = "Amount milked: " + amountMilked;
        }
    }
}