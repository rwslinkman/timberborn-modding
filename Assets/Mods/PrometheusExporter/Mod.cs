using Bindito.Core;
using PrometheusExporter.Metrics;
using PrometheusExporter.Settings;
using Timberborn.ModManagerScene;
using UnityEngine;

namespace PrometheusExporter
{
    [Context("MainMenu")]
    public class ModMainMenuConfigurator : IConfigurator
    {
        public void Configure(IContainerDefinition containerDefinition)
        {
            containerDefinition.Bind<PrometheusExporterModSettings>().AsSingleton();
        }
    }
    
    [Context("Game")]
    public class ModGameConfigurator : IConfigurator
    {
        public void Configure(IContainerDefinition containerDefinition)
        {
            // TODO: Tick handler that sends sampling events with EventBus
            containerDefinition.Bind<MetricsCollector>().AsSingleton();
            // TODO: http server singleton that handles /metrics requests & handle metrics-collectedEvent
        }
    }
}