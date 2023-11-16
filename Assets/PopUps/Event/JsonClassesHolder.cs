using System.Collections.Generic;
using Objects;

namespace PopUps.Event
{
    [System.Serializable]
    public class Result
    {
        public string type;
        public string text;
        public string effect;
        public EffectStatus effectType;
    }

    [System.Serializable]
    public class ResultWrapper
    {
        public string Name;
        public Result[] Results;
    }
        
    [System.Serializable]
    public class ResultsHolder
    {
        public List<ResultWrapper> Objects;
    }

    [System.Serializable]
    public class Scenario
    {
        public string Name;
        public string Question;
        public string[] Descriptions;
    }

    [System.Serializable]
    public class ScenarioHolder
    {
        public List<Scenario> Objects;
    }
}