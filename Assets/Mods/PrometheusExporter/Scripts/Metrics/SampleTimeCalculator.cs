using PrometheusExporter.Settings;
using Timberborn.TimeSystem;
using UnityEngine;

namespace PrometheusExporter.Metrics
{
    class SampleTimeCalculator
    {

        private readonly PrometheusExporterModSettings _goodStatisticsSettings;
        private readonly IDayNightCycle _dayNightCycle;

        public SampleTimeCalculator(
            PrometheusExporterModSettings modSettings,
            IDayNightCycle dayNightCycle
        )
        {
            _goodStatisticsSettings = modSettings;
            _dayNightCycle = dayNightCycle;
        }

        public float CalculateNextSampleTime()
        {
            var partialDayNumber = _dayNightCycle.PartialDayNumber;
            var samplesPerDay = _goodStatisticsSettings.SamplesPerDay.Value;
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