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
        private float totalMoney = 0;
        private float money = 0;
        private int multiplication = 0;

        private void Awake()
        {
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

        public float AddMoney(float amount)
        {
            int multiplyBy = multiplication;
            if (multiplyBy == 0)
            {
                multiplyBy = 1;
            }

            amount *= multiplyBy;

            money += amount;
            totalMoney += amount;
            ChangeDisplayedMoney();
            return amount;
        }

        public bool SpendMoney(float amount)
        {
            if (IsEnoughFunds(amount))
            {
                money -= amount;
                ChangeDisplayedMoney();
                return true;
            }

            return false;
        }

        public bool IsEnoughFunds(float price)
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

        public void RaiseMultiplicationBy(int raiseBy)
        {
            multiplication += raiseBy;
            multiplier.enabled = true;
            ChangeDisplayStreak();
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