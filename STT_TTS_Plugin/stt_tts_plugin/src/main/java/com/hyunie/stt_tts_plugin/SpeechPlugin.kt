package com.hyunie.stt_tts_plugin

import android.annotation.SuppressLint
import android.app.Activity
import android.content.Intent
import android.os.Bundle
import android.speech.RecognitionListener
import android.speech.RecognizerIntent
import android.speech.SpeechRecognizer
import android.speech.tts.TextToSpeech
import com.unity3d.player.UnityPlayer
import java.util.Locale

class SpeechPlugin {
    companion object {
        @SuppressLint("StaticFieldLeak")
        private var unityActivity: Activity? = null
        private var stt: SpeechRecognizer? = null
        private var tts: TextToSpeech? = null
        private var localCode: String = "vi-VN"
        private var notifyControllerGameObject: String = ""

        @JvmStatic
        fun init(activity: Activity, notifyGO: String, localCode: String? = null) {
            unityActivity = activity
            notifyControllerGameObject = notifyGO
            this.localCode = localCode ?: this.localCode

            activity.runOnUiThread {
                stt = SpeechRecognizer.createSpeechRecognizer(activity)

                tts = TextToSpeech(activity) { status ->
                    if (status == TextToSpeech.SUCCESS) {
                        val locale = Locale.forLanguageTag(this.localCode)
                        val result = tts?.setLanguage(locale)
                        if (result == TextToSpeech.LANG_MISSING_DATA || result == TextToSpeech.LANG_NOT_SUPPORTED) {
                            UnityPlayer.UnitySendMessage(
                                notifyControllerGameObject,
                                "OnGetNotification",
                                "Language not supported"
                            )
                        }
                    }
                }
            }
        }

        @JvmStatic
        fun startListening(gameObject: String, callback: String) {
            val activity = unityActivity ?: return

            val intent = Intent(RecognizerIntent.ACTION_RECOGNIZE_SPEECH).apply {
                putExtra(RecognizerIntent.EXTRA_LANGUAGE_MODEL, RecognizerIntent.LANGUAGE_MODEL_FREE_FORM)
                putExtra(RecognizerIntent.EXTRA_LANGUAGE, localCode)
            }

            activity.runOnUiThread {
                stt?.setRecognitionListener(object : RecognitionListener {
                    override fun onReadyForSpeech(p0: Bundle?) {
                        UnityPlayer.UnitySendMessage(notifyControllerGameObject, "OnGetNotification", "onReadyForSpeech")
                    }

                    override fun onBeginningOfSpeech() {
                        UnityPlayer.UnitySendMessage(notifyControllerGameObject, "OnGetNotification", "onBeginningOfSpeech")
                    }

                    override fun onRmsChanged(p0: Float) {
                        UnityPlayer.UnitySendMessage(notifyControllerGameObject, "OnGetNotification", "onRmsChanged")
                    }

                    override fun onBufferReceived(p0: ByteArray?) {
                        UnityPlayer.UnitySendMessage(notifyControllerGameObject, "OnGetNotification", "onBufferReceived")
                    }

                    override fun onEndOfSpeech() {
                        UnityPlayer.UnitySendMessage(notifyControllerGameObject, "OnGetNotification", "onEndOfSpeech")
                    }

                    override fun onError(error: Int) {
                        UnityPlayer.UnitySendMessage(notifyControllerGameObject, "OnGetNotification", "Error: $error")
                    }

                    override fun onResults(result: Bundle?) {
                        val matches = result?.getStringArrayList(SpeechRecognizer.RESULTS_RECOGNITION)
                        if (!matches.isNullOrEmpty()) {
                            val text = matches[0]
                            UnityPlayer.UnitySendMessage(gameObject, callback, text)
                            UnityPlayer.UnitySendMessage(
                                notifyControllerGameObject,
                                "OnGetNotification",
                                "onResults $text"
                            )
                        }
                    }

                    override fun onPartialResults(p0: Bundle?) {
                        UnityPlayer.UnitySendMessage(notifyControllerGameObject, "OnGetNotification", "onPartialResults")
                    }

                    override fun onEvent(p0: Int, p1: Bundle?) {
                        UnityPlayer.UnitySendMessage(notifyControllerGameObject, "OnGetNotification", "onEvent")
                    }
                })

                stt?.startListening(intent)
            }
        }

        @JvmStatic
        fun speak(text: String) {
            val activity = unityActivity ?: return
            activity.runOnUiThread {
                tts?.speak(text, TextToSpeech.QUEUE_FLUSH, null, null)
                UnityPlayer.UnitySendMessage(notifyControllerGameObject, "OnGetNotification", "speaking: $text")
            }
        }

        @JvmStatic
        fun stopSpeaking() {
            val activity = unityActivity ?: return
            activity.runOnUiThread {
                tts?.stop()
                UnityPlayer.UnitySendMessage(notifyControllerGameObject, "OnGetNotification", "Stop speak")
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
                unityActivity = null
                UnityPlayer.UnitySendMessage(notifyControllerGameObject, "OnGetNotification", "Shutdown")
            }
        }
    }
}
