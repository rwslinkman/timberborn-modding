using Timberborn.SingletonSystem;
using Timberborn.TickSystem;
using Timberborn.TimeSystem;
using UnityEngine;

namespace PrometheusExporter.Metrics
{
    class CollectorSamplingTrigger : ITickableSingleton, ILoadableSingleton
    {
        private readonly SampleTimeCalculator sampleTimeCalculator;
        private readonly IDayNightCycle dayNightCycle;
        private readonly EventBus eventBus;
        private float nextSampleTime;

        public CollectorSamplingTrigger(SampleTimeCalculator calculator, IDayNightCycle cycle, EventBus bus)
        {
            this.sampleTimeCalculator = calculator;
            this.dayNightCycle = cycle;
            this.eventBus = bus;
        }

        public void Load()
        {
            CalculateNextSampleTime();
        }

        public void Tick()
        {
            if (this.dayNightCycle.PartialDayNumber >= this.nextSampleTime)
            {
                EmitCollectMetricsEvent();
                CalculateNextSampleTime();
            }
        }

        private void CalculateNextSampleTime()
        {
            this.nextSampleTime = this.sampleTimeCalculator.CalculateNextSampleTime();
        }

        private void EmitCollectMetricsEvent()
        {
            Debug.Log("Emit event to collect metrics");
            this.eventBus.Post(new CollectMetricsCommand());
        }
    }
}