using Objects.UnlockableObjectClasses;
using UnityEngine;

namespace Objects
{
    public class Cows : UnlockableObject
    {
        public Cows()
        {
            objectName = "Cows";
            description = "Interesting";
            kokButtonDescription = "You can get more of them?";
            shopButtonBuyPrice = 15;
            kokButtonUnlockPrice = 5;
            productionPower = 1f;
            interval = 1f;
        }
    }
}