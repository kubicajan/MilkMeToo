using GooglePlayGames;
using Managers;
using Objects.Abstract.ActiveObjectClasses;
using UnityEngine;
using UnityEngine.UI;
using Utilities;

namespace Objects.SpecialObjects
{
    public class Cows : ActiveKokTreeObject
    {
        [SerializeField] private Button anotherCow;
        [SerializeField] private Button anotherAnotherCow;
        private Transform vemenButtonTransform;

        public Cows()
        {
            objectCounter = 1;
            effectInfo = "CLICK UPGRADE";
            objectName = "Cows";
            description = "It is like you are milking them all at once";
            kokButtonDescription = "You can get more of them?";
            shopButtonBuyPrice = 15;
            kokButtonUnlockPrice = 5;
            productionPower = 1;
        }

        protected override void ActivateThings(int value)
        {
            if (value > 1)
            {
                objectCounter = value;
                primalSpriteButton.gameObject.SetActive(true);

                if (value > 2)
                {
                    anotherCow.gameObject.SetActive(true);
                    if (value > 3)
                    {
                        anotherAnotherCow.gameObject.SetActive(true);
                    }
                }
            }
            else
            {
                objectCounter = 1;
                primalSpriteButton.gameObject.SetActive(false);
                anotherCow.gameObject.SetActive(false);
                anotherAnotherCow.gameObject.SetActive(false);
            }
        }

        protected override void LoadData()
        {
            VyjimecnyElan data = SaveManager.instance.GetItemToUpdate(this.GetType().ToString());
            if (data.CountBought == 0)
            {
                this.ObjectCount = 1;
            }
            else
            {
                this.ObjectCount = data.CountBought;
            }

            this.allTimeMilked = data.AmountMilked;
        }


        protected override void Start()
        {
            anotherCow.gameObject.SetActive(false);
            anotherAnotherCow.gameObject.SetActive(false);
            base.Start();
            vemenButtonTransform = GameObject.Find("vemenButton").transform;
        }

        public void BirthACow(int value)
        {
            ObjectCount += value;
            SaveManager.instance.UpdateCountBoughtWrapper(this.GetType().ToString(), 1);
        }

        private int counter = 0;

        public void MilkMe()
        {
            counter++;
            if (counter <= 10)
            {
                PlayGamesPlatform.Instance.IncrementAchievement("CgkIrdTOtaYPEAIQBg", 1, (bool success) => { });
            }

            float money = productionPower * ObjectCount;

            StartCoroutine(PlayMilkedCoroutine(vemenButtonTransform,
                MoveItABit(Helpers.GetObjectPositionRelativeToCanvas(vemenButtonTransform.position)), money));
        }

        private Vector2 MoveItABit(Vector2 position)
        {
            float randomY = Random.Range(5f, 25f);
            float randomX = Random.Range(10f, 100f);
            float makeNegativeOrNotY = Random.Range(0f, 1f) < 0.5 ? -1 : 1;
            float makeNegativeOrNotX = Random.Range(0f, 1f) < 0.5 ? -1 : 1;

            float moveY = randomY * makeNegativeOrNotY;
            float moveX = randomX * makeNegativeOrNotX;

            return position - new Vector2(-70f + (moveX), +240f + (moveY));
        }
    }
}