using System.Collections;
using System.Collections.Generic;
using Managers;
using Objects.Abstract.ActiveObjectClasses;
using UnityEngine;
using UnityEngine.UI;
using Utilities;

namespace Objects.ActiveObjects
{
    public class Capy : ActiveKokTreeObject
    {
        [SerializeField] private Button anotherCapy;
        [SerializeField] private Button yetAnotherCapy;
        [SerializeField] private Button yetAnotherAnotherCapy;

        public Capy()
        {
            objectName = "Just a little boy";
            description =
                "He brought his own family to where animals are equal. You can bring more of them.";
            kokButtonDescription =
                "The chillest animal on the block. \n \n Perhaps he joins you for some of your wealth.";
            shopButtonBuyPrice = 15;
            kokButtonUnlockPrice = 5;
            productionPower = 0.3m;
            interval = 1f;
        }

        private bool achievementUnlocked = false;

        public override void BuyObject()
        {
            base.BuyObject();
            if (!achievementUnlocked && objectCounter >= 1)
            {
                Social.ReportProgress(GPGSIds.achievement_mans_best_friend,100.0f, (bool success) => { });
                achievementUnlocked = true;
            }
        }

        protected override void Start()
        {
            anotherCapy.gameObject.SetActive(false);
            yetAnotherCapy.gameObject.SetActive(false);
            yetAnotherAnotherCapy.gameObject.SetActive(false);
            base.Start();
        }

        protected override void ActivateThings(int value)
        {
            base.ActivateThings(value);
            StartCoroutine(PlayMilkedCoroutine(value));
        }

        private IEnumerator PlayMilkedCoroutine(int value)
        {
            yield return new WaitForSeconds(0.68f);
            yetAnotherAnotherCapy.gameObject.SetActive(value > 6);
            yield return new WaitForSeconds(0.50f);
            yetAnotherCapy.gameObject.SetActive(value > 4);
            yield return new WaitForSeconds(0.3f);
            anotherCapy.gameObject.SetActive(value > 2);
        }

        public override void PlayMilked(int? number)
        {
            if (number != null)
            {
                switch (number)
                {
                    case 1:
                        ConfigureAndPlayMilked(anotherCapy.transform);
                        break;
                    case 2:
                        ConfigureAndPlayMilked(yetAnotherCapy.transform);
                        break;
                    case 3:
                        ConfigureAndPlayMilked(yetAnotherAnotherCapy.transform);
                        break;
                }
            }
            else
            {
                ConfigureAndPlayMilked(primalSpriteButton.gameObject.transform);
            }
        }
    }
}