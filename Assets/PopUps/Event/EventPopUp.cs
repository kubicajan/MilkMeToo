using System.Linq;
using System.Text.RegularExpressions;
using Managers;
using Objects;
using Objects.Abstract.ActiveObjectClasses;
using PopUps.Event;
using TMPro;
using Unity.VisualScripting;
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
        [SerializeField] private AudioSource wompOrWinSource;
        [SerializeField] private AudioClip splash;
        [SerializeField] private AudioClip wompSound;
        [SerializeField] private AudioClip winSound;
        [SerializeField] private ParticleSystem successParticles;
        [SerializeField] private ParticleSystem unsuccessParticles;


        private TextMeshProUGUI questionText;
        private Result basedResult;
        private string result;
        private string effect;

        protected override void Start()
        {
            base.Start();
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
            //todo: fix
            audioSource.PlayOneShot(splash);
            ConfigureNewValues();
            SongManager.instance.UpdateAudioMutes(3);
            SetActive();
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
            gameObject.SetActive(false);
            summaryHolder.SetActive(true);
            SongManager.instance.PlayClick();

            if (basedResult.effect.StartsWith("+"))
            {
                wompOrWinSource.PlayOneShot(winSound);
                successParticles.gameObject.SetActive(true);
                unsuccessParticles.gameObject.SetActive(false);
            }
            else if (basedResult.effect.StartsWith("-"))
            {
                wompOrWinSource.PlayOneShot(wompSound);
                unsuccessParticles.gameObject.SetActive(true);
                successParticles.gameObject.SetActive(false);
            }
            else
            {
                unsuccessParticles.gameObject.SetActive(true);
                successParticles.gameObject.SetActive(true);
            }

            SongManager.instance.PlayLastOne();
        }

        public override void SetInactive()
        {
            SongManager.instance.PlayClick();
            base.SetInactive();
            summaryHolder.SetActive(false);
        }

        public override void SetInactiveByClick()
        {
            summaryHolder.SetActive(false);
            SongManager.instance.PlayLastOne();
            base.SetInactiveByClick();
        }

        public void DismissEventSummary()
        {
            DoThing(basedResult);
            summaryHolder.SetActive(false);
            SongManager.instance.PlayLastOne();
            base.SetInactiveByClick();
        }
    }
}