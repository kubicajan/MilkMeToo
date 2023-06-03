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


        public void ModifyMoneyValue(int amount)
        {
            if (IsCanBuyFor(amount))
            {
                money += amount;
                ChangeDisplayedMoney();
            }
        }

        public bool IsCanBuyFor(int amount)
        {
            return (money + amount) >= 0;
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