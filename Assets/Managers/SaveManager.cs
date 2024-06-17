using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
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
            const string DATA_FILE_NAME = "MarkuvDlouhyPindour";
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
                () => new VyjimecnyElan(className, kokTreeStatus, 0, "0", "0"));
        }

        public void UpdateCountBoughtWrapper(string className, int countBought)
        {
            UpdateItem(className, itemToUpdate => itemToUpdate.CountBought += countBought,
                () => new VyjimecnyElan(className, ButtonStatus.LOCKED, countBought, "0", "0"));
        }

        public void RestartCountBoughtWrapper(string className)
        {
            UpdateItem(className, itemToUpdate => itemToUpdate.CountBought = 0,
                () => new VyjimecnyElan(className, ButtonStatus.LOCKED, 0, "0", "0"));
        }

        public void UpdateShopBuyPriceWrapper(string className, Decimal shopBuyPrice)
        {
            UpdateItem(className, itemToUpdate => itemToUpdate.ShopBuyPrice = shopBuyPrice.ToString(),
                () => new VyjimecnyElan(className, ButtonStatus.LOCKED, 0, "0", shopBuyPrice.ToString()));
        }

        public void UpdateAmountMilkedWrapper(string className, Decimal amountMilked)
        {
            Decimal tmp = Decimal.Parse(GetItemToUpdate(className).AmountMilked) + amountMilked;
            UpdateItem(className, itemToUpdate => tmp.ToString(),
                () => new VyjimecnyElan(className, ButtonStatus.BOUGHT, 0, amountMilked.ToString(), "0"));
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

        public int GetMultiplier()
        {
            return wrapper.multiplier;
        }

        public Decimal GetCurrentMoney()
        {
            return Decimal.Parse(wrapper.currentMoney);
        }

        public void UpdateCurrentMoney(Decimal moneyToBeAdded)
        {
            wrapper.currentMoney = (GetCurrentMoney() + moneyToBeAdded).ToString();
        }


        public BigInteger GetTotalMoney()
        {
            return BigInteger.Parse(wrapper.totalMoney);
        }

        public void UpdateTotalMoney(BigInteger money)
        {
            wrapper.totalMoney = (GetTotalMoney() + money).ToString();
        }

        public void SetTutorialFinished(bool finished)
        {
            wrapper.tutorialFinished = finished;
        }

        public bool GetTutorialFinished()
        {
            return wrapper.tutorialFinished;
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
        [SerializeField] public string currentMoney = "0";
        [SerializeField] public bool tutorialFinished = false;
        [SerializeField] public string totalMoney = "0";
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
        public string AmountMilked;
        public string ShopBuyPrice = "0";

        public VyjimecnyElan(string name, ButtonStatus kokTreeStatus, int countBought, string amountMilked,
            string shopBuyPrice)
        {
            Name = name;
            KokTreeStatus = kokTreeStatus;
            CountBought = countBought;
            AmountMilked = amountMilked;
            ShopBuyPrice = shopBuyPrice;
        }
    }
}