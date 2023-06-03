using System;
using TMPro;
using UnityEngine;

namespace Managers
{
    public class MoneyManager : MonoBehaviour
    {
        [SerializeField] public TextMeshProUGUI moneyScore;
        public static MoneyManager instance;
        private int money;

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

        public void AddMoney(int amount)
        {
            money += amount;
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

        public int GetMoney()
        {
            return money;
        }

        private void ChangeDisplayedMoney()
        {
            moneyScore.text = money.ToString();
        }
    }
}