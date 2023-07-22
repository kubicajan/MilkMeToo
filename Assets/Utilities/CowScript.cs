using Managers;
using UnityEngine;

namespace Utilities
{
    public class CowScript : MonoBehaviour
    {
        public void MilkMe()
        {
            MoneyManagerSingleton.instance.AddMoney(1);
        }
    }
}