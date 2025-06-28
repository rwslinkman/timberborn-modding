using PrometheusExporter.Settings;
using Timberborn.SingletonSystem;
using UnityEngine;

namespace PrometheusExporter.Http
{
    class ExporterHttpServer : ILoadableSingleton
    {
        private PrometheusExporterModSettings settings;
        private readonly EventBus eventBus;


        public ExporterHttpServer(PrometheusExporterModSettings modSettings, EventBus bus)
        {
            this.settings = modSettings;
            this.eventBus = bus;
        }

        // TODO: subscribe to metrics-collected events
        public void Load()
        {
            Debug.Log("Start HTTP server for PromExporter");
        }
    }
}