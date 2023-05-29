using System;
using Unity.VisualScripting;

namespace Managers
{
    public sealed class MoneyManager
    {
        private static readonly Lazy<MoneyManager> Lazy = new(() => new MoneyManager());

        public static MoneyManager Instance => Lazy.Value;

        private int money = 0;


        public int AddMoney(int amountToAdd)
        {
            money += amountToAdd;
            return money;
        }
    }
}