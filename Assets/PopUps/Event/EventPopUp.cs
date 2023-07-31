using TMPro;
using UnityEngine;

namespace PopUps
{
    public class EventPopUp : PopUp<EventPopUp>
    {
        [SerializeField] private GameObject summaryHolder;
        private TextMeshProUGUI summaryDescription;
        private TextMeshProUGUI summaryEffectInfo;

        private TextMeshProUGUI questionText;
        private string description;
        private string question;

        protected override void Awake()
        {
            base.Awake();
            description = "";
            question = "";
            gameObject.transform.position = new Vector2(0, 0);
            summaryHolder.transform.position = new Vector2(0, 0);
            summaryDescription = GameObject.Find("SummaryDescription").GetComponent<TextMeshProUGUI>();
            summaryEffectInfo = GameObject.Find("SummaryEffect").GetComponent<TextMeshProUGUI>();
            summaryHolder.SetActive(false);
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

        public void AcceptEvent()
        {
            gameObject.SetActive(false);
            summaryDescription.text = "deprese";
            summaryEffectInfo.text = "jeste vetsi deprese";
            summaryHolder.SetActive(true);
        }

        public override void SetInactive()
        {
            base.SetInactive();
            summaryHolder.SetActive(false);
        }
    }
}