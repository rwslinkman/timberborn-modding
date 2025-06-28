using PrometheusExporter.Settings;
using Timberborn.TimeSystem;
using UnityEngine;

namespace PrometheusExporter.Metrics
{
    class SampleTimeCalculator
    {

        private readonly PrometheusExporterModSettings settings;
        private readonly IDayNightCycle dayNightCycle;

        public SampleTimeCalculator(
            PrometheusExporterModSettings modSettings,
            IDayNightCycle dayNightCycle
        )
        {
            this.settings = modSettings;
            this.dayNightCycle = dayNightCycle;
        }

        public float CalculateNextSampleTime()
        {
            var partialDayNumber = this.dayNightCycle.PartialDayNumber;
            var samplesPerDay = this.settings.SamplesPerDay.Value;
            var sampleInterval = 1f / samplesPerDay;
            var nextSample = Mathf.Ceil(partialDayNumber / sampleInterval) * sampleInterval;
            if (Mathf.Approximately(nextSample, partialDayNumber))
            {
                return nextSample + sampleInterval;
            }
            return nextSample;
        }
    }
}