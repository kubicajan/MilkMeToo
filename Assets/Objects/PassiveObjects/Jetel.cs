using Objects.Abstract;
using UnityEngine;
using UnityEngine.UI;

namespace Objects.PassiveObjects
{
    public class Jetel : PassiveKokTreeObjects
    {
        [SerializeField] private Image anotherJetel;
        [SerializeField] private Image yetAnotherJetel;
        [SerializeField] private Image yetAnotherAnotherJetel;

        public Jetel()
        {
            kokButtonStatus = ButtonStatus.AVAILABLE;
            objectName = "Jetel";
            kokButtonDescription = "Maybe it is time to start feeding your cows";
            kokButtonUnlockPrice = 5;
        }
        
        protected override void Start()
        {
            base.Start();
            anotherJetel.gameObject.SetActive(false);
            yetAnotherJetel.gameObject.SetActive(false);
            yetAnotherAnotherJetel.gameObject.SetActive(false);
        }
        
        public override void BuyUpgrade()
        {
            base.BuyUpgrade();
            anotherJetel.gameObject.SetActive(true);
            yetAnotherJetel.gameObject.SetActive(true);
            yetAnotherAnotherJetel.gameObject.SetActive(true);
        }
        
        protected override void ResetHandler()
        {
            base.ResetHandler();
            kokButtonStatus = ButtonStatus.AVAILABLE;
            anotherJetel.gameObject.SetActive(false);
            yetAnotherJetel.gameObject.SetActive(false);
            yetAnotherAnotherJetel.gameObject.SetActive(false);
            KokTreeButtonStart();
        }
        
        protected override void ActivateAllOfThem()
        {
            base.ActivateAllOfThem();
            anotherJetel.gameObject.SetActive(true);
            yetAnotherJetel.gameObject.SetActive(true);
            yetAnotherAnotherJetel.gameObject.SetActive(true);
        }
    }
}