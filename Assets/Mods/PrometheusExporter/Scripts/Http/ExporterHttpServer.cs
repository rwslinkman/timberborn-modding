using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using PrometheusExporter.Http.Model;
using PrometheusExporter.Metrics;
using PrometheusExporter.Settings;
using Timberborn.SingletonSystem;
using UnityEngine;

namespace PrometheusExporter.Http
{
    class ExporterHttpServer : ILoadableSingleton
    {
        private readonly PrometheusExporterModSettings settings;
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
            var port = this.settings.HttpPort.Value;

            // Start http listener
            this.listener = new HttpListener();
            this.listener.Prefixes.Add("http://*:" + port + "/");
            this.listener.AuthenticationSchemes = AuthenticationSchemes.Anonymous;
            this.listener.Start();

            this.listenerThread = new Thread(StartListener);
            this.listenerThread.Start();
            Debug.Log("Prometheus HTTP Server Started");

            eventBus.Register(this);
        }

        [OnEvent]
        public void OnMetricsCollected(MetricsCollectedEvent metricsEvent)
        {
            this.metricsCollection.Increment(PrometheusMetrics.Counter(TimberbornMetrics.SampleCount));

            var updater = new PrometheusMetricUpdater();
            updater.UpdateMetricCollection(metricsEvent, this.metricsCollection);
        }

        private void StartListener()
        {
            while (true)
            {
                var result = this.listener.BeginGetContext(ListenerCallback, this.listener);
                result.AsyncWaitHandle.WaitOne();
            }
        }

        private void ListenerCallback(IAsyncResult result)
        {
            var context = this.listener.EndGetContext(result);
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