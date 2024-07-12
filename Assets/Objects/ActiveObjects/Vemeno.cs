using System;
using Managers;
using Objects.Abstract.ActiveObjectClasses;
using UnityEngine;
using Random = UnityEngine.Random;

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
            shopButtonBuyPrice = 75000;
            kokButtonUnlockPrice = 120000000;
            productionPower = 1;
            // interval = 1f;
        }

        protected override void LoadData()
        {
            VyjimecnyElan data = SaveManager.instance.GetItemToUpdate(this.GetType().ToString());
            this.ObjectCount = data.CountBought;
            this.FreeObjects = data.FreeCount;
            Decimal gggg = Decimal.Parse(data.AmountMilked);
            this.allTimeMilked = gggg;
            Decimal ggtmp = Decimal.Parse(data.ShopBuyPrice);

            if (ggtmp != 0)
            {
                this.shopButtonBuyPrice = ggtmp;
            }
            MoneyManagerSingleton.instance.numberOfTitties = ObjectCount;
        }

        protected override void ResetHandler()
        {
            base.ResetHandler();
            MoneyManagerSingleton.instance.numberOfTitties = 0;
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();
            if (IsItTime2())
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

        private float timer2 = 0f;
        private float interval2 = 0.5f;

        private bool IsItTime2()
        {
            timer2 += Time.deltaTime;

            if (timer2 > (interval2))
            {
                timer2 = 0;
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