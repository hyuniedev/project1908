using UnityEngine;
using System.Collections;

namespace Speech
{
    public class SpeechConnection : MonoBehaviour
    {
        public string languageCode = "vi-VN";
        private AndroidJavaClass pluginClass;

        public void Initialize()
        {
            using (AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            {
                AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
                pluginClass = new AndroidJavaClass("com.hyunie.stt_tts_plugin.SpeechPlugin");
                pluginClass.CallStatic("init", currentActivity, "GameManager", languageCode);
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

        public void StartListening(string gameobject, string callback)
        {
            if (!UnityEngine.Android.Permission.HasUserAuthorizedPermission(UnityEngine.Android.Permission.Microphone))
            {
                UnityEngine.Android.Permission.RequestUserPermission(UnityEngine.Android.Permission.Microphone);
                StartCoroutine(WaitForPermissionThenListen(gameobject,callback));
            }
            else
            {
                CallStartListening(gameobject, callback);
            }
        }

        private IEnumerator WaitForPermissionThenListen(string gameobject ,string callback)
        {
            while (!UnityEngine.Android.Permission.HasUserAuthorizedPermission(UnityEngine.Android.Permission.Microphone))
            {
                yield return null;
            }

            CallStartListening(gameobject, callback);
        }

        private void CallStartListening(string gameobject ,string callback)
        {
            pluginClass.CallStatic("startListening", gameobject, callback);
        }

        #endregion
        
        private void OnDestroy()
        {
            pluginClass.CallStatic("shutdown");
        }
    }
}