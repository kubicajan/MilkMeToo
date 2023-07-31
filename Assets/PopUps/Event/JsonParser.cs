using System.Collections.Generic;
using UnityEngine;

namespace PopUps
{
    public class JsonParser : MonoBehaviour
    {
        [System.Serializable]
        public class Result
        {
            public string type;
            public string text;
            public string effect;
        }

        [System.Serializable]
        public class ResultHolder
        {
            public List<Result> Universal;
            public List<Result> People;
            public List<Result> Thoughts;
            public List<Result> Fifty;
            public List<Result> Investments;
            public List<Result> Things;
        }
        
        [System.Serializable]
        public class Scenario
        {
            public string Question;
            public string[] Descriptions;
        }

        [System.Serializable]
        public class ScenarioHolder
        {
            public Scenario People;
            public Scenario Thoughts;
            public Scenario Fifty;
            public Scenario Investments;
            public Scenario Things;
        }

        public static JsonParser instance;

        private ResultHolder resultHolder;
        private ScenarioHolder scenarioHolder;

        private void Awake()
        {
            TextAsset resultFile = Resources.Load<TextAsset>("EventResults");
            TextAsset scenarioFile = Resources.Load<TextAsset>("EventScenarios");
            ResultHolder resultHolder = JsonUtility.FromJson<ResultHolder>(resultFile.text);
            ScenarioHolder scenarioHolder = JsonUtility.FromJson<ScenarioHolder>(scenarioFile.text);

            if (instance != null && instance != this)
            {
                Destroy(this.gameObject);
            }
            else
            {
                instance = this;
            }
        }

        private void g()
        {
            float random = Random.Range(0, 6);
        }
    }
}