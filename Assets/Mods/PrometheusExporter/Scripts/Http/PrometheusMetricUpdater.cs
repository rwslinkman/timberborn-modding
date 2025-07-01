using PrometheusExporter.Metrics;

namespace PrometheusExporter.Http
{
    class PrometheusMetricUpdater
    {
        public void updateMetricCollection(MetricsCollectedEvent data, PrometheusMetricsCollection metrics)
        {
            metrics.SetGauge(TimberbornPrometheusMetrics.SciencePoints, data.SciencePoints);
            metrics.SetGauge(TimberbornPrometheusMetrics.TotalBeaverCount, data.TotalBeaverCount);
            metrics.SetGauge(TimberbornPrometheusMetrics.AdultBeaverCount, data.AdultBeaverCount);
            metrics.SetGauge(TimberbornPrometheusMetrics.ChildBeaverCount, data.ChildBeaverCount);
            metrics.SetGauge(TimberbornPrometheusMetrics.TotalBotCount, data.TotalBotCount);
            foreach (var pair in data.GoodAmounts)
            {
                string goodName = pair.Key;
                int amount = pair.Value;
                int capacity = data.GoodCapacities[goodName];

                var metricNameGoodAmount = TimberbornPrometheusMetrics.GoodAmount + "_" + goodName.ToLower();
                metrics.SetGauge(metricNameGoodAmount, amount);

                var metricNameGoodCapacity = TimberbornPrometheusMetrics.GoodCapacity + "_" + goodName.ToLower();
                metrics.SetGauge(metricNameGoodCapacity, capacity);
            }
        }
    }
}