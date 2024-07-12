using System;
using System.Collections;
using System.Globalization;
using System.Linq;
using System.Numerics;
using GooglePlayGames;
using Objects.PassiveObjects;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Utilities;
using ColorUtility = UnityEngine.ColorUtility;
using Random = UnityEngine.Random;
using Vector2 = UnityEngine.Vector2;

namespace Managers
{
    public class MoneyManagerSingleton : MonoBehaviour
    {
        [SerializeField] private AudioSource audioHorn;
        [SerializeField] private AudioClip hornSound;     
        
        [SerializeField] public TextMeshProUGUI moneyScore;
        [SerializeField] public TextMeshProUGUI MULTI;
        [SerializeField] public TextMeshProUGUI multiplier;
        [SerializeField] private ParticleSystem obegaParticl;
        [SerializeField] public Slider slider;
        ParticleSystem fillParticle;
        private Image fillArea;

        public static MoneyManagerSingleton instance;

        private double temporaryMultiplication = 0;
        private double temporaryPermanentMultiplication = 0;
        private double temporaryMultiplicationPilulky = 0;

        private BigInteger totaloMoneys = 0;

        private BigInteger totalMoney
        {
            get => totaloMoneys;
            set
            {
                totaloMoneys = value;

                if (!mamMilion && totaloMoneys >= 1000000)
                {
                    Social.ReportProgress(GPGSIds.achievement_money_can_buy_happiness, 100.0f, (bool success) => { });
                    mamMilion = true;
                }

                if (!mamGazilion && totaloMoneys >= 5000000000000000000)
                {
                    Social.ReportProgress(GPGSIds.achievement_buy_the_earth, 100.0f, (bool success) => { });
                    mamGazilion = true;
                }
            }
        }

        public int numberOfTitties = 0;
        private Decimal money = 0;
        private double multiplication = 0;
        private bool multiplicationHasBeenShown = false;

        private void Awake()
        {
            moneyScore.raycastTarget = false;
            multiplier.raycastTarget = false;
            ChangeDisplayedMoney();
            multiplier.enabled = false;

            if (instance != null && instance != this)
            {
                Destroy(this.gameObject);
            }
            else
            {
                instance = this;
            }
        }

        private bool mamMilion = false;
        private bool mamGazilion = false;


        public void Start()
        {
            slider.gameObject.SetActive(false);
            fillParticle = slider.transform.Find("FillArea/Fill/FillParticle").GetComponent<ParticleSystem>();
            fillArea = slider.transform.Find("FillArea/Fill").GetComponent<Image>();
            PlayGamesPlatform.Instance
                .LoadAchievements(achievements =>
                {
                    mamMilion = achievements
                        .Where(achivement => achivement.id == GPGSIds.achievement_money_can_buy_happiness)
                        .Any(ach => ach.completed);
                });

            PlayGamesPlatform.Instance
                .LoadAchievements(achievements =>
                {
                    mamGazilion = achievements
                        .Where(achivement => achivement.id == GPGSIds.achievement_buy_the_earth)
                        .Any(ach => ach.completed);
                });


            money = SaveManager.instance.GetCurrentMoney();
            totalMoney = SaveManager.instance.GetTotalMoney();
            ChangeDisplayedMoney();
            multiplication = SaveManager.instance.GetMultiplier();
            temporaryPermanentMultiplication = SaveManager.instance.GetTemporaryPermanentMultiplier();

            if (multiplication != 0)
            {
                multiplier.enabled = true;
            }

            ChangeDisplayStreak();
        }

        Decimal remainingFraction = 0;

        public Decimal AddMoney(Decimal amount)
        {
            double multiplyBy = multiplication + temporaryMultiplication + temporaryPermanentMultiplication + temporaryMultiplicationPilulky;
            if (multiplyBy == 0)
            {
                multiplyBy = 1;
            }

            amount *= (int)multiplyBy;
            money += amount;
            Decimal tmpAmount = amount + remainingFraction;
            BigInteger integerPart;
            SeparateDecimal(tmpAmount, out integerPart, out remainingFraction);

            totalMoney += integerPart;
            SaveManager.instance.UpdateTotalMoney(integerPart);
            ChangeDisplayedMoney();
            SaveManager.instance.UpdateCurrentMoney(amount);
            return amount;
        }

        static void SeparateDecimal(decimal decimalNumber, out BigInteger integerPart, out Decimal remainingFraction)
        {
            string numberString = decimalNumber.ToString(CultureInfo.InvariantCulture);
            string[] parts = numberString.Split('.');
            integerPart = BigInteger.Parse(parts[0], CultureInfo.InvariantCulture);
            remainingFraction = parts.Length > 1 ? decimal.Parse("0." + parts[1], CultureInfo.InvariantCulture) : 0m;
        }

        public void AddRewardMoney(Decimal amount)
        {
            money += amount;
            totalMoney += (BigInteger)amount;
            ChangeDisplayedMoney();
            SaveManager.instance.UpdateCurrentMoney(amount);
            SaveManager.instance.UpdateTotalMoney((BigInteger)amount);
        }

        public bool SpendMoney(Decimal amount)
        {
            if (IsEnoughFunds(amount))
            {
                SaveManager.instance.UpdateCurrentMoney(-amount);
                money -= amount;
                ChangeDisplayedMoney();
                return true;
            }

            return false;
        }

        public bool IsEnoughFunds(Decimal price)
        {
            return money >= price;
        }

        public Decimal GetMoney()
        {
            return money;
        }

        public void ResetMoney()
        {
            money = 0;
            ChangeDisplayedMoney();
        }

        public void ResetMultiplicationAndAddToIt(int value)
        {
            multiplication = value;
            SaveManager.instance.UpdateMultiplier(value);
            // SaveManager.instance.UpdateTemporaryPermanentMultiplier(0);
            multiplicationHasBeenShown = true;
            ChangeDisplayStreak();
        }

        public BigInteger GetTotalMoney()
        {
            return totalMoney;
        }

        public void eyo(int value)
        {
            StartCoroutine(StartBonusProduction(value));
        }

        private bool bonusIsOn = false;

        private IEnumerator StartBonusProduction(int value)
        {
            float valueToBeSavedPermanently = (float)(((double)value / 100) * MultiplierBonus.permanentBonus);
            float actualValue = 0;

            if (temporaryPermanentMultiplication + valueToBeSavedPermanently <= 0)
            {
                actualValue = value;
                AddTotemporarilyPermanently(-temporaryPermanentMultiplication);
            }
            else
            {
                AddTotemporarilyPermanently(valueToBeSavedPermanently);
                actualValue = value - valueToBeSavedPermanently;
            }

            bonusIsOn = true;
            slider.gameObject.SetActive(true);
            audioHorn.PlayOneShot(hornSound);

            float finalTime = 10f;
            float timer = 0f;
            string colour = value >= 0 ? "#33cc33" : "#D1003A";
            ColorUtility.TryParseHtmlString(colour, out Color parsedColor);
            fillArea.color = parsedColor;
            fillParticle.startColor = parsedColor;
            // obegaParticl.Play();


            SetTempMultiplication(value, colour);
            while (timer <= finalTime)
            {
                timer += Time.deltaTime;

                float t = Mathf.Pow(timer / finalTime, 3f);

                float currentAcceleration = Mathf.Lerp(0, 10, t);
                slider.value = (finalTime * currentAcceleration) / 10;

                float magicValue = actualValue - (actualValue * currentAcceleration) / 10;
                MoneyManagerSingleton.instance.SetTempMultiplication(magicValue, colour);

                yield return null;
            }

            ClearStreakDisplayColour();
            bonusIsOn = false;
            // obegaParticl.Stop();
            slider.gameObject.SetActive(false);

        }

        private void AddTotemporarilyPermanently(double value)
        {
            temporaryPermanentMultiplication += value;
            SaveManager.instance.UpdateTemporaryPermanentMultiplier(temporaryPermanentMultiplication);

            multiplier.enabled = true;
            ChangeDisplayStreak("red");
            multiplicationHasBeenShown = true;
        }

        public void SetTempMultiplication(double value, string colour)
        {
            temporaryMultiplication = value;
            multiplier.enabled = true;
            MULTI.text =
                $"<color={colour}>{Helpers.ConvertNumbersToString((decimal)(temporaryMultiplication + temporaryMultiplicationPilulky + temporaryPermanentMultiplication + multiplication), true)}X</color>";

            ChangeDisplayStreak(colour);
            multiplicationHasBeenShown = true;
        }

        public void RaiseTemporaryMultiplication2(double raiseBy)
        {
            temporaryMultiplicationPilulky = temporaryMultiplicationPilulky + raiseBy;
            multiplier.enabled = true;
            ChangeDisplayStreak("#FFFF00");
            multiplicationHasBeenShown = true;
        }

        public void ClearStreakDisplayColour()
        {
            ChangeDisplayStreak("white");
        }

        public void SettemporaryMultiplicationPilulkyToZero()
        {
            temporaryMultiplicationPilulky = 0;
        }

        public void RaiseMultiplicationBy(double raiseBy)
        {
            multiplication = multiplication + raiseBy;
            multiplier.enabled = true;
            ChangeDisplayStreak();
            multiplicationHasBeenShown = true;
            SaveManager.instance.UpdateMultiplier(multiplication);
            // RaiseTemporaryMultiplication(raiseBy);
        }

        public double GetTotalPermanentMultiplication()
        {
            return multiplication + temporaryPermanentMultiplication;
        }

        private void ChangeDisplayedMoney()
        {
            moneyScore.text = $"{Helpers.ConvertNumbersToString(money)}$";
            //      totalScore.text = $"ALL TIME: {totalMoney}$";
        }

        private void ChangeDisplayStreak(string color = "white")
        {
            multiplier.text =
                $"MULTIPLIER: <color={color}>{Helpers.ConvertNumbersToString((decimal)(temporaryMultiplication + temporaryMultiplicationPilulky + temporaryPermanentMultiplication + multiplication), true)}X</color>";
        }
    }
}