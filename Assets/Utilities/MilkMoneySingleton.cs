using System;
using System.Collections;
using TMPro;
using UnityEngine;

namespace Utilities
{
    public class MilkMoneySingleton : MonoBehaviour
    {
        public static MilkMoneySingleton instance;
        private GameObject tmpTextHolder;

        private void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(this.gameObject);
            }
            else
            {
                instance = this;
            }
        }

        private void Start()
        {
            tmpTextHolder = GameObject.Find("tmpTextHolder");
        }

        public void HandleMilkMoneyShow(string points, Vector2 spriteCanvasPosition)
        {
            ShowMilkedMoney(points, TransformVectorByABitUp(spriteCanvasPosition));
        }

        private Vector2 TransformVectorByABitUp(Vector2 position)
        {
            return position - new Vector2(100f, -120f);
        }

        private void ShowMilkedMoney(string points, Vector2 spriteCanvasPosition)
        {
            GameObject textObjectWrapper = Instantiate(tmpTextHolder, spriteCanvasPosition, Quaternion.identity);
            TextMeshProUGUI textMeshPro = textObjectWrapper.GetComponentInChildren<TextMeshProUGUI>();
            textObjectWrapper.transform.SetParent(GameObject.Find("EvenGreaterHolder").transform, false);
            textMeshPro.fontSize = 50;
            textMeshPro.text = $"+ {points}";
            textMeshPro.CrossFadeAlpha(0.0f, 0.8f, true);
            textMeshPro.raycastTarget = false;
            Destroy(textObjectWrapper, 0.8f);
        }

        //this works but is not necessary

        // private IEnumerator MoveTextSlowlyUpCoroutine(TextMeshProUGUI textMeshPro)
        // {
        //     float moveDuration = 1.0f;
        //     float moveDistance = 50.0f;
        //
        //     Vector2 initialPosition = textMeshPro.rectTransform.anchoredPosition;
        //
        //     float elapsedTime = 0f;
        //     while (elapsedTime < moveDuration)
        //     {
        //         float progress = elapsedTime / moveDuration;
        //         Vector2 targetPosition = initialPosition + Vector2.up * moveDistance * progress;
        //         textMeshPro.rectTransform.anchoredPosition = targetPosition;
        //
        //         elapsedTime += Time.deltaTime;
        //         yield return null;
        //     }
        // }
    }
}