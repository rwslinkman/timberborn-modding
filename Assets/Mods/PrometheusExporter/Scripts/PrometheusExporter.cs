using Bindito.Core;
using PrometheusExporter.Http;
using PrometheusExporter.Metrics;
using PrometheusExporter.Settings;

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
            containerDefinition.Bind<PrometheusExporterModSettings>().AsSingleton();
            containerDefinition.Bind<SampleTimeCalculator>().AsSingleton();
            containerDefinition.Bind<CollectorSamplingTrigger>().AsSingleton();
            containerDefinition.Bind<ExporterHttpServer>().AsSingleton();
            containerDefinition.Bind<MetricsCollector>().AsSingleton();
        }
    }
}