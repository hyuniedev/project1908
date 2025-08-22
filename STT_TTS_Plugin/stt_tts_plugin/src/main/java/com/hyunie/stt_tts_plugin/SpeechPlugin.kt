package com.hyunie.stt_tts_plugin

import android.annotation.SuppressLint
import android.app.Activity
import android.content.Intent
import android.os.Bundle
import android.speech.RecognitionListener
import android.speech.RecognizerIntent
import android.speech.SpeechRecognizer
import android.speech.tts.TextToSpeech
import android.speech.tts.UtteranceProgressListener
import com.unity3d.player.UnityPlayer
import java.util.Locale

class SpeechPlugin {
    companion object {
        @SuppressLint("StaticFieldLeak")
        private var unityActivity: Activity? = null
        private var stt: SpeechRecognizer? = null
        private var tts: TextToSpeech? = null
        private var isTtsReady: Boolean = false
        private var localCode: String = "vi-VN"
        private var currentGameObject: String = ""

        @JvmStatic
        fun init(activity: Activity, currentGameObject: String, sttCompletedCallback: String, ttsCompletedCallback: String, onGetNotificationCallback: String, localCode: String? = null) {
            unityActivity = activity
            this.localCode = localCode ?: this.localCode
            this.currentGameObject = currentGameObject

            activity.runOnUiThread {
                stt = SpeechRecognizer.createSpeechRecognizer(activity)
                
                stt?.setRecognitionListener(object : RecognitionListener {
                    override fun onReadyForSpeech(p0: Bundle?) {
                        UnityPlayer.UnitySendMessage(currentGameObject, onGetNotificationCallback, "onReadyForSpeech")
                    }

                    override fun onBeginningOfSpeech() {
                        UnityPlayer.UnitySendMessage(currentGameObject, onGetNotificationCallback, "onBeginningOfSpeech")
                    }

                    override fun onRmsChanged(p0: Float) {
                        UnityPlayer.UnitySendMessage(currentGameObject, onGetNotificationCallback, "onRmsChanged")
                    }

                    override fun onBufferReceived(p0: ByteArray?) {
                        UnityPlayer.UnitySendMessage(currentGameObject, onGetNotificationCallback, "onBufferReceived")
                    }

                    override fun onEndOfSpeech() {
                        UnityPlayer.UnitySendMessage(currentGameObject, onGetNotificationCallback, "onEndOfSpeech")
                    }

                    override fun onError(error: Int) {
                        UnityPlayer.UnitySendMessage(currentGameObject, onGetNotificationCallback, "Error: $error")
                    }

                    override fun onResults(result: Bundle?) {
                        val matches = result?.getStringArrayList(SpeechRecognizer.RESULTS_RECOGNITION)
                        if (!matches.isNullOrEmpty()) {
                            val text = matches[0]
                            UnityPlayer.UnitySendMessage(currentGameObject, sttCompletedCallback, text)
                            UnityPlayer.UnitySendMessage(
                                currentGameObject,
                                onGetNotificationCallback,
                                "onResults $text"
                            )
                        }
                    }

                    override fun onPartialResults(p0: Bundle?) {
                        UnityPlayer.UnitySendMessage(currentGameObject, onGetNotificationCallback, "onPartialResults")
                    }

                    override fun onEvent(p0: Int, p1: Bundle?) {
                        UnityPlayer.UnitySendMessage(currentGameObject, onGetNotificationCallback, "onEvent")
                    }
                })

                tts = TextToSpeech(activity) { status ->
                    if (status == TextToSpeech.SUCCESS) {
                        val locale = Locale.forLanguageTag(this.localCode)
                        val result = tts?.setLanguage(locale)
                        if (result == TextToSpeech.LANG_MISSING_DATA || result == TextToSpeech.LANG_NOT_SUPPORTED) {
                            UnityPlayer.UnitySendMessage(
                                currentGameObject,
                                onGetNotificationCallback,
                                "Language not supported"
                            )
                        } else {
                            // Configure TTS defaults for responsiveness
                            tts?.setSpeechRate(1.0f)
                            tts?.setPitch(1.5f)
                            isTtsReady = true
                            UnityPlayer.UnitySendMessage(
                                currentGameObject,
                                onGetNotificationCallback,
                                "TTS_Ready"
                            )
                        }
                    }
                }

                tts?.setOnUtteranceProgressListener(object : UtteranceProgressListener() {
                            override fun onStart(utteranceId: String?) {
                                UnityPlayer.UnitySendMessage(
                                    currentGameObject,
                                    onGetNotificationCallback,
                                    "TTS_Started: $utteranceId"
                                )
                            }

                            override fun onDone(utteranceId: String?) {
                                UnityPlayer.UnitySendMessage(currentGameObject, ttsCompletedCallback, utteranceId)
                                UnityPlayer.UnitySendMessage(
                                    currentGameObject,
                                    onGetNotificationCallback,
                                    "TTS_Completed: $utteranceId"
                                )
                            }

                            @Deprecated("Deprecated in Java")
                            override fun onError(utteranceId: String?) {
                                UnityPlayer.UnitySendMessage(
                                    currentGameObject,
                                    onGetNotificationCallback,
                                    "TTS_Error: $utteranceId"
                                )
                            }
                })
            }
        }

        @JvmStatic
        fun startListening() {
            val activity = unityActivity ?: return

            val intent = Intent(RecognizerIntent.ACTION_RECOGNIZE_SPEECH).apply {
                putExtra(RecognizerIntent.EXTRA_LANGUAGE_MODEL, RecognizerIntent.LANGUAGE_MODEL_FREE_FORM)
                putExtra(RecognizerIntent.EXTRA_LANGUAGE, localCode)
                // Make STT more responsive
                putExtra(RecognizerIntent.EXTRA_PARTIAL_RESULTS, true)
                putExtra(RecognizerIntent.EXTRA_MAX_RESULTS, 1)
                putExtra(
                    RecognizerIntent.EXTRA_SPEECH_INPUT_COMPLETE_SILENCE_LENGTH_MILLIS,
                    600
                )
                putExtra(
                    RecognizerIntent.EXTRA_SPEECH_INPUT_POSSIBLY_COMPLETE_SILENCE_LENGTH_MILLIS,
                    600
                )
                putExtra(
                    RecognizerIntent.EXTRA_SPEECH_INPUT_MINIMUM_LENGTH_MILLIS,
                    800
                )
            }

            activity.runOnUiThread {
                stt?.startListening(intent)
            }
        }

        @JvmStatic
        fun stopListening() {
            unityActivity?.runOnUiThread {
                stt?.stopListening()
            }
        }

        @JvmStatic
        fun speak(text: String) {
            val activity = unityActivity ?: return
            activity.runOnUiThread {
                if (!isTtsReady) return@runOnUiThread
                val utteranceId = "utterance_${System.currentTimeMillis()}"
                val params = Bundle().apply {
                    putString(TextToSpeech.Engine.KEY_PARAM_UTTERANCE_ID, utteranceId)
                }
                tts?.speak(text, TextToSpeech.QUEUE_FLUSH, params, utteranceId)
            }
        }

        @JvmStatic
        fun stopSpeaking() {
            val activity = unityActivity ?: return
            activity.runOnUiThread {
                tts?.stop()
            }
        }

        @JvmStatic
        fun shutdown() {
            val activity = unityActivity ?: return
            activity.runOnUiThread {
                tts?.shutdown()
                stt?.destroy()
                tts = null
                stt = null
                isTtsReady = false
                unityActivity = null
            }
        }
    }
}
