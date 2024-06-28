using System;
using System.Globalization;
using System.Numerics;
using System.Text.RegularExpressions;
using Objects.Abstract.ActiveObjectClasses;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

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

        public static string ConvertNumbersToString(BigInteger number)
        {
            BigInteger MILION = 1000000;
            BigInteger BILION = 1000000000;
            BigInteger TRILLION = 1000000000000;
            BigInteger QUADRILION = 1000000000000000;
            BigInteger QUINTILION = 1000000000000000000;

            if (number > QUINTILION)
            {
                return (number / 1000000000000).ToString() + "E";
            }
            else if (number > QUADRILION)
            {
                return (number / 1000000000).ToString() + "P";
            }
            else if (number > TRILLION)
            {
                return (number / 1000000000).ToString() + "T";
            }
            else if (number > BILION)
            {
                return (number / 1000000).ToString() + "B";
            }
            else if (number > MILION)
            {
                return (number / 1000).ToString() + "M";
            }
            else
            {
                return number.ToString();
            }
        }


        public static string ConvertNumbersToString(Decimal number, bool ignoreLast = false)
        {
            const Decimal MILION = 1000000;
            const Decimal BILION = 1000000000;
            const Decimal TRILLION = 1000000000000;
            const Decimal QUADRILION = 1000000000000000;
            const Decimal QUINTILION = 1000000000000000000;
            CultureInfo culture = CultureInfo.GetCultureInfo("en-US");

            switch (number)
            {
                case > QUINTILION:
                    return Math.Round(number / 1000000000000000, 3).ToString("N2", culture) + "E";
                    break;
                case > QUADRILION:
                    return Math.Round(number / 1000000000000, 3).ToString("N2", culture) + "P";
                    break;
                case > TRILLION:
                    return Math.Round(number / 1000000000000, 3).ToString("N2", culture) + "T";
                    break;
                case > BILION:
                    return Math.Round(number / 1000000000, 3).ToString("N2", culture) + "B";
                    break;
                case > MILION:
                    return Math.Round(number / 1000000, 3).ToString("N2", culture) + "M";
                    break;
                default:
                    string gg = number.ToString("N1", culture);
                    if (!ignoreLast && gg.EndsWith(".0"))
                    {
                        gg = gg.TrimEnd('0').TrimEnd('.');
                    }

                    return gg;
                // return Math.Round(number, 2).ToString();
            }
        }
    }
}