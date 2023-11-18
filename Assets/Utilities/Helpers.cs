using System;
using System.Text.RegularExpressions;
using Objects.Abstract.ActiveObjectClasses;
using UnityEngine;

namespace Utilities
{
    public static class Helpers
    {
        public static Vector2 GetObjectPositionRelativeToCanvas(Vector3 position)
        {
            Vector3 screenPosition = RectTransformUtility.WorldToScreenPoint(null, position);
            RectTransform canvasRectTransform = GameObject.Find("Canvas").GetComponent<RectTransform>();

            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRectTransform, screenPosition, null,
                out Vector2 canvasPosition);

            return canvasPosition;
        }

        public static ActiveKokTreeObject GetActiveKokTreeObject(string objectName)
        {
            
            string objectNameWithCapitalLetter = char.ToUpper(objectName[0]) + objectName.Substring(1);
            Type type = Type.GetType("Objects.ActiveObjects." + objectNameWithCapitalLetter);
            return (ActiveKokTreeObject)GameObject.Find(objectNameWithCapitalLetter).GetComponent(type);
        }
        
        public static Match ParseRegex(string input, string pattern)
        {
            Regex regex = new Regex(pattern);
            return regex.Match(input);
        }
    }
}