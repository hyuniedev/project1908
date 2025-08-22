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
        [SerializeField] private Button microphoneChangeStateBtn;
        private SpeechConnection _speechController;
        private AIModel _aiModel;

        private bool _microphoneState = false;
        void Start()
        {
            StartCoroutine(Initialize());
            microphoneChangeStateBtn.onClick.AddListener(ChangeMicrophoneState);
        }

        private IEnumerator Initialize()
        {
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
            resultTxt.text = "Khởi tạo thành công";
            yield return null;
            _speechController.Speak("Xin chào, tôi có thể giúp gì cho bạn?");
        }

        private void ChangeMicrophoneState()
        {
            _microphoneState = !_microphoneState;
            if (_microphoneState)
            {
                microphoneChangeStateBtn.GetComponent<Image>().color = Color.green;
                _speechController.StopSpeaking();
                _speechController.StartListening();
            }
            else
            {
                microphoneChangeStateBtn.GetComponent<Image>().color = Color.grey;
                _speechController.StopListening();
            }
        }
        
        private void STTCompletedCallback(string result)
        {
            resultTxt.text = $"Bạn nói: {result}";
            StartCoroutine(_aiModel.RequestHandle(result, _speechController.Speak));
        }

        private void TTSCompletedCallback(string result)
        {
            
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
