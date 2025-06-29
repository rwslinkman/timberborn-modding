using System.Collections.Generic;
using System.Text;

namespace PrometheusExporter.Http
{
    public class PrometheusMetricsCollection
    {
        private readonly Dictionary<string, double> _gauges = new();

        public void SetGauge(string name, double value)
        {
            _gauges[name] = value;
        }

        public void IncrementGauge(string name, double delta = 1.0)
        {
            if (_gauges.TryGetValue(name, out var current))
            {
                _gauges[name] = current + delta;
            }
            else
            {
                _gauges[name] = delta;
            }
        }

        public void DecrementGauge(string name, double delta = 1.0)
        {
            IncrementGauge(name, -delta);
        }

        public string Collect()
        {
            var sb = new StringBuilder();
            foreach (var entry in _gauges)
            {
                string sanitized = Sanitize(entry.Key);
                sb.AppendLine($"# TYPE {sanitized} gauge");
                sb.Append(sanitized);
                sb.Append(' ');
                sb.AppendLine(entry.Value.ToString());
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