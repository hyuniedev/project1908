using UnityEngine;
using System.Collections;

namespace Speech
{
    public class SpeechConnection
    {
        public string languageCode = "vi-VN";
        private AndroidJavaClass pluginClass;
        private string _gameObjectName;
        public SpeechConnection(string gameObjectName, string sttCompletedCallback, string ttsCompletedCallback)
        { 
            _gameObjectName = gameObjectName;
            using (AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            {
                AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
                pluginClass = new AndroidJavaClass("com.hyunie.stt_tts_plugin.SpeechPlugin");
                pluginClass.CallStatic("init", currentActivity, gameObjectName, sttCompletedCallback, ttsCompletedCallback ,languageCode);
            }
        }
        
        #region TTS

        public void Speak(string text)
        {
            pluginClass.CallStatic("speak", text);
        }

        public void StopSpeaking()
        {
            pluginClass.CallStatic("stopSpeaking");
        }

        #endregion

        #region STT
        public void StartListening()
        {
            pluginClass.CallStatic("startListening", _gameObjectName);
        }

        #endregion
        
        public void ShutdownSpeech()
        {
            pluginClass.CallStatic("shutdown");
        }
    }
}