using Bindito.Core;
using PrometheusExporter.Metrics;
using PrometheusExporter.Settings;
using Timberborn.ModManagerScene;

namespace PrometheusExporter
{
    public class ModStarter : IModStarter
    {
        public void StartMod(IModEnvironment modEnvironment)
        {
            var playerLogPath = Application.persistentDataPath + "/Player.log";
            Debug.Log("Hello World, but in the Player.log file at: " + playerLogPath);
        }

        void IModStarter.StartMod(IModEnvironment modEnvironment)
        {
            new Harmony(nameof(PrometheusExporter)).PatchAll();
        }
    }

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