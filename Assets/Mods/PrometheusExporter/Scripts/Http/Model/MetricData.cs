using System.Collections.Generic;
using System.Data.SqlTypes;

namespace PrometheusExporter.Http.Model
{
    public class MetricData
    {
        public string Name { get; internal set; }
        public PrometheusMetrics.MetricType Type { get; internal set; }
        public Dictionary<string, string> Labels { get; internal set; }
        public int Value { get; internal set; }

        public MetricData(string name, PrometheusMetrics.MetricType type)
        {
            Name = name;
            Type = type;
            Labels = new();
        }

        public MetricData(string name, PrometheusMetrics.MetricType type, int value) : this(name, type)
        {
            Value = value;
        }

        public MetricData WithLabel(string key, string value)
        {
            Labels[key] = value;
            return this;
        }

        public MetricData WithValue(int value)
        {
            this.Value = value;
            return this;
        }
    }
}
