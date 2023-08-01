using System.Linq;
using PopUps.Event;
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
        private string result;
        private string effect;

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
            ConfigureNewValues();
            SetActive();
        }

        public void ConfigureNewValues()
        {
            Scenario scenario = JsonParser.instance.GetRandomScenario();
            int random = Random.Range(0, scenario.Descriptions.Length);
            string randomDescription = scenario.Descriptions.ToArray()[random];
            Result basedResult = JsonParser.instance.GetResultBasedOnScenario(scenario);

            //set popup fields
            descriptionText.text = randomDescription;
            questionText.text = scenario.Question;

            //set summary/accept fields
            summaryDescription.text = basedResult.text;
            summaryEffectInfo.text = basedResult.effect;
        }

        public void AcceptEvent()
        {
            gameObject.SetActive(false);
            summaryHolder.SetActive(true);
        }

        public override void SetInactive()
        {
            base.SetInactive();
            summaryHolder.SetActive(false);
        }
    }
}