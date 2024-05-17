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

        public void UpdateKokTreeStatusWrapper(string className, ButtonStatus kokTreeStatus)
        {
            UpdateItem(className, itemToUpdate => itemToUpdate.KokTreeStatus = kokTreeStatus,
                () => new VyjimecnyElan(className, kokTreeStatus, 0, 0));
        }

        public void UpdateCountBoughtWrapper(string className, int countBought)
        {
            UpdateItem(className, itemToUpdate => itemToUpdate.CountBought += countBought,
                () => new VyjimecnyElan(className, ButtonStatus.LOCKED, countBought, 0));
        }

        public void UpdateAmountMilkedWrapper(string className, float amountMilked)
        {
            UpdateItem(className, itemToUpdate => itemToUpdate.AmountMilked += amountMilked,
                () => new VyjimecnyElan(className, ButtonStatus.BOUGHT, 0, amountMilked));
        }

        public void UpdateCurrentMoney(float moneyToBeAdded)
        {
            wrapper.currentMoney += moneyToBeAdded;
        }

        public void UpdateMultiplier(int multiplier)
        {
            wrapper.multiplier = multiplier;
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
            Debug.Log("I AM LOADING");
            Debug.Log("existing? " + File.Exists(saveFilePath));
            Debug.Log(saveFilePath);
            if (File.Exists(saveFilePath))
            {
                string jsonData = File.ReadAllText(saveFilePath);
                Debug.Log(jsonData);
                Wrapper tmpWrapper = JsonUtility.FromJson<Wrapper>(jsonData);
                wrapper = tmpWrapper;
                foreach (var item in wrapper.listToBeSaved)
                {
                    Debug.Log(item.Name + " " + item.KokTreeStatus + " " + item.AmountMilked + " " + item.CountBought);
                }
            }
            else
            {
                wrapper = new Wrapper();
            }
        }

        private void SaveGameData()
        {
            Debug.Log("I am saving");
            Directory.CreateDirectory(Path.GetDirectoryName(saveFilePath));
            string savePlayerData = JsonUtility.ToJson(wrapper, true);
            File.WriteAllText(saveFilePath, savePlayerData);
            Debug.Log(savePlayerData);
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

        public VyjimecnyElan(string name, ButtonStatus kokTreeStatus, int countBought, float amountMilked)
        {
            Name = name;
            KokTreeStatus = kokTreeStatus;
            CountBought = countBought;
            AmountMilked = amountMilked;
        }
    }
}