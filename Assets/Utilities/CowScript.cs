using Managers;
using UnityEngine;

namespace Utilities
{
    public class CowScript : MonoBehaviour
    {
        private void OnMouseDown()
        {
            MoneyManager.instance.ModifyMoneyValue(1);
        }
    }
}