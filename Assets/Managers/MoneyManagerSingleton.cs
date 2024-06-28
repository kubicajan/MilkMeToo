using System;
using System.Collections;
using System.Globalization;
using System.Linq;
using System.Numerics;
using GooglePlayGames;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utilities;

namespace Managers
{
    public class MoneyManagerSingleton : MonoBehaviour
    {
        [SerializeField] public TextMeshProUGUI moneyScore;
        [SerializeField] public TextMeshProUGUI multiplier;
        [SerializeField] public Slider slider;

        public static MoneyManagerSingleton instance;

        private double temporaryMultiplication = 0;
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

            if (multiplication != 0)
            {
                multiplier.enabled = true;
            }

            ChangeDisplayStreak();
        }

        Decimal remainingFraction = 0;

        public Decimal AddMoney(Decimal amount)
        {
            double multiplyBy = multiplication + temporaryMultiplication + temporaryMultiplicationPilulky;
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
            multiplicationHasBeenShown = true;
            ChangeDisplayStreak();
        }

        public BigInteger GetTotalMoney()
        {
            return totalMoney;
        }

        public void eyo(int value)
        {
            StartCoroutine(StartBonusProduction());
            StartCoroutine(AddMultiplierCoroutine(value));
        }

        IEnumerator AddMultiplierCoroutine(int value)
        {
            double tmpBonus = value;
            double stepBonus = (tmpBonus / (5)) / 4;
            double tmpTemporaryMultiplication = this.multiplication;
            MoneyManagerSingleton.instance.RaiseTemporaryMultiplication(tmpBonus);

            while (bonusIsOn)
            {
                MoneyManagerSingleton.instance.RaiseTemporaryMultiplication(-stepBonus);
                yield return new WaitForSeconds(0.25f);
            }
            SetRaiseTemporaryMultiplicationToZero();
        }

        private bool bonusIsOn = false;

        private IEnumerator StartBonusProduction()
        {
            bonusIsOn = true;
            slider.gameObject.SetActive(true);
            float timer = 0f;
            while (timer <= 5)
            {
                timer += Time.deltaTime;
                slider.value = timer;
                yield return null;
            }

            bonusIsOn = false;
            slider.gameObject.SetActive(false);
        }

        public void RaiseTemporaryMultiplication(double raiseBy)
        {
            temporaryMultiplication = temporaryMultiplication + raiseBy;
            multiplier.enabled = true;
            ChangeDisplayStreak();
            multiplicationHasBeenShown = true;
        }

        public void SetRaiseTemporaryMultiplicationToZero()
        {
            temporaryMultiplication = 0;
        }

        public void RaiseTemporaryMultiplication2(double raiseBy)
        {
            temporaryMultiplicationPilulky = temporaryMultiplicationPilulky + raiseBy;
            multiplier.enabled = true;
            ChangeDisplayStreak();
            multiplicationHasBeenShown = true;
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

        private void ChangeDisplayedMoney()
        {
            moneyScore.text = $"{Helpers.ConvertNumbersToString(money)}$";
            //      totalScore.text = $"ALL TIME: {totalMoney}$";
        }

        private void ChangeDisplayStreak()
        {
            multiplier.text =
                $"MULTIPLIER: {Helpers.ConvertNumbersToString((decimal)(temporaryMultiplication + temporaryMultiplicationPilulky + multiplication), true)}X";
        }
    }
}