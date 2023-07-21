using System.Collections;
using TMPro;
using UnityEngine;

namespace Utilities
{
    public class MilkMoneySingleton : MonoBehaviour
    {
        public static MilkMoneySingleton instance;

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

        public void HandleMilkMoneyShow(float points, Vector2 spriteCanvasPosition)
        {
            ShowMilkedMoney(points, spriteCanvasPosition, out TextMeshProUGUI textMeshPro, out GameObject textObject);
            StartCoroutine(MoveTextSlowlyUpCoroutine(textMeshPro));
            textMeshPro.CrossFadeAlpha(0.0f, 1.25f, false);
            Destroy(textObject, 1.25f);
        }

        private void ShowMilkedMoney(float points, Vector2 spriteCanvasPosition, out TextMeshProUGUI textMeshPro,
            out GameObject textObject)
        {
            textObject = new GameObject("MyText");
            textMeshPro = textObject.AddComponent<TextMeshProUGUI>();

            textObject.transform.SetParent(GameObject.Find("CowPanel").transform, false);
            RectTransform textRectTransform = textObject.GetComponent<RectTransform>();
            textRectTransform.anchoredPosition = new Vector2(spriteCanvasPosition.x + 300, spriteCanvasPosition.y);

            textMeshPro.fontSize = 50;
            textMeshPro.text = "+" + points;
        }

        private IEnumerator MoveTextSlowlyUpCoroutine(TextMeshProUGUI textMeshPro)
        {
            float moveDuration = 1.0f;
            float moveDistance = 50.0f;

            Vector2 initialPosition = textMeshPro.rectTransform.anchoredPosition;

            float elapsedTime = 0f;
            while (elapsedTime < moveDuration)
            {
                float progress = elapsedTime / moveDuration;
                Vector2 targetPosition = initialPosition + Vector2.up * moveDistance * progress;
                textMeshPro.rectTransform.anchoredPosition = targetPosition;

                elapsedTime += Time.deltaTime;
                yield return null;
            }
        }
    }
}