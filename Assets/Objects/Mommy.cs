using Objects.UnlockableObjectClasses;
using UnityEngine;

namespace Objects
{
    public class Mommy : UnlockableObject
    {
        public Mommy()
        {
            objectName = "Mommy";
            description = "She is proud of you";
            kokButtonDescription = "The ultimate reward";
            shopButtonBuyPrice = 15;
            kokButtonUnlockPrice = 5;
            productionPower = 1f;
            interval = 1f;
        }
    }
}