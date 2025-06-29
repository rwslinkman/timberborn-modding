using System.Collections.Generic;
using System.Text;

namespace PrometheusExporter.Http
{
    public class PrometheusMetricsCollection
    {
        private readonly Dictionary<string, double> _gauges = new();
        private readonly Dictionary<string, double> _counters = new();

        // ==== GAUGE ====

        public void SetGauge(string name, double value)
        {
            _gauges[name] = value;
        }

        public void IncrementGauge(string name, double delta = 1.0)
        {
            _gauges.TryGetValue(name, out var current);
            _gauges[name] = current + delta;
        }

        public void DecrementGauge(string name, double delta = 1.0)
        {
            IncrementGauge(name, -delta);
        }

        // ==== COUNTER ====

        public void IncrementCounter(string name, double amount = 1.0)
        {
            _counters.TryGetValue(name, out var current);
            _counters[name] = current + amount;
        }

        // ==== EXPORT ====

        public string Collect()
        {
            var sb = new StringBuilder();

            foreach (var kv in _gauges)
            {
                string name = Sanitize(kv.Key);
                sb.AppendLine($"# TYPE {name} gauge");
                sb.Append(name).Append(" ").AppendLine(kv.Value.ToString());
            }

            foreach (var kv in _counters)
            {
                string name = Sanitize(kv.Key);
                sb.AppendLine($"# TYPE {name} counter");
                sb.Append(name).Append(" ").AppendLine(kv.Value.ToString());
            }
            return sb.ToString();
        }

        private string Sanitize(string name)
        {
            // Replace illegal characters for Prometheus metric names
            return name.Replace(' ', '_').Replace("-", "_");
        }
    }
}