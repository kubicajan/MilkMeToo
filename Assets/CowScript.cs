using Managers;
using UnityEngine;

public class CowScript : MonoBehaviour
{
    private int counter = 0;

    private MoneyManager moneyManager;

    private void Start()
    {
        moneyManager = new MoneyManager();
    }
    
    private void OnMouseDown()
    {
        counter = moneyManager.AddMoney(1);
        Debug.Log("MONEY " + counter);
    }
}