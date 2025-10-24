using System;
using System.Text;
using UnityEngine.Networking;

namespace AI
{
    public class StreamingDownloadHandle : DownloadHandlerScript
    {
        private Action<string> _callback;

        public StreamingDownloadHandle(Action<string> callback)
        {
            _callback = callback;
        }

        protected override bool ReceiveData(byte[] data, int dataLength)
        {
            if (data == null || dataLength <= 0) return false;
            string chuck = Encoding.UTF8.GetString(data, 0, dataLength);
            _callback.Invoke(chuck);
            return true;
        }
    }

}