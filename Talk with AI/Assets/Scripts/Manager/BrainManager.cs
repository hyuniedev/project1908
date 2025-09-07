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
        private ICommunication _communication;
        private AIModel _aiModel;

        void Start()
        {
            StartCoroutine(Initialize());
            stopSpeakingBtn.onClick.AddListener(() =>
            {
                _communication.StopSpeaking();
                _communication.StartListening();
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

            _communication = new CommunicationAndroid();
            _communication.Init(
                languageCode: "vi-VN",
                gameObjectName: gameObject.name, 
                sttCompletedCallback: STTCompletedCallback, 
                ttsCompletedCallback: TTSCompletedCallback, 
                onNotificationCallback: OnGetNotification);
            _aiModel = new AIModel();
        }
        
        private void STTCompletedCallback(string result)
        {
            resultTxt.text = $"Bạn nói: {result}";
            StartCoroutine(_aiModel.RequestHandle(result, _communication.Speak));
        }

        private void TTSCompletedCallback()
        {
            debugTxt.text = "Hãy nói gì đó!";
            _communication.StartListening();
        }
        
        public void OnGetNotification(string notify)
        {
            debugTxt.text = notify;
        }

        private void OnDestroy()
        {
            _communication.ShutDown();
        }
    }
}
