using Timberborn.SingletonSystem;

namespace PrometheusExporter.Metrics
{
    class MetricsCollector : ILoadableSingleton
    {
        void ILoadableSingleton.Load()
        {
            // TODO: subscribe to sampling event
            // TODO: on sampling event, collect data
            // TODO: emit metrics-collectd event
        }
    }
}