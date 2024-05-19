using System;
using System.Numerics;
using TMPro;
using UnityEngine;

namespace Managers
{
    public class MoneyManagerSingleton : MonoBehaviour
    {
        [SerializeField] public TextMeshProUGUI moneyScore;

        //[SerializeField] public TextMeshProUGUI totalScore;
        [SerializeField] public TextMeshProUGUI multiplier;

        public static MoneyManagerSingleton instance;
        private BigInteger totalMoney = 0;
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

        public Decimal AddMoney(Decimal amount)
        {
            int multiplyBy = multiplication;
            if (multiplyBy == 0)
            {
                multiplyBy = 1;
            }

            amount *= multiplyBy;

            money += amount;
            //todo: fix
            totalMoney += (int)amount;
            //todo:
            // Social.ReportScore((long)totalMoney, "CgkIrdTOtaYPEAIQBA", (bool success) => { });
            ChangeDisplayedMoney();
            SaveManager.instance.UpdateCurrentMoney(amount);
            //todo:
            SaveManager.instance.UpdateTotalMoney((int)amount);

            return amount;
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
            moneyScore.text = $"{money}$";
            //      totalScore.text = $"ALL TIME: {totalMoney}$";
        }

        private void ChangeDisplayStreak()
        {
            multiplier.text = $"MULTIPLIER: {multiplication.ToString()}X";
        }
    }
}