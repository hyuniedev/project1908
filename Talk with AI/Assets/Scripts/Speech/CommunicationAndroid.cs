using System;
using UnityEngine;
using System.Collections;

namespace Speech
{
    public class CommunicationAndroid  : ICommunication
    {
        protected AndroidJavaClass pluginClass;
        public void Init(string languageCode, string gameObjectName, Action<string> sttCompletedCallback, Action ttsCompletedCallback,
            Action<string> onNotificationCallback)
        { 
            using (AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            {
                AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
                pluginClass = new AndroidJavaClass("com.hyunie.stt_tts_plugin.SpeechPlugin");
                pluginClass.CallStatic(
                    "init", 
                    currentActivity, 
                    gameObjectName, 
                    nameof(sttCompletedCallback), 
                    nameof(ttsCompletedCallback), 
                    nameof(onNotificationCallback),
                    languageCode);
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
            pluginClass.CallStatic("startListening");
        }

        public void StopListening()
        {
            pluginClass.CallStatic("stopListening");
        }

        public void SetLanguage(string languageCode)
        {
            pluginClass.CallStatic("setLanguage","languageCode");
        }

        #endregion
        public void ShutDown()
        {
            pluginClass.CallStatic("shutdown");
        }
    }
}