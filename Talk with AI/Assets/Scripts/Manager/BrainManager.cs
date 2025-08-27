using System.Collections;
using AI;
using Speech;
using UnityEngine;
using UnityEngine.UI;

namespace Manager
{
    public class BrainManager : MonoBehaviour
    {
        private SpeechConnection _speechController;
        private AIConnection _aiConnection;

        void Start()
        {
            Initialize();
        }

        private void Initialize()
        {
#if !UNITY_EDITOR
            _speechController = new SpeechConnection(
                gameObjectName: gameObject.name, 
                sttCompletedCallback: nameof(STTCompletedCallback), 
                ttsCompletedCallback: nameof(TTSCompletedCallback), 
                onNotificationCallback: nameof(OnGetNotification));
            _aiConnection = new AIConnection();
#endif
        }
        
        private void STTCompletedCallback(string result)
        {
            StartCoroutine(_aiConnection.RequestHandle(result, _speechController.Speak));
        }

        private void TTSCompletedCallback(string result)
        {
            _speechController.StartListening();
        }
        
        public void OnGetNotification(string notify)
        {
        }

        private void OnDestroy()
        {
#if !UNITY_EDITOR
            _speechController.ShutdownSpeech();
#endif
        }
    }
}
