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
    private bool zeroFaded = false;
    private bool oneFaded = false;
    private bool twoFaded = false;
    private bool threeFaded = false;
    public static bool NOW = false;

    private Transform cowClickInfoArrow;
    private Transform cowClickInfoText;
    private Transform unlocksClickInfoArrow;
    private Transform unlocksClickInfoText;
    private Transform ShopClickInfoText;
    private Transform ShopClickInfoArrow;

    private Coroutine coroutine1;
    private Coroutine coroutine2;
    private Coroutine coroutine3;

    private void Start()
    {
        cowClickInfoArrow = transform.Find("CowClickInfo/arrow");
        cowClickInfoText = transform.Find("CowClickInfo/text");
        unlocksClickInfoArrow = transform.Find("UnlocksClickInfo/arrow");
        unlocksClickInfoText = transform.Find("UnlocksClickInfo/text");
        ShopClickInfoText = transform.Find("ShopClickInfo/text");
        ShopClickInfoArrow = transform.Find("ShopClickInfo/arrow");

        cowClickInfoArrow.gameObject.SetActive(false);
        cowClickInfoText.gameObject.SetActive(false);
        unlocksClickInfoArrow.gameObject.SetActive(false);
        unlocksClickInfoText.gameObject.SetActive(false);
        ShopClickInfoText.gameObject.SetActive(false);
        ShopClickInfoArrow.gameObject.SetActive(false);
    }

    public void Update()
    {
        if (!zeroFaded)
        {
            zeroFaded = true;
            coroutine1 = MakeItAppear(cowClickInfoArrow.gameObject, cowClickInfoText.gameObject);
        }
        else
        {
            if (MoneyManagerSingleton.instance.GetTotalMoney() != 0)
            {
                if (!oneFaded)
                {
                    oneFaded = true;
                    if (coroutine1 != null)
                    {
                        StopCoroutine(coroutine1);
                    }

                    FadeIt(cowClickInfoArrow.gameObject, cowClickInfoText.gameObject);
                }

                if (!kokTreeSwipedOnce)
                {
                    if (!unlocksClickInfoArrow.gameObject.activeSelf)
                    {
                        coroutine2 = MakeItAppear(unlocksClickInfoArrow.gameObject, unlocksClickInfoText.gameObject);
                    }
                }
                else
                {
                    if (!twoFaded)
                    {
                        twoFaded = true;
                        if (coroutine2 != null)
                        {
                            StopCoroutine(coroutine2);
                        }

                        FadeIt(unlocksClickInfoArrow.gameObject, unlocksClickInfoText.gameObject);
                    }

                    if (!shopSwipedOnce)
                    {
                        if (!ShopClickInfoArrow.gameObject.activeSelf && NOW)
                        {
                            coroutine3 = MakeItAppear(ShopClickInfoArrow.gameObject, ShopClickInfoText.gameObject);
                        }
                    }
                    else
                    {
                        if (!threeFaded)
                        {
                            threeFaded = true;
                            if (coroutine3 != null)
                            {
                                StopCoroutine(coroutine3);
                            }

                            FadeIt(ShopClickInfoArrow.gameObject, ShopClickInfoText.gameObject);
                        }
                    }
                }
            }
        }

        if (MoneyManagerSingleton.instance.GetTotalMoney() != 0 && shopSwipedOnce && kokTreeSwipedOnce)
        {
            enabled = false;
        }
    }

    private Coroutine MakeItAppear(GameObject img, GameObject textMeshPro)
    {
        textMeshPro.SetActive(true);
        // img.SetActive(true);
        SetTransparent(img, textMeshPro);
        img.SetActive(true);
        Image image = img.GetComponent<Image>();
        image.color = new Color(0, 0, 0, 0);
        return StartCoroutine(UnFadeIt(textMeshPro, img));
    }


    private void FadeIt(GameObject img, GameObject textMeshPro)
    {
        StartCoroutine(FadeImage(img, textMeshPro));
    }

    private void SetTransparent(GameObject img, GameObject textMeshPro)
    {
        TextMeshProUGUI text = textMeshPro.GetComponent<TextMeshProUGUI>();
        text.alpha = 0;
    }

    IEnumerator FadeImage(GameObject img, GameObject textMeshPro)
    {
        TextMeshProUGUI text = textMeshPro.GetComponent<TextMeshProUGUI>();
        Image image = img.GetComponent<Image>();
        for (float i = text.alpha; i >= 0; i -= Time.deltaTime)
        {
            image.color = new Color(1, 1, 1, image.color.a - i);
            yield return null;
            text.alpha = i;
        }

        textMeshPro.SetActive(false);
        img.SetActive(false);
    }

    IEnumerator UnFadeIt(GameObject textMeshPro, GameObject img)
    {
        yield return new WaitForSeconds(0.8f);
        float duration = 0.8f;
        TextMeshProUGUI text = textMeshPro.GetComponent<TextMeshProUGUI>();

        for (float i = 0; i <= duration; i += Time.deltaTime)
        {
            yield return null;
            text.alpha = i;
        }

        // yield return new WaitForSeconds(1f);
        Image image = img.GetComponent<Image>();

        for (float i = 0.1f; i <= 1; i += Time.deltaTime)
        {
            image.color = new Color(1, 1, 1, i);
            yield return null;
        }
    }
}