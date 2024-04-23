using System;
using System.IO;
using UnityEngine;

namespace Managers
{
    public class SaveManager : MonoBehaviour
    {
        public Wrapper wrapper;
        public static SaveManager instance;

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
        }
        
        //todo: reading from file.

        public void Start()
        {
            wrapper = new Wrapper();
        }

        //todo: path
        private void SaveGameData()
        {
            string savePlayerData = JsonUtility.ToJson(wrapper);
            File.WriteAllText("", savePlayerData);
        }

        void OnApplicationPause(bool pauseStatus)
        {
            if (pauseStatus)
            {
                SaveGameData();
            }
        }

        void OnApplicationQuit()
        {
            SaveGameData();
        }
    }

    public class Wrapper
    {
        private VyjimecnyElan InitializeDefault() => new VyjimecnyElan(false, 0, 0);

        // Initialize properties with default values using the method
        public VyjimecnyElan Cat { get; set; }
        public VyjimecnyElan Capy { get; set; }
        public VyjimecnyElan Jetel { get; set; }
        public VyjimecnyElan Seno { get; set; }
        public VyjimecnyElan VrHeadset { get; set; }
        public VyjimecnyElan Lover { get; set; }
        public VyjimecnyElan Drugs { get; set; }
        public VyjimecnyElan Cows { get; set; }
        public VyjimecnyElan Slaves { get; set; }
        public VyjimecnyElan ChildLabour { get; set; }
        public VyjimecnyElan CumBuckets { get; set; }
        public VyjimecnyElan Vemeno { get; set; }
        public VyjimecnyElan Mommy { get; set; }

        //todo: dodelat nastenku a mommy asi taky bude jebat
        public Wrapper()
        {
            Cat = InitializeDefault();
            Capy = InitializeDefault();
            Jetel = InitializeDefault();
            Seno = InitializeDefault();
            VrHeadset = InitializeDefault();
            Lover = InitializeDefault();
            Drugs = InitializeDefault();
            Cows = InitializeDefault();
            Slaves = InitializeDefault();
            ChildLabour = InitializeDefault();
            CumBuckets = InitializeDefault();
            Vemeno = InitializeDefault();
            Mommy = InitializeDefault();
        }
    }


    public class VyjimecnyElan
    {
        private bool Unlocked;
        private int CountBought;
        private long AmountMilked;

        internal VyjimecnyElan(bool unlocked, int countBought, long amountMilked)
        {
            this.Unlocked = unlocked;
            this.CountBought = countBought;
            this.AmountMilked = amountMilked;
        }
    }
}