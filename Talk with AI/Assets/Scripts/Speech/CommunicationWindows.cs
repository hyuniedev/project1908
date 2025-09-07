using System;
using System.Globalization;
using System.Speech.Synthesis;
using System.Speech.Recognition;

namespace Speech
{
    public class CommunicationWindows : ICommunication
    {
        private SpeechSynthesizer tts;
        private SpeechRecognitionEngine recognizer;
        
        private Action<string> onNotificationCallback;
        
        public void Init(string languageCode, string gameObjectName, Action<string> sttCompletedCallback, Action ttsCompletedCallback,
            Action<string> onNotificationCallback)
        {
            this.onNotificationCallback = onNotificationCallback;
            
            tts = new SpeechSynthesizer();
            tts.SelectVoiceByHints(VoiceGender.Female);
            tts.SpeakCompleted += (sender, args) => { ttsCompletedCallback(); };
            
            recognizer = new SpeechRecognitionEngine(new CultureInfo(languageCode));
            recognizer.SetInputToDefaultAudioDevice();
            recognizer.LoadGrammar(new DictationGrammar());
            recognizer.RecognizeCompleted += (sender, args) => { onNotificationCallback(args.Result.Text); };
        }

        public void Speak(string text)
        {
            tts.SpeakAsync(text);
        }

        public void StopSpeaking()
        {
            tts.SpeakAsyncCancelAll();
        }

        public void StartListening()
        {
            recognizer.RecognizeAsync(RecognizeMode.Single);
        }

        public void StopListening()
        {
            recognizer.RecognizeAsyncStop();
        }

        public void ShutDown()
        {
            tts?.Dispose();
            recognizer?.Dispose();
        }
    }
}