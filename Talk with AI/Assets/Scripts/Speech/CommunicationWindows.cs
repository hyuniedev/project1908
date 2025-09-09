using System;
using System.Threading.Tasks;
#if ENABLE_WINMD_SUPPORT && UNITY_WSA
using Windows.Media.SpeechRecognition;
using Windows.Media.SpeechSynthesis;
using Windows.Media.Playback;
using Windows.Media.Core;
#endif

namespace Speech
{
    public class CommunicationWindows : ICommunication
    {
#if ENABLE_WINMD_SUPPORT && UNITY_WSA
        private SpeechRecognizer recognizer;
        private SpeechSynthesizer tts;
        private MediaPlayer mediaPlayer;
#endif
        private Action<string> onNotificationCallback;
        private Action<string> sttCompletedCallback;
        private Action ttsCompletedCallback;

        public async void Init(
            string languageCode,
            string gameObjectName,
            Action<string> sttCompletedCallback,
            Action ttsCompletedCallback,
            Action<string> onNotificationCallback)
        {
            this.sttCompletedCallback = sttCompletedCallback;
            this.ttsCompletedCallback = ttsCompletedCallback;
            this.onNotificationCallback = onNotificationCallback;

#if ENABLE_WINMD_SUPPORT && UNITY_WSA
            recognizer = new SpeechRecognizer();
            tts = new SpeechSynthesizer();
            mediaPlayer = new MediaPlayer();

            // Compile constraints (để trống vẫn OK cho dictation mode)
            await recognizer.CompileConstraintsAsync();
#endif
        }

        public async void Speak(string text)
        {
#if ENABLE_WINMD_SUPPORT && UNITY_WSA
            var stream = await tts.SynthesizeTextToStreamAsync(text);
            mediaPlayer.Source = MediaSource.CreateFromStream(stream, stream.ContentType);

            mediaPlayer.MediaEnded += (s, e) =>
            {
                ttsCompletedCallback?.Invoke();
                onNotificationCallback?.Invoke("TTS completed.");
            };

            mediaPlayer.Play();
#else
            onNotificationCallback?.Invoke("TTS not supported on this platform.");
#endif
        }

        public void StopSpeaking()
        {
#if ENABLE_WINMD_SUPPORT && UNITY_WSA
            mediaPlayer.Pause();
            onNotificationCallback?.Invoke("TTS stopped.");
#endif
        }

        public async void StartListening()
        {
#if ENABLE_WINMD_SUPPORT && UNITY_WSA
            try
            {
                var result = await recognizer.RecognizeAsync();

                if (result.Status == SpeechRecognitionResultStatus.Success)
                {
                    sttCompletedCallback?.Invoke(result.Text);
                    onNotificationCallback?.Invoke("STT recognized: " + result.Text);
                }
                else
                {
                    onNotificationCallback?.Invoke("STT failed: " + result.Status);
                }
            }
            catch (Exception ex)
            {
                onNotificationCallback?.Invoke("Error recognizing: " + ex.Message);
            }
#endif
        }

        public void StopListening()
        {
            // Không cần cho RecognizeAsync vì nó tự dừng sau 1 lần nghe
            onNotificationCallback?.Invoke("StopListening called, but RecognizeAsync stops automatically.");
        }

        public void ShutDown()
        {
#if ENABLE_WINMD_SUPPORT && UNITY_WSA
            tts?.Dispose();
            recognizer?.Dispose();
            mediaPlayer?.Dispose();
#endif
        }
    }
}
