namespace PrometheusExporter.Http.Model
{
    public class PrometheusMetrics
    {
        // Create builder for new Gauge
        public static MetricData Gauge(string name) => new(name, MetricType.Gauge);
        public static MetricData Gauge(string name, int value) => new(name, MetricType.Gauge, value);

        // Create builder for new Counter
        public static MetricData Counter(string name) => new(name, MetricType.Counter);

        public enum MetricType { Gauge, Counter }
    }
}