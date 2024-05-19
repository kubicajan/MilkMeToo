using System;
using Managers;
using Objects.Abstract.ActiveObjectClasses;
using Utilities;
using UnityEngine;


namespace Objects.ActiveObjects
{
    public class Cat : ActiveKokTreeObject
    {
        public Cat()
        {
            kokButtonStatus = ButtonStatus.AVAILABLE;
            shopButtonBuyPrice = 5;
            kokButtonUnlockPrice = 3;
            objectName = "Jeremy";
            description =
                "Look at this cat and don't mind that your competition started disappearing. You" +
                " can get more of his friends to help.";
            kokButtonDescription = "You find a little cat. Try to take it home, but it requires a fee..." +
                                   "\n \n What use is coin for him?";
            productionPower = 0.5m;
            interval = 0.5f;
        }

        protected override void ResetHandler()
        {
            base.ResetHandler();
            kokButtonStatus = ButtonStatus.AVAILABLE;
            KokTreeButtonStart();
        }

        protected override void ConfigureAndPlayMilked(Transform transformMe)
        {
            Decimal moneyMoney = objectCounter * productionPower * (MoneyManagerSingleton.instance.numberOfTitties + 1);
            StartCoroutine(PlayMilkedCoroutine(transformMe,
                Helpers.GetObjectPositionRelativeToCanvas(transformMe.position), moneyMoney));
        }
    }
}