using System;
using TMPro;
using UnityEngine;

namespace Managers
{
    public class MoneyManager : MonoBehaviour
    {
        [SerializeField] public TextMeshProUGUI moneyScore;
        [SerializeField] public TextMeshProUGUI totalScore;
        public static MoneyManager instance;
        private float totalMoney = 0;
        private float money;

        private void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(this.gameObject);
            }
            else
            {
                instance = this;
            }
        }

        public void AddMoney(float amount)
        {
            money += amount;
            totalMoney += amount;
            ChangeDisplayedMoney();
        }

        public bool SpendMoney(int amount)
        {
            if (IsEnoughFunds(amount))
            {
                money -= amount;
                ChangeDisplayedMoney();
                return true;
            }

            return false;
        }

        public bool IsEnoughFunds(int price)
        {
            return money >= price;
        }

        public float GetMoney()
        {
            return money;
        }

        public float GetTotalMoney()
        {
            return totalMoney;
        }

        private void ChangeDisplayedMoney()
        {
            moneyScore.text = "money = " + money;
            totalScore.text = "score = " + totalMoney;
        }
    }
}