using System;
using Config;
using Speech;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Manager
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField]
        private GameObject addressAsset;

        [SerializeField] private Text resultTxt;

        [SerializeField] private Text debugTxt;
        
        private SpeechConnection speechConnection;

        void Start()
        {
            Initialize();
            speechConnection.Initialize();
            speechConnection.Speak("Xin chào, vui lòng nói gì đó");
            speechConnection.StartListening(gameObject.name, nameof(SttResultCallback));
        }

        private void Initialize()
        {
            speechConnection = Instantiate(addressAsset).GetComponent<SpeechConnection>();;
            debugTxt.text = "Good";
        }

        private void SttResultCallback(string result)
        {
            resultTxt.text = result;
            speechConnection.Speak("Tôi nghe thấy: " + result);
            speechConnection.StartListening(gameObject.name, nameof(SttResultCallback));
        }
        
        public void OnGetNotification(string notify)
        {
            debugTxt.text = notify;
        }
    }
}
