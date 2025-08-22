using AI;
using Speech;
using UnityEngine;
using UnityEngine.UI;

namespace Manager
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private Text resultTxt;

        [SerializeField] private Text debugTxt;
        
        private SpeechConnection speechController;
        private AIModel aiModel;
        void Start()
        {
            speechController = new SpeechConnection(gameObject.name, nameof(STTCompletedCallback), nameof(TTSCompletedCallback), nameof(OnGetNotification));
            aiModel = new AIModel();
            speechController.StartListening();
        }

        private void STTCompletedCallback(string result)
        {
            resultTxt.text = result;
            StartCoroutine(aiModel.Request(result, speechController.Speak));
        }

        private void TTSCompletedCallback(string _)
        {
            speechController.StartListening();
        }
        
        public void OnGetNotification(string notify)
        {
            debugTxt.text = notify;
        }

        private void OnDestroy()
        {
            speechController.ShutdownSpeech();
        }
    }
}
