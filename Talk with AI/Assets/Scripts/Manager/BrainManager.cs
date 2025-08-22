using System.Collections;
using AI;
using Speech;
using UnityEngine;
using UnityEngine.UI;

namespace Manager
{
    public class BrainManager : MonoBehaviour
    {
        [SerializeField] private Text resultTxt;
        [SerializeField] private Text debugTxt;
        [SerializeField] private Button stopSpeakingBtn;
        private SpeechConnection _speechController;
        private AIModel _aiModel;

        void Start()
        {
            StartCoroutine(Initialize());
            stopSpeakingBtn.onClick.AddListener(() =>
            {
                _speechController.StopSpeaking();
                _speechController.StartListening();
            });
        }

        private IEnumerator Initialize()
        {
            resultTxt.text = "Hãy nói gì đó...";
            while (!UnityEngine.Android.Permission.HasUserAuthorizedPermission(UnityEngine.Android.Permission
                       .Microphone))
            {
                yield return null;
            }
            _speechController = new SpeechConnection(
                gameObjectName: gameObject.name, 
                sttCompletedCallback: nameof(STTCompletedCallback), 
                ttsCompletedCallback: nameof(TTSCompletedCallback), 
                onNotificationCallback: nameof(OnGetNotification));
            _aiModel = new AIModel();
        }
        
        private void STTCompletedCallback(string result)
        {
            resultTxt.text = $"Bạn nói: {result}";
            StartCoroutine(_aiModel.RequestHandle(result, _speechController.Speak));
        }

        private void TTSCompletedCallback(string result)
        {
            debugTxt.text = "Hãy nói gì đó!";
            _speechController.StartListening();
        }
        
        public void OnGetNotification(string notify)
        {
            debugTxt.text = notify;
        }

        private void OnDestroy()
        {
            _speechController.ShutdownSpeech();
        }
    }
}
