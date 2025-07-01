using System.Collections.Generic;

namespace PrometheusExporter.Metrics
{
    class MetricsCollectedEvent
    {
        public int SampleNumber { get; internal set; }
        public int SciencePoints { get; internal set; }
        public Dictionary<string, int> GoodAmounts { get; set; } = new();
        public Dictionary<string, int> GoodCapacities { get; set; } = new();
        public int TotalBeaverCount { get; internal set; }
        public int AdultBeaverCount { get; internal set; }
        public int ChildBeaverCount { get; internal set; }
        public int TotalBotCount { get; internal set; }
    }
}