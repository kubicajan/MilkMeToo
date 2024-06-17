using Managers;
using Objects.Abstract.ActiveObjectClasses;
using UnityEngine;

namespace Objects.ActiveObjects
{
    public class Vemeno : ActiveKokTreeObject
    {
        [SerializeField] private Animator animator;

        public Vemeno()
        {
            objectName = "Vemeno";
            description = "ANOTHER ONE?";
            kokButtonDescription = "This does not seem natural \n \n <b> Gives cats extra production </b> ";
          
            shopButtonBuyPrice = 15;
            originalPrice = 15;
            
            kokButtonUnlockPrice = 5;
            productionPower = 1;
            interval = 1f;
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();
            if (ObjectCount > 0)
            {
                animator.SetBool("isTiddy", true);
            }

            if (Random.Range(0, 10) < 2)
            {
                animator.SetBool("switcheroo", true);
            }
            else
            {
                animator.SetBool("switcheroo", false);
            }
        }

        public override void BuyObject()
        {
            MoneyManagerSingleton.instance.numberOfTitties++;
            base.BuyObject();
        }
    }
}