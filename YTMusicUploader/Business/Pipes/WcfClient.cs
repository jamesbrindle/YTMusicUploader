using System.ServiceModel;

namespace YTMusicUploader.Business.Pipes
{
    /// <summary>
    /// WCF Named pipe client - To Send data to an existing YTMusicUploader process
    /// </summary>
    internal class WcfClient : ClientBase<IIpcClient>, IIpcClient
    {
        internal WcfClient(string pipeName) : base(
            new NetNamedPipeBinding(),
            new EndpointAddress(string.Format("net.pipe://localhost/{0}", pipeName)))
        { }

        /// <summary>
        /// Send data a string to the Wcf named pipe server
        /// </summary>
        public void Send(string data)
        {
            Channel.Send(data);
        }
    }
}