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
            speechController = new SpeechConnection();
            aiModel = new AIModel();
            
            speechController.StartListening(gameObject.name, nameof(ListenCompletedCallback));
        }

        private void ListenCompletedCallback(string result)
        {
            resultTxt.text = result;
            StartCoroutine(aiModel.Request(result, speechController.Speak));
            speechController.StartListening(gameObject.name, nameof(ListenCompletedCallback));
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
