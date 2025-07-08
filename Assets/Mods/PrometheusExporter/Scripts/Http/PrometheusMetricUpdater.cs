using PrometheusExporter.Metrics;
using PrometheusExporter.Http.Model;

namespace PrometheusExporter.Http
{
    class PrometheusMetricUpdater
    {
        public void UpdateMetricCollection(MetricsCollectedEvent data, PrometheusMetricsCollection metrics)
        {
            metrics.Set(
                PrometheusMetrics.Gauge(TimberbornMetrics.SciencePoints).WithValue(data.SciencePoints)
            );
            metrics.Set(
                PrometheusMetrics.Gauge(TimberbornMetrics.SciencePoints, data.SciencePoints)
            );
            metrics.Set(
                PrometheusMetrics.Gauge(TimberbornMetrics.TotalBeaverCount, data.TotalBeaverCount)
            );
            metrics.Set(
                PrometheusMetrics.Gauge(TimberbornMetrics.AdultBeaverCount, data.AdultBeaverCount)
            );
            metrics.Set(
                PrometheusMetrics.Gauge(TimberbornMetrics.ChildBeaverCount, data.ChildBeaverCount)
            );
            metrics.Set(
                PrometheusMetrics.Gauge(TimberbornMetrics.TotalBotCount, data.TotalBotCount)
            );
            foreach (var pair in data.GoodAmounts)
            {
                string goodName = pair.Key;
                int amount = pair.Value;
                int capacity = data.GoodCapacities[goodName];

                // Goods in stock
                var goodAmountMetric = PrometheusMetrics
                    .Gauge(TimberbornMetrics.GoodsStock)
                    .WithLabel("good", goodName)
                    .WithValue(amount);
                metrics.Set(goodAmountMetric);
                // Total goods capacity
                var goodCapacityMetric = PrometheusMetrics
                    .Gauge(TimberbornMetrics.GoodsCapacity)
                    .WithLabel("good", goodName)
                    .WithValue(capacity);
                metrics.Set(goodCapacityMetric);
            }
        }
    }
}