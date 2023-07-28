using UnityEngine;

namespace Utilities
{
    public class Helpers
    {
        public static Vector2 GetObjectPositionRelativeToCanvas(Vector3 position)
        {
            Vector3 screenPosition = RectTransformUtility.WorldToScreenPoint(null, position);
            RectTransform canvasRectTransform = GameObject.Find("Canvas").GetComponent<RectTransform>();

            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRectTransform, screenPosition, null,
                out Vector2 canvasPosition);

            return canvasPosition;
        }
    }
}