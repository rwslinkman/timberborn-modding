using Timberborn.SingletonSystem;
using UnityEngine;

namespace PrometheusExporter.Metrics
{
    class MetricsCollector : ILoadableSingleton
    {
        private readonly EventBus eventBus;
        private int samplesCollected;

        public MetricsCollector(EventBus bus)
        {
            this.eventBus = bus;
            this.samplesCollected = 0;
        }

        public void Load()
        {
            eventBus.Register(this);
        }

        [OnEvent]
        public void CollectMetrics(CollectMetricsCommand cmd)
        {
            samplesCollected++;
            
            var metrics = new MetricsCollectedEvent();
            metrics.sampleNumber = samplesCollected;
            // TODO: collect data, store in event class

            eventBus.Post(metrics);
        }
    }
}