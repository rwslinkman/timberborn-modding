using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
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


        public ExporterHttpServer(PrometheusExporterModSettings modSettings, EventBus bus)
        {
            this.settings = modSettings;
            this.eventBus = bus;
        }

        // TODO: subscribe to metrics-collected events
        public void Load()
        {
            var port = settings.HttpPort.Value;
            Debug.Log("Start HTTP server for PromExporter");
            listener = new HttpListener();
            listener.Prefixes.Add("http://localhost:"+ port +"/");
            listener.Prefixes.Add("http://127.0.0.1:" + port + "/");
            listener.AuthenticationSchemes = AuthenticationSchemes.Anonymous;
            listener.Start();

            listenerThread = new Thread(startListener);
            listenerThread.Start();
            Debug.Log("Prometheus HTTP Server Started");
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

            Debug.Log("Method: " + request.HttpMethod);
            Debug.Log("LocalUrl: " + request.Url.LocalPath);

            if (request.Url.AbsolutePath == "/metrics")
            {
                string body = "# HELP sample_metric Example metric\nsample_metric 1\n";
                // TODO: export real metrics
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