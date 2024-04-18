using Managers;
using UnityEngine;

public class InitialHanlder : MonoBehaviour
{
    void Start()
    {
    }

    void Update()
    {
        if (MoneyManagerSingleton.instance.GetTotalMoney() != 0)
        {
            Transform childObjectTransform1 = transform.Find("CowClickInfo/arrow");
            Transform childObjectTransform2 = transform.Find("CowClickInfo/text");
            Transform childObjectTransform3 = transform.Find("UnlocksClickInfo/arrow");
            Transform childObjectTransform4 = transform.Find("UnlocksClickInfo/text");
            Transform childObjectTransform5 = transform.Find("ShopClickInfo/text");
            Transform childObjectTransform6 = transform.Find("ShopClickInfo/arrow");
            childObjectTransform1.gameObject.SetActive(false);
            childObjectTransform2.gameObject.SetActive(false);
            childObjectTransform3.gameObject.SetActive(false);
            childObjectTransform4.gameObject.SetActive(false);
            childObjectTransform5.gameObject.SetActive(false);
            childObjectTransform6.gameObject.SetActive(false);
            enabled = false;
        }
    }
}