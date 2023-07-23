using Objects.UnlockableObjectClasses;
using UnityEngine;

namespace Objects
{
    public class Gingers : UnlockableObject
    {
        public Gingers()
        {
            objectName = "Gingers";
            description = "Pact with the devil";
            kokButtonDescription = "They do not have souls anyway";
            shopButtonBuyPrice = 15;
            kokButtonUnlockPrice = 5;
            productionPower = 1f;
            interval = 1f;
        }
    }
}