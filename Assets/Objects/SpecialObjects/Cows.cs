using System;
using System.Collections;
using System.Linq;
using GooglePlayGames;
using Managers;
using Objects.Abstract.ActiveObjectClasses;
using UnityEngine;
using UnityEngine.UI;
using Utilities;
using Random = UnityEngine.Random;

namespace Objects.SpecialObjects
{
    public class Cows : ActiveKokTreeObject
    {
        [SerializeField] private Button anotherCow;
        [SerializeField] private Button anotherAnotherCow;
        private Transform vemenButtonTransform;
        private bool loaded = false;
        private bool milkmanLoaded = false;

        public Cows()
        {
            objectCounter = 1;
            effectInfo = "CLICK UPGRADE";
            objectName = "Cows";
            description = "It is like you are milking them all at once.";
            kokButtonDescription = "You can get more of them? \n \n <b> Gives extra production to your clicks </b>";
            shopButtonBuyPrice = 10000;
            kokButtonUnlockPrice = 800000;
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
            SaveManager.instance.SetUpdateCountBoughtWrapper(this.GetType().ToString(), value);
            shopButtonBuyPrice = CalculatePrice();
            SaveManager.instance.UpdateShopBuyPriceWrapper(this.GetType().ToString(), shopButtonBuyPrice);
        }

        private bool iAmMilking = false;
        private Coroutine timerCoroutine = null;

        private void IamMilkingNow()
        {
            iAmMilking = true;
            if (timerCoroutine != null)
            {
                StopCoroutine(timerCoroutine);
            }

            timerCoroutine = StartCoroutine(StartTimer());
        }

        private IEnumerator StartTimer()
        {
            yield return new WaitForSeconds(0.3f);
            iAmMilking = false;
        }

        public bool AmIMilkingNow()
        {
            return iAmMilking;
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

            Decimal ggtmp = Decimal.Parse(data.ShopBuyPrice);
            if (ggtmp != 0)
            {
                this.shopButtonBuyPrice = ggtmp;
            }

            Decimal gggg = Decimal.Parse(data.AmountMilked);
            this.allTimeMilked = gggg;
            if (SaveManager.instance.GetTimesProud() != 0)
            {
                productionPower = 10000;
            }
        }

        protected override void Start()
        {
            anotherCow.gameObject.SetActive(false);
            anotherAnotherCow.gameObject.SetActive(false);
            base.Start();
            vemenButtonTransform = GameObject.Find("vemenButton").transform;

            PlayGamesPlatform.Instance
                .LoadAchievements(achievements =>
                {
                    loaded = achievements
                        .Where(achivement => achivement.id == GPGSIds.achievement_touchy)
                        .Any(ach => ach.completed);
                });

            PlayGamesPlatform.Instance
                .LoadAchievements(achievements =>
                {
                    milkmanLoaded = achievements
                        .Where(achivement => achivement.id == GPGSIds.achievement_milk_man)
                        .Any(ach => ach.completed);
                });
            if (SaveManager.instance.GetTimesProud() != 0)
            {
                productionPower = 10;
            }
        }

        public void BirthACow(int value)
        {
            FreeObjects += value;
            SaveManager.instance.UpdateFreeCountWrapper(this.GetType().ToString(), 1);
        }

        private long counter = 0;

        public void MilkMe()
        {
            IamMilkingNow();
            if (!milkmanLoaded || !loaded)
            {
                counter++;
            }

            if (!milkmanLoaded && counter >= 10)
            {
                PlayGamesPlatform.Instance.IncrementAchievement(GPGSIds.achievement_milk_man, 10,
                    (bool success) => { });
                milkmanLoaded = true;
            }

            if (!loaded && counter >= 100)
            {
                counter = 0;
                PlayGamesPlatform.Instance.IncrementAchievement(GPGSIds.achievement_touchy, 100, (bool success) => { });
            }

            Decimal money = productionPower * (Decimal)ObjectCount;

            StartCoroutine(PlayMilkedCoroutine(vemenButtonTransform,
                MoveItABit(Helpers.GetObjectPositionRelativeToCanvas(vemenButtonTransform.position)), money));
        }

        private Vector2 MoveItABit(Vector2 position)
        {
            Random.seed = System.DateTime.Now.Millisecond;

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