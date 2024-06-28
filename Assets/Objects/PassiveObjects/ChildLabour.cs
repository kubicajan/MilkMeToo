using Managers;
using Objects.Abstract;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Objects.PassiveObjects
{
    public class ChildLabour : PassiveKokTreeObjects
    {
        [SerializeField] private GameObject anotherSlave;
        [SerializeField] private GameObject yetAnotherSlave;
        public ChildLabour()
        {
            objectName = "Child Labour";
            kokButtonDescription = "Let everyone help";
            kokButtonUnlockPrice = 15000000;
            multiplicationBonus = 40;
        }
        
        protected override void Start()
        {
            anotherSlave.gameObject.SetActive(false);
            yetAnotherSlave.gameObject.SetActive(false);
            base.Start();
        }
        
        protected override void LoadAllAssets()
        {
            if (SaveManager.instance.GetItemToUpdate(this.GetType().ToString()).KokTreeStatus == ButtonStatus.BOUGHT)
            {
                primalSpriteButton.gameObject.SetActive(true); 
                anotherSlave.gameObject.SetActive(true);
                yetAnotherSlave.gameObject.SetActive(true);
            }
        }
        
        
        
        protected override void ResetHandler()
        {
            base.ResetHandler();
            anotherSlave.gameObject.SetActive(false);
            yetAnotherSlave.gameObject.SetActive(false);
        }
        
        public override void BuyUpgrade()
        {
            base.BuyUpgrade();
            anotherSlave.gameObject.SetActive(true);
            yetAnotherSlave.gameObject.SetActive(true);
        }
        
        protected override void ActivateAllOfThem()
        {
            base.ActivateAllOfThem();
            anotherSlave.gameObject.SetActive(true);
            yetAnotherSlave.gameObject.SetActive(true);
        }
    }
}