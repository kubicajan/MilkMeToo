using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Objects;

namespace Managers
{
    public class SaveManager : MonoBehaviour
    {
        public Wrapper wrapper;
        public static SaveManager instance;
        private string saveFilePath;

        private void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(this.gameObject);
            }
            else
            {
                instance = this;
            }

            string dataStringPath = Application.persistentDataPath;
            const string DATA_FILE_NAME = "saveFile";
            saveFilePath = Path.Combine(dataStringPath, DATA_FILE_NAME);
            Load();
        }

        public VyjimecnyElan GetItemToUpdate(string className)
        {
            return wrapper.listToBeSaved.Find(item => item.Name == className);
        }

        public int GetFatherTo()
        {
            return wrapper.fatherTo;
        }

        public void UpdateFatherTo(int fatherTo)
        {
            wrapper.fatherTo += fatherTo;
        }

        public void UpdateKokTreeStatusWrapper(string className, ButtonStatus kokTreeStatus)
        {
            UpdateItem(className, itemToUpdate => itemToUpdate.KokTreeStatus = kokTreeStatus,
                () => new VyjimecnyElan(className, kokTreeStatus, 0, 0, 0));
        }

        public void UpdateCountBoughtWrapper(string className, int countBought)
        {
            UpdateItem(className, itemToUpdate => itemToUpdate.CountBought += countBought,
                () => new VyjimecnyElan(className, ButtonStatus.LOCKED, countBought, 0, 0));
        }

        public void UpdateShopBuyPriceWrapper(string className, float shopBuyPrice)
        {
            UpdateItem(className, itemToUpdate => itemToUpdate.ShopBuyPrice = shopBuyPrice,
                () => new VyjimecnyElan(className, ButtonStatus.LOCKED, 0, 0, shopBuyPrice));
        }

        public void UpdateAmountMilkedWrapper(string className, float amountMilked)
        {
            UpdateItem(className, itemToUpdate => itemToUpdate.AmountMilked += amountMilked,
                () => new VyjimecnyElan(className, ButtonStatus.BOUGHT, 0, amountMilked, 0));
        }

        public void UpdateCurrentMoney(float moneyToBeAdded)
        {
            wrapper.currentMoney += moneyToBeAdded;
        }

        public void UpdateMultiplier(int multiplier)
        {
            wrapper.multiplier = multiplier;
        }
        
        public void UpdateMommyUnlockCounter(int mommyUnlockCounter)
        {
            wrapper.mommyUnlockCounter = mommyUnlockCounter;
        }
        
        public int GetMommyUnlockCounter()
        {
            return wrapper.mommyUnlockCounter;
        }
        
        public void UpdateTimesProud(int timesProud)
        {
            wrapper.timesProud += timesProud;
        }
        
        public int GetTimesProud()
        {
            return wrapper.timesProud;
        }

        private void UpdateItem(string className, Action<VyjimecnyElan> updateAction, Func<VyjimecnyElan> createNewItem)
        {
            VyjimecnyElan itemToUpdate = GetItemToUpdate(className);

            if (itemToUpdate != null)
            {
                updateAction(itemToUpdate);
            }
            else
            {
                wrapper.listToBeSaved.Add(createNewItem());
            }
        }

        public float GetCurrentMoney()
        {
            return wrapper.currentMoney;
        }

        public int GetMultiplier()
        {
            return wrapper.multiplier;
        }

        public void Load()
        {
            Debug.Log(saveFilePath);
            if (File.Exists(saveFilePath))
            {
                string jsonData = File.ReadAllText(saveFilePath);
                Wrapper tmpWrapper = JsonUtility.FromJson<Wrapper>(jsonData);
                wrapper = tmpWrapper;
            }
            else
            {
                wrapper = new Wrapper();
            }
        }

        private void SaveGameData()
        {
            Directory.CreateDirectory(Path.GetDirectoryName(saveFilePath));
            string savePlayerData = JsonUtility.ToJson(wrapper, true);
            File.WriteAllText(saveFilePath, savePlayerData);
            Debug.Log($"I saved to{saveFilePath}");
        }

        public void OnApplicationPause(bool pauseStatus)
        {
            if (pauseStatus)
            {
                SaveGameData();
            }
        }

        public void OnApplicationQuit()
        {
            Debug.Log("CLOSING THIS");
            SaveGameData();
        }
    }

    [Serializable]
    public class Wrapper
    {
        [SerializeField] public float currentMoney = 0;
        [SerializeField] public int timesProud = 0;
        [SerializeField] public int mommyUnlockCounter = 0;
        [SerializeField] public int fatherTo = 0;
        [SerializeField] public int multiplier = 0;
        [SerializeField] public List<VyjimecnyElan> listToBeSaved = new List<VyjimecnyElan>();
    }


    [Serializable]
    public class VyjimecnyElan
    {
        public string Name;
        public ButtonStatus KokTreeStatus;
        public int CountBought;
        public float AmountMilked;
        public float ShopBuyPrice;

        public VyjimecnyElan(string name, ButtonStatus kokTreeStatus, int countBought, float amountMilked,
            float shopBuyPrice)
        {
            Name = name;
            KokTreeStatus = kokTreeStatus;
            CountBought = countBought;
            AmountMilked = amountMilked;
            ShopBuyPrice = shopBuyPrice;
        }
    }
}