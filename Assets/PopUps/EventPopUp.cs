using TMPro;
using UnityEngine;

namespace PopUps
{
    public class EventPopUp : PopUp<EventPopUp>
    {
        private TextMeshProUGUI questionText;
        private string description;
        private string question;

        protected override void Awake()
        {
            base.Awake();
            description = "";
            question = "";
            gameObject.transform.position = new Vector2(0, 0);
            questionText = holdingImageTransform
                .Find("QuestionBackground")
                .Find("Question").GetComponent<TextMeshProUGUI>();
        }

        public void ShowPopUp()
        {
            questionText.text = question;
            descriptionText.text = description;
            SetActive();
        }

        public void ConfigureFields(string newDescription, string newQuestion)
        {
            description = newDescription;
            question = newQuestion;
        }
    }
}