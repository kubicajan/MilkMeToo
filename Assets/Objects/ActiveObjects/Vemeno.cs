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
            shopButtonBuyPrice = 50000;
            kokButtonUnlockPrice = 2500000;
            productionPower = 1;
            // interval = 1f;
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();
            if (IsItTime() )
            {
                Debug.Log("Animation finished");
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
        }
        
        private float timer = 0f;
        private float interval2 = 0.5f;

        private bool IsItTime()
        {
            timer += Time.deltaTime;

            if (timer > (interval2))
            {
                timer = 0;
                return true;
            }

            return false;
        }

        public override void BuyObject()
        {
            MoneyManagerSingleton.instance.numberOfTitties++;
            base.BuyObject();
        }
    }
}