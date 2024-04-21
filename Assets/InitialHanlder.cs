using System.Collections;
using System.Collections.Generic;
using Managers;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

//THIS WHOLE CLASS IS AN ABSOLUTE ABOMINATION AND SHOULD BE NUKED
public class InitialHanlder : MonoBehaviour
{
    public static bool shopSwipedOnce = false;
    public static bool kokTreeSwipedOnce = false;
    private bool oneFaded = false;
    private bool twoFaded = false;
    private bool threeFaded = false;

    private Transform cowClickInfoArrow;
    private Transform cowClickInfoText;
    private Transform unlocksClickInfoArrow;
    private Transform unlocksClickInfoText;
    private Transform ShopClickInfoText;
    private Transform ShopClickInfoArrow;

    private void Start()
    {
        cowClickInfoArrow = transform.Find("CowClickInfo/arrow");
        cowClickInfoText = transform.Find("CowClickInfo/text");
        unlocksClickInfoArrow = transform.Find("UnlocksClickInfo/arrow");
        unlocksClickInfoText = transform.Find("UnlocksClickInfo/text");
        ShopClickInfoText = transform.Find("ShopClickInfo/text");
        ShopClickInfoArrow = transform.Find("ShopClickInfo/arrow");

        cowClickInfoArrow.gameObject.SetActive(true);
        cowClickInfoText.gameObject.SetActive(true);
        unlocksClickInfoArrow.gameObject.SetActive(false);
        unlocksClickInfoText.gameObject.SetActive(false);
        ShopClickInfoText.gameObject.SetActive(false);
        ShopClickInfoArrow.gameObject.SetActive(false);
    }

    public void Update()
    {
        if (MoneyManagerSingleton.instance.GetTotalMoney() != 0)
        {
            if (!oneFaded)
            {
                oneFaded = true;
                FadeIt(cowClickInfoArrow.gameObject, cowClickInfoText.gameObject);
            }

            if (!kokTreeSwipedOnce)
            {
                if (!unlocksClickInfoArrow.gameObject.activeSelf)
                {
                    MakeItAppear(unlocksClickInfoArrow.gameObject, unlocksClickInfoText.gameObject);
                }
            }
            else
            {
                if (!twoFaded)
                {
                    twoFaded = true;
                    FadeIt(unlocksClickInfoArrow.gameObject, unlocksClickInfoText.gameObject);
                }

                if (!shopSwipedOnce)
                {
                    if (!ShopClickInfoArrow.gameObject.activeSelf)
                    {
                        MakeItAppear(ShopClickInfoArrow.gameObject, ShopClickInfoText.gameObject);
                    }
                }
                else
                {
                    if (!threeFaded)
                    {
                        threeFaded = true;
                        FadeIt(ShopClickInfoArrow.gameObject, ShopClickInfoText.gameObject);
                    }
                }
            }
        }

        if (MoneyManagerSingleton.instance.GetTotalMoney() != 0 && shopSwipedOnce && kokTreeSwipedOnce)
        {
            enabled = false;
        }
    }

    private void MakeItAppear(GameObject img, GameObject textMeshPro)
    {
        Debug.Log("SD");
        textMeshPro.SetActive(true);
        img.SetActive(true);
        SetTransparent(img, textMeshPro);
        StartCoroutine(UnFadeIt(img, textMeshPro));
    }

    private void FadeIt(GameObject img, GameObject textMeshPro)
    {
        Debug.Log("SaasdasdasdasD");
        StartCoroutine(FadeImage(img, textMeshPro));
    }

    private void SetTransparent(GameObject img, GameObject textMeshPro)
    {
        Image image = img.GetComponent<Image>();
        image.color = new Color(0, 0, 0, 0);
        TextMeshProUGUI text = textMeshPro.GetComponent<TextMeshProUGUI>();
        text.alpha = 0;
    }

    IEnumerator FadeImage(GameObject img, GameObject textMeshPro)
    {
        float duration = 1;
        TextMeshProUGUI text = textMeshPro.GetComponent<TextMeshProUGUI>();
        Image image = img.GetComponent<Image>();
        for (float i = duration; i >= 0; i -= Time.deltaTime)
        {
            image.color = new Color(1, 1, 1, i);
            yield return null;
            text.alpha = i;
        }
    }

    IEnumerator UnFadeIt(GameObject img, GameObject textMeshPro)
    {
        yield return new WaitForSeconds(1);
        float duration = 1;
        TextMeshProUGUI text = textMeshPro.GetComponent<TextMeshProUGUI>();
        Image image = img.GetComponent<Image>();

        for (float i = 0; i <= duration; i += Time.deltaTime)
        {
            image.color = new Color(1, 1, 1, i);
            yield return null;
            text.alpha = i;
        }
    }
}