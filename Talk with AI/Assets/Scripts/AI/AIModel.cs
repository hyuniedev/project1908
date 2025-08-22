using System;
using System.Collections;
using System.Text;
using Config;
using UnityEngine;
using UnityEngine.Networking;

namespace AI
{
    public class AIModel
    {
        private readonly ApiConfig _apiKey = ConfigLoader.GetApiConfig();

        public IEnumerator RequestHandle(string message, Action<string> callback)
        {
            ChatRequest chatRequest = new ChatRequest
            {
                model = _apiKey.model,
                messages = new[] { new Message { role = "user", content = message } },
                max_tokens = 512
            };
            string jsonString = JsonUtility.ToJson(chatRequest);
            byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonString);
            using UnityWebRequest uwr = new UnityWebRequest(_apiKey.url, "POST");
            uwr.uploadHandler = new UploadHandlerRaw(bodyRaw);
            uwr.downloadHandler = new DownloadHandlerBuffer();
            uwr.SetRequestHeader("Content-Type", "application/json");
            uwr.SetRequestHeader("Authorization", $"Bearer {_apiKey.apiKey}");
            
            yield return uwr.SendWebRequest();

            if (uwr.result == UnityWebRequest.Result.Success)
            {
                ChatResponse response = JsonUtility.FromJson<ChatResponse>(uwr.downloadHandler.text);
                string answer = response.choices[0].message.content;
                callback.Invoke(answer);
            }
            else
            {
                callback.Invoke("Lỗi rồi mày ơi! Lỗi: " + uwr.error);
                Debug.LogError($"Error: {uwr.error}");
            }
        }
    }
    [Serializable]
    public class Message
    {
        public string role;
        public string content;
    }

    [Serializable]
    public class ChatRequest
    {
        public string model;
        public Message[] messages;
        public int max_tokens;
    }
    
    [Serializable]
    public class Choice
    {
        public int index;
        public Message message;
    }
    
    [Serializable]
    public class ChatResponse
    {
        public Choice[] choices;
    }
}