using System;
using System.Linq;
using System.Text.RegularExpressions;
using Managers;
using Objects;
using Objects.Abstract;
using Objects.Abstract.ActiveObjectClasses;
using Objects.ActiveObjects;
using PopUps.Event;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

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
            DoThing(basedResult);
        }

        private void DoThing(Result basedResult)
        {
            switch (basedResult.effectType)
            {
                case EffectStatus.MONEY:
                    HandleMoneyCase(basedResult);
                    break;
                case EffectStatus.HELPER:
                    HandleHelperCase(basedResult);
                    break;
                case EffectStatus.PRODUCTION:
                    HandleProductionCase(basedResult);
                    break;
                default:
                    break;
            }
        }

        private void HandleProductionCase(Result basedResult)
        {
            const string PATTERN = @"^([-+]?\d+)";
            Match match = ParseRegex(basedResult.effect, PATTERN);
            MoneyManagerSingleton.instance.RaiseMultiplicationBy(int.Parse(match.Groups[1].Value));
        }

        private void HandleMoneyCase(Result basedResult)
        {
            const string PATTERN = @"^([-+]?\d+)";
            Match match = ParseRegex(basedResult.effect, PATTERN);
            MoneyManagerSingleton.instance.AddRewardMoney(float.Parse(match.Groups[1].Value));
        }

        private void HandleHelperCase(Result basedResult)
        {
            const string PATTERN = @"^([-+]?\d+)\s(\w+)$";
            Match match = ParseRegex(basedResult.effect, PATTERN);
            string objectName = match.Groups[2].Value;
            string objectNameWithCapitalLetter = char.ToUpper(objectName[0]) + objectName.Substring(1);
            Type type = Type.GetType("Objects.ActiveObjects." + objectNameWithCapitalLetter);
            ActiveKokTreeObject activeKokTreeObject = (ActiveKokTreeObject)GameObject.Find(objectNameWithCapitalLetter).GetComponent(type);
            activeKokTreeObject.AddBoughtObject(int.Parse(match.Groups[1].Value));
        }

        private Match ParseRegex(string input, string pattern)
        {
            Regex regex = new Regex(pattern);
            return regex.Match(input);
        }

        public void AcceptEvent()
        {
            SongManager.instance.PlayClick();
            gameObject.SetActive(false);
            summaryHolder.SetActive(true);
        }

        public override void SetInactive()
        {
            SongManager.instance.PlayClick();
            base.SetInactive();
            summaryHolder.SetActive(false);
        }
    }
}