using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Managers;
using Objects;
using Objects.Abstract.ActiveObjectClasses;
using PopUps.Event;
using TMPro;
using UnityEngine;
using Utilities;
using Random = UnityEngine.Random;

namespace PopUps
{
    public class EventPopUp : PopUp<EventPopUp>
    {
        [SerializeField] private GameObject summaryHolder;
        private TextMeshProUGUI summaryDescription;
        private TextMeshProUGUI summaryEffectInfo;
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioClip splash;

        private TextMeshProUGUI questionText;
        private Result basedResult;
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
            audioSource.PlayOneShot(splash);
            ConfigureNewValues();
            SetActive();
            SongManager.instance.UpdateAudioMutes(3);
        }

        public void ConfigureNewValues()
        {
            Scenario scenario = JsonParser.instance.GetRandomScenario();
            int random = Random.Range(0, scenario.Descriptions.Length);
            string randomDescription = scenario.Descriptions.ToArray()[random];
            basedResult = JsonParser.instance.GetResultBasedOnScenario(scenario);

            //set popup fields
            descriptionText.text = randomDescription;
            questionText.text = scenario.Question;

            //set summary/accept fields
            summaryDescription.text = basedResult.text;
            summaryEffectInfo.text = basedResult.effect;
        }

        private void DoThing(Result basedResult)
        {
            switch (basedResult.effectType)
            {
                case EffectStatus.MONEY:
                    HandleMoneyCase(basedResult.effect);
                    break;
                case EffectStatus.HELPER:
                    HandleHelperCase(basedResult.effect);
                    break;
                case EffectStatus.PRODUCTION:
                    HandleProductionCase(basedResult.effect);
                    break;
                default:
                    break;
            }
        }

        private void HandleProductionCase(string gEffect)
        {
            const string PATTERN = @"^([-+]?\d+)";
            Match match = Helpers.ParseRegex(gEffect, PATTERN);
            MoneyManagerSingleton.instance.RaiseMultiplicationBy(int.Parse(match.Groups[1].Value));
        }

        private void HandleMoneyCase(string gEffect)
        {
            const string PATTERN = @"^([-+]?\d+)";
            Match match = Helpers.ParseRegex(gEffect, PATTERN);
            MoneyManagerSingleton.instance.AddRewardMoney(float.Parse(match.Groups[1].Value));
        }

        private void HandleHelperCase(string gEffect)
        {
            const string PATTERN = @"^([-+]?\d+)\s(\w+)$";
            Match match = Helpers.ParseRegex(gEffect, PATTERN);
            string objectName = match.Groups[2].Value;
            ActiveKokTreeObject activeKokTreeObject = Helpers.GetActiveKokTreeObject(objectName);
            activeKokTreeObject.AddBoughtObject(int.Parse(match.Groups[1].Value));
        }

        public void AcceptEvent()
        {
            SongManager.instance.PlayClick();
            gameObject.SetActive(false);
            summaryHolder.SetActive(true);
        //    DoThing(basedResult);
        }

        public override void SetInactive()
        {
            SongManager.instance.PlayClick();
            base.SetInactive();
            summaryHolder.SetActive(false);
        }
        
        public override void SetInactiveByClick()
        {
            DoThing(basedResult);
            SongManager.instance.PlayLastOne();
            base.SetInactiveByClick();
        }
    }
}