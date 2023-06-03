using Managers;
using UnityEngine;

namespace Utilities
{
    public class CowScript : MonoBehaviour
    {
        private void OnMouseDown()
        {
            MoneyManager.instance.AddMoney(1);
        }
    }
}