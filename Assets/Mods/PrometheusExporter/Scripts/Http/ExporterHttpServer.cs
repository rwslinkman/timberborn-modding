using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using PrometheusExporter.Metrics;
using PrometheusExporter.Settings;
using Timberborn.SingletonSystem;
using UnityEngine;

namespace PrometheusExporter.Http
{
    class ExporterHttpServer : ILoadableSingleton
    {
        private PrometheusExporterModSettings settings;
        private readonly EventBus eventBus;
        private HttpListener listener;
        private Thread listenerThread;
        private PrometheusMetricsCollection metricsCollection;


        public ExporterHttpServer(PrometheusExporterModSettings modSettings, EventBus bus)
        {
            this.settings = modSettings;
            this.eventBus = bus;
            this.metricsCollection = new PrometheusMetricsCollection();
        }

        public void Load()
        {
            var port = settings.HttpPort.Value;

            // Start http listener
            listener = new HttpListener();
            listener.Prefixes.Add("http://localhost:" + port + "/");
            listener.Prefixes.Add("http://127.0.0.1:" + port + "/");
            listener.AuthenticationSchemes = AuthenticationSchemes.Anonymous;
            listener.Start();

            listenerThread = new Thread(startListener);
            listenerThread.Start();
            Debug.Log("Prometheus HTTP Server Started");

            eventBus.Register(this);
        }

        [OnEvent]
        public void OnMetricsCollected(MetricsCollectedEvent metricsEvent)
        {
            this.metricsCollection.IncrementGauge("sample_count", metricsEvent.sampleNumber);
        }

        private void startListener()
        {
            while (true)
            {
                var result = listener.BeginGetContext(ListenerCallback, listener);
                result.AsyncWaitHandle.WaitOne();
            }
        }

        private void ListenerCallback(IAsyncResult result)
        {
            var context = listener.EndGetContext(result);
            var request = context.Request;
            var response = context.Response;

            if (request.Url.AbsolutePath == "/metrics")
            {
                string body = this.metricsCollection.Collect();
                byte[] buffer = Encoding.UTF8.GetBytes(body);
                response.ContentType = "text/plain; version=0.0.4";
                response.ContentLength64 = buffer.Length;
                response.OutputStream.Write(buffer, 0, buffer.Length);
            }
            else
            {
                response.StatusCode = 404;
            }

            response.OutputStream.Close();
        }
    }
}