using System;
using System.ServiceModel;

namespace YTMusicUploader.Business.Pipes
{
    ///<summary>
    /// WCF Named pipe server - To receive data fro, an existing YTMusicUploader process
    /// </summary>
    internal sealed class WcfServer : IIpcServer
    {
        [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
        private class YTMusicWcfServer : IIpcClient
        {
            private readonly WcfServer server;

            public YTMusicWcfServer(WcfServer server)
            {
                this.server = server;
            }

            public void Send(string data)
            {
                server.OnReceived(new DataReceivedEventArgs(data));
            }
        }

        private readonly ServiceHost host;

        private void OnReceived(DataReceivedEventArgs e)
        {
            Received?.Invoke(this, e);
        }

        private void OnFaulted(object sender, EventArgs e)
        {
            Faulted?.Invoke(this, e);
        }

        ///<summary>
        /// WCF Named pipe server - To receive data fro, an existing YTMusicUploader process
        /// </summary>
        internal WcfServer(string pipeName)
        {
            host = new ServiceHost(new YTMusicWcfServer(this), new Uri(string.Format("net.pipe://localhost/{0}", pipeName)));
            host.IncrementManualFlowControlLimit(500);
            host.Faulted += OnFaulted;
        }

        public event EventHandler<DataReceivedEventArgs> Received;
        public event EventHandler<EventArgs> Faulted;

        /// <summary>
        ///  Start the Wcf named pipe server
        /// </summary>
        public void Start()
        {
            host.Open();
        }

        /// <summary>
        ///  Stop the Wcf named pipe server
        /// </summary>
        public void Stop()
        {
            host.Close();
        }

        void IDisposable.Dispose()
        {
            Stop();
            (host as IDisposable).Dispose();
        }
    }
}