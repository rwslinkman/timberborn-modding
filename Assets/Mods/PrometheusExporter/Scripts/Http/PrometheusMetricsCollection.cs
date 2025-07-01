using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Moq;
using PrometheusExporter.Http.Model;
using Timberborn.Common;

namespace PrometheusExporter.Http
{
    public class PrometheusMetricsCollection
    {
        private readonly Dictionary<(string, string), MetricData> _metrics = new();

        internal void Set(MetricData metric)
        {
            _metrics[MetricKey(metric)] = metric;
        }

        internal void Increment(MetricData metric)
        {
            var key = MetricKey(metric);
            var existing = _metrics.GetOrDefault(key);
            if (existing == null)
            {
                existing = metric;
            }
            var incremented = existing.Value + 1;
            _metrics[key] = existing.WithValue(incremented);
        }

        public string Collect()
        {
            var sb = new StringBuilder();
            var seenTypes = new HashSet<string>();

            foreach (var (key, metric) in _metrics)
            {
                var metricName = key.Item1;
                if (!seenTypes.Contains(metricName))
                {
                    var metricType = metric.Type.ToString().ToLower();
                    sb.AppendLine($"# TYPE {metricName} {metricType}");
                }

                var labelStr = BuildLabelString(metric.Labels);
                sb.Append(metricName).Append(labelStr).Append(' ')
                  .AppendLine(metric.Value.ToString());
            }

            return sb.ToString();
        }

        private string BuildLabelString(Dictionary<string, string> labels)
        {
            if (labels == null || labels.Count == 0)
                return string.Empty;

            var labelBody = labels
                .OrderBy(kv => kv.Key, StringComparer.Ordinal)
                .Select(i => Sanitize(i.Key).ToLower() + "=\"" + EscapeLabelValue(i.Value).ToLower() + "\"")
                .Aggregate((current, next) => current + "," + next);

            return "{" + labelBody + "}";
        }

        private static string EscapeLabelValue(string value)
        {
            return value.Replace(@"\", @"\\").Replace("\"", "\\\"");
        }

        private string Sanitize(string name)
        {
            // Replace illegal characters for Prometheus metric names
            return name.Replace(' ', '_').Replace("-", "_");
        }

        private string ToQueryLabelString(Dictionary<string, string> labels)
        {
            if (labels == null || labels.Count == 0)
                return string.Empty;

            return string.Join("&", labels
                .OrderBy(kv => kv.Key, StringComparer.Ordinal)
                .Select(kv => $"{Sanitize(kv.Key)}={EscapeLabelValue(kv.Value)}"));
        }

        private (string, string) MetricKey(MetricData metric)
        {
            return (Sanitize(metric.Name), ToQueryLabelString(metric.Labels));
        }
    }
}