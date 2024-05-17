using Managers;
using Objects.Abstract;
using PopUps;
using UnityEngine;
using UnityEngine.UI;

namespace Objects.SpecialObjects
{
    public class TheLover : KokTreeObject
    {
        [SerializeField] public Button cowsButton;
        private Cows cows;
        private float currentTime = 5;
        private int fatherToKids;
        private string description;

        public TheLover()
        {
            objectName = "The Lover";
            kokButtonDescription = "THAT is what I call a MALE";
            kokButtonUnlockPrice = 5;
            effectInfo = "COW BABIES ???";
            description = "He has found the promised land. He intends to stay";
        }

        protected override void Start()
        {
            base.Start();
            cows = cowsButton.transform.GetComponent<Cows>();
            if (SaveManager.instance.GetItemToUpdate(this.GetType().ToString()).KokTreeStatus == ButtonStatus.BOUGHT)
            {
                primalSpriteButton.gameObject.SetActive(true);
            }
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();
            CreateNewCow();
        }

        public override void BuyUpgrade()
        {
            base.BuyUpgrade();
            primalSpriteButton.gameObject.SetActive(true);
        }

        public override void Clicked()
        {
            base.Clicked();
            string fatherTo = $"Father to:\n{fatherToKids}";
            InformationPopUp.instance.ShowPopUp(objectName, description, fatherTo,
                primalSpriteButton.image.sprite, "UNIQUE");
        }

        private void UpdatePopUpCount()
        {
            if (InformationPopUp.instance.isActiveAndEnabled && clickedInfo)
            {
                this.Clicked();
            }
            else
            {
                clickedInfo = false;
            }
        }

        private void CreateNewCow()
        {
            if (kokButtonStatus == ButtonStatus.BOUGHT)
            {
                currentTime -= Time.deltaTime;
                // timerText.text = FormatTime(currentTime);

                if (currentTime <= 0f)
                {
                    currentTime = 5f;
                    fatherToKids += 1;
                    cows.BirthACow(1);
                    UpdatePopUpCount();
                }
            }
        }
    }
}