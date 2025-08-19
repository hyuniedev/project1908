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

@SuppressLint("StaticFieldLeak")
object SpeechPlugin {
    private var unityActivity : Activity? = null
    private var stt : SpeechRecognizer? = null
    private var tts : TextToSpeech? = null
    private var localCode : String = "vi-VN"
    fun init(activity: Activity, localCode: String? = null){
        unityActivity = activity
        stt = SpeechRecognizer.createSpeechRecognizer(activity)
        this.localCode = localCode?:this.localCode

        tts = TextToSpeech(activity){ status ->
            if(status == TextToSpeech.SUCCESS){
                val locale = Locale.forLanguageTag(this.localCode)
                tts?.language = locale
            }
        }
    }

    fun startListening(gameObject: String, callback: String){
        val activity = unityActivity?:return
        val intent = Intent(RecognizerIntent.ACTION_RECOGNIZE_SPEECH).apply {
            putExtra(RecognizerIntent.EXTRA_LANGUAGE_MODEL, RecognizerIntent.LANGUAGE_MODEL_FREE_FORM)
            putExtra(RecognizerIntent.EXTRA_LANGUAGE, localCode)
        }
        stt?.setRecognitionListener(object: RecognitionListener{
            override fun onReadyForSpeech(p0: Bundle?) {
            }

            override fun onBeginningOfSpeech() {
            }

            override fun onRmsChanged(p0: Float) {
            }

            override fun onBufferReceived(p0: ByteArray?) {
            }

            override fun onEndOfSpeech() {
            }

            override fun onError(error: Int) {
                UnityPlayer.UnitySendMessage(gameObject, callback, "Error: $error")
            }

            override fun onResults(result: Bundle?) {
                val matches = result?.getStringArrayList(SpeechRecognizer.RESULTS_RECOGNITION)
                if(!matches.isNullOrEmpty()){
                    val text = matches[0]
                    UnityPlayer.UnitySendMessage(gameObject, callback, text)
                }
            }

            override fun onPartialResults(p0: Bundle?) {
            }

            override fun onEvent(p0: Int, p1: Bundle?) {
            }

        })

        stt?.startListening(intent)
    }

    fun speak(text: String){
        tts?.speak(text, TextToSpeech.QUEUE_FLUSH, null, null)
    }

    fun stopSpeaking(){
        tts?.stop()
    }

    fun shutdown(){
        tts?.shutdown()
        stt?.destroy()
        tts = null
        stt = null
        unityActivity = null
    }
}