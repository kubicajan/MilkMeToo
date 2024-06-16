using System;
using System.Globalization;
using System.Linq;
using System.Numerics;
using GooglePlayGames;
using TMPro;
using UnityEngine;
using Utilities;

namespace Managers
{
    public class MoneyManagerSingleton : MonoBehaviour
    {
        [SerializeField] public TextMeshProUGUI moneyScore;

        //[SerializeField] public TextMeshProUGUI totalScore;
        [SerializeField] public TextMeshProUGUI multiplier;

        public static MoneyManagerSingleton instance;

        private BigInteger totaloMoneys = 0;

        private bool gg = false;
        private bool gg3 = false;

        private BigInteger totalMoney
        {
            get => totaloMoneys;
            set
            {
                Debug.Log(totaloMoneys.ToString());
                totaloMoneys = value;

                if (!gg && totaloMoneys >= 1000000)
                {
                    gg = true;
                    Social.ReportProgress(GPGSIds.achievement_money_can_buy_happiness, 100.0f, (bool success) => { });
                }

                if (!gg3 && totaloMoneys >= 5000000000000000000)
                {
                    gg3 = true;
                    Social.ReportProgress(GPGSIds.achievement_buy_the_earth, 100.0f, (bool success) => { });
                }
            }
        }

        public int numberOfTitties = 0;
        private Decimal money = 0;
        private int multiplication = 0;
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

        public void Start()
        {
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
            int multiplyBy = multiplication;
            if (multiplyBy == 0)
            {
                multiplyBy = 1;
            }

            amount *= multiplyBy;
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
            //todo: fix
            totalMoney += (int)amount;
            ChangeDisplayedMoney();
            SaveManager.instance.UpdateCurrentMoney(amount);
            SaveManager.instance.UpdateTotalMoney((int)amount);
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

        public void RaiseMultiplicationBy(int raiseBy)
        {
            int tmpMultiplication = multiplication + raiseBy;

            multiplication = tmpMultiplication > 0
                ? tmpMultiplication
                : 0;
            multiplier.enabled = true;
            ChangeDisplayStreak();
            multiplicationHasBeenShown = true;
            SaveManager.instance.UpdateMultiplier(multiplication);
        }

        private void ChangeDisplayedMoney()
        {
            moneyScore.text = $"{Helpers.ConvertNumbersToString(money)}$";
            //      totalScore.text = $"ALL TIME: {totalMoney}$";
        }

        private void ChangeDisplayStreak()
        {
            multiplier.text = $"MULTIPLIER: {multiplication.ToString()}X";
        }
    }
}