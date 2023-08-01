using System.Collections.Generic;
using System.Linq;
using PopUps.Event;
using UnityEngine;

namespace PopUps
{
    public class JsonParser : MonoBehaviour
    {
        public static JsonParser instance;

        private List<Scenario> scenarios;
        private ResultsHolder results;

        private void Awake()
        {
            TextAsset resultFile = Resources.Load<TextAsset>("EventResults");
            TextAsset scenarioFile = Resources.Load<TextAsset>("EventScenarios");
            results = JsonUtility.FromJson<ResultsHolder>(resultFile.text);
            ScenarioHolder scenarioHolder = JsonUtility.FromJson<ScenarioHolder>(scenarioFile.text);
            scenarios = scenarioHolder.Objects;

            if (instance != null && instance != this)
            {
                Destroy(this.gameObject);
            }
            else
            {
                instance = this;
            }
        }

        public Scenario GetRandomScenario()
        {
            int random = Random.Range(0, scenarios.Count);

            return scenarios.ToArray()[random];
        }

        public Result GetResultBasedOnScenario(Scenario scenario)
        {
            ResultWrapper wrapper = results.Objects.Find(wrapper => wrapper.Name.Equals(scenario.Name));
            int random = Random.Range(0, wrapper.Results.Count());
            return wrapper.Results.ToArray()[random];
        }
    }
}