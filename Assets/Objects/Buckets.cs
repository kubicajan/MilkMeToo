using Objects.UnlockableObjectClasses;
using UnityEngine;

namespace Objects
{
    public class Buckets : UnlockableObject
    {
        public Buckets()
        {
            objectName = "Capy";
            description = "BAKET";
            kokButtonDescription = "You do not need to keep the milk in the mouth anymore.";
            shopButtonBuyPrice = 15;
            kokButtonUnlockPrice = 5;
            productionPower = 1f;
            interval = 1f;
        }
    }
}