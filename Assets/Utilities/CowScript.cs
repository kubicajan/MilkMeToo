using Managers;
using UnityEngine;

namespace Utilities
{
    public class CowScript : MonoBehaviour
    {
        private void OnMouseDown()
        {
            MoneyManagerSingleton.instance.AddMoney(1);
        }
    }
}