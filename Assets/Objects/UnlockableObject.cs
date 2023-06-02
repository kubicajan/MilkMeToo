using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Objects
{
    public abstract class UnlockableObject : MonoBehaviour
    {
        [SerializeField] public GameObject sprite;
        [SerializeField] public TextMeshProUGUI counter;
        [SerializeField] public Button shopButton;
        [SerializeField] public Button kokButton;

        [SerializeField] private Sprite availableKokButtonSprite;
        [SerializeField] private Sprite lockedKokButtonSprite;
        [SerializeField] private Sprite unlockedKokButtonSprite;

        private bool unlocked;
        private int count;
        protected string objectName = "";

        private void Start()
        {
            unlocked = false;
            count = 0;

            shopButton.enabled = false;
            sprite.SetActive(false);
            counter.text = "";
            kokButton.image.sprite = lockedKokButtonSprite;
            shopButton.GetComponentInChildren<TextMeshProUGUI>().text = "unavailable";
            shopButton.image.color = Color.gray;
        }

        //TODO: potreba dodelat logiku co unlockuje postupne picky

        public bool IsUnlocked()
        {
            return unlocked;
        }

        public void Unlock()
        {
            unlocked = true;
            shopButton.enabled = true;
            kokButton.image.sprite = unlockedKokButtonSprite;
            shopButton.GetComponentInChildren<TextMeshProUGUI>().text = "buy " + objectName;
            shopButton.image.color = Color.cyan;
        }

        public void BuyAmount(int addAmount)
        {
            count += addAmount;
            if (count > 0)
            {
                sprite.SetActive(true);
                counter.text = count.ToString();
            }
        }

        public void RemoveAmount(int removeAmount)
        {
            count -= removeAmount;
            if (count < 0)
            {
                count = 0;
            }

            if (count <= 0)
            {
                sprite.SetActive(false);
                counter.text = "";
            }
        }

        public int GetCount()
        {
            return count;
        }
    }
}