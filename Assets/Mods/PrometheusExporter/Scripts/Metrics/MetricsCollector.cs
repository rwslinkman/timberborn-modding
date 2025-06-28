using Timberborn.SingletonSystem;

namespace PrometheusExporter.Metrics
{
    class MetricsCollector : ILoadableSingleton
    {
        private readonly EventBus eventBus;

        public MetricsCollector(EventBus bus)
        {
            this.eventBus = bus;
        }

        public void Load()
        {
            // TODO: subscribe to sampling event
            // TODO: on sampling event, collect data
            // TODO: emit metrics-collectd event
        }
    }
}