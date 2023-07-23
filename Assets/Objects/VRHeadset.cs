using Objects.UnlockableObjectClasses;
using UnityEngine;

namespace Objects
{
    public class VRHeadset : UnlockableObject
    {
        public VRHeadset()
        {
            objectName = "VRHeadset";
            description = "Eyo sus";
            kokButtonDescription = "Put them into the matrix to make them feel better";
            shopButtonBuyPrice = 15;
            kokButtonUnlockPrice = 5;
            productionPower = 1f;
            interval = 1f;
        }
    }
}