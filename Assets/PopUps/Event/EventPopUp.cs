using System;
using System.Collections;
using System.Linq;
using System.Text.RegularExpressions;
using GooglePlayGames;
using Managers;
using Objects;
using Objects.Abstract.ActiveObjectClasses;
using PopUps.Event;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
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

            PlayGamesPlatform.Instance
                .LoadAchievements(achievements =>
                {
                    mamho = achievements
                        .Where(achivement => achivement.id == GPGSIds.achievement_the_leprechaun)
                        .Any(ach => ach.completed);
                });
        }

        public void ShowPopUp()
        {
            audioSource.PlayOneShot(splash);
            ConfigureNewValues();
            SongManager.instance.UpdateAudioMutes(3);
            SetActive();
        }

        public void ConfigureNewValues()
        {
            Random.seed = System.DateTime.Now.Millisecond;
            Scenario scenario = JsonParser.instance.GetRandomScenario();
            int random = Random.Range(0, scenario.Descriptions.Length);
            string randomDescription = scenario.Descriptions.ToArray()[random];
            basedResult = JsonParser.instance.GetResultBasedOnScenario(scenario);

            //set popup fields
            descriptionText.text = randomDescription;
            questionText.text = scenario.Question;

            //set summary/accept fields
            if (basedResult.effectType == EffectStatus.MONEY)
            {
                summaryEffectInfo.text = Helpers.ConvertNumbersToString((dopici(basedResult.effect)));
            }
            else
            {
                summaryEffectInfo.text = basedResult.effect;
            }

            summaryDescription.text = basedResult.text;
        }

        private Decimal dopici(string effect)
        {
            const string PATTERN = @"^([-+]?\d+)";
            Match match = Helpers.ParseRegex(effect, PATTERN);
            double tmpValue = int.Parse(match.Groups[1].Value);
            double multiplier = MoneyManagerSingleton.instance.GetTotalPermanentMultiplication();
            if (multiplier == 0)
            {
                multiplier = 1;
            }

            var gg = (tmpValue * multiplier);
            return (Decimal)(tmpValue * multiplier);
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
            MoneyManagerSingleton.instance.eyo(int.Parse(match.Groups[1].Value));
        }

        private void HandleMoneyCase(string gEffect)
        {
            MoneyManagerSingleton.instance.AddRewardMoney(dopici(gEffect));
        }

        private void HandleHelperCase(string gEffect)
        {
            const string PATTERN = @"^([-+]?\d+)\s(\w+)$";
            Match match = Helpers.ParseRegex(gEffect, PATTERN);
            string objectName = match.Groups[2].Value;
            ActiveKokTreeObject activeKokTreeObject = Helpers.GetActiveKokTreeObject(objectName);
            var gg = int.Parse(match.Groups[1].Value);
            activeKokTreeObject.AddFreeObject(int.Parse(match.Groups[1].Value));
        }

        private int leprikonCounter = 0;
        private bool mamho = false;

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
                leprikonCounter++;

                if (leprikonCounter >= 3 && !mamho)
                {
                    mamho = true;
                    Social.ReportProgress(GPGSIds.achievement_the_leprechaun, 100.0f, (bool success) => { });
                }
            }
            else if (basedResult.effect.StartsWith("-"))
            {
                leprikonCounter = 0;
                wompOrWinSource.PlayOneShot(wompSound);
                unsuccessParticles.gameObject.SetActive(true);
                successParticles.gameObject.SetActive(false);
            }
            else
            {
                leprikonCounter = 0;
                unsuccessParticles.gameObject.SetActive(true);
                successParticles.gameObject.SetActive(true);
            }
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