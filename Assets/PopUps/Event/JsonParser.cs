using System.Collections.Generic;
using System.Linq;
using PopUps.Event;
using UnityEngine;

namespace PopUps
{
    public class JsonParser : MonoBehaviour
    {
        public static JsonParser instance;

        private static List<Scenario> scenarios;
        private static ResultsHolder results;

        private void Awake()
        {
            ConfigureLevelOne();
            if (instance != null && instance != this)
            {
                Destroy(this.gameObject);
            }
            else
            {
                instance = this;
            }
        }

        public void ConfigureLevelOne()
        {
            ConfigureLevel("EventResults", "EventScenarios");
        }

        private static void ConfigureLevel(string resultsName, string scenarioName)
        {
            TextAsset resultFile = Resources.Load<TextAsset>(resultsName);
            TextAsset scenarioFile = Resources.Load<TextAsset>(scenarioName);
            results = JsonUtility.FromJson<ResultsHolder>(resultFile.text);
            ScenarioHolder scenarioHolder = JsonUtility.FromJson<ScenarioHolder>(scenarioFile.text);
            scenarios = scenarioHolder.Objects;
        }

        public static void ConfigureLevelTwo()
        {
            ConfigureLevel("EventResultsHenry", "EventScenariosHenry");
        }

        public Scenario GetRandomScenario()
        {
            Random.seed = System.DateTime.Now.Millisecond;
            int random = Random.Range(0, scenarios.Count);

            return scenarios.ToArray()[random];
        }

        public Result GetResultBasedOnScenario(Scenario scenario)
        {
            ResultWrapper wrapper = results.Objects.Find(wrapper => wrapper.Name.Equals(scenario.Name));
            Random.seed = System.DateTime.Now.Millisecond;
            int random = Random.Range(0, wrapper.Results.Count());
            return wrapper.Results.ToArray()[random];
        }
    }
}