using System;

namespace Speech
{
    public interface ICommunication
    {
        public void Init(string languageCode, string gameObjectName, Action<string> sttCompletedCallback, Action ttsCompletedCallback,
            Action<string> onNotificationCallback);

        public void Speak(string text);
        public void StopSpeaking();
        
        public void StartListening();
        public void StopListening();
        
        public void SetLanguage(string languageCode);
        
        public void ShutDown();
    }
}