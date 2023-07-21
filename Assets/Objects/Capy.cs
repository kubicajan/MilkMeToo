using Objects.UnlockableObjectClasses;
using UnityEngine;

namespace Objects
{
    public class Capy : UnlockableObject
    {
        public Capy()
        {
            objectName = "capy";
            shopButtonBuyPrice = 15;
            kokButtonUnlockPrice = 5;
            productionPower = 1f;
            interval = 1f;
        }

        protected override void UnlockAnotherButton()
        {
            Debug.Log("nothing yet");
        }
    }
}