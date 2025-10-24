using System;
using System.Collections;
using System.Text;
using Config;
using Model;
using UnityEngine;
using UnityEngine.Networking;

namespace AI
{
    public class AIConnection
    {
        private readonly ApiConfig _apiKey = ConfigLoader.GetApiConfig();

        public IEnumerator RequestHandle(string message, Action<string> callback)
        {
            ChatRequest chatRequest = new ChatRequest
            {
                model = _apiKey.model,
                stream = true,
                messages = new[] { new Message { role = "user", content = message } },
                max_tokens = 512
            };
            string jsonString = JsonUtility.ToJson(chatRequest);
            byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonString);
            using UnityWebRequest uwr = new UnityWebRequest(_apiKey.url, "POST");

            string result = "";
            
            uwr.uploadHandler = new UploadHandlerRaw(bodyRaw);
            uwr.downloadHandler = new StreamingDownloadHandle((data) =>
            {
                if (data.StartsWith("data: "))
                {
                    string json = data.Substring(6).Trim();
                    if (json != "[DONE]")
                    {
                        var resp = JsonUtility.FromJson<ChunkResponse>(json);
                        result += resp.choices[0].delta.content;
                        int spaceIndex;
                        while ((spaceIndex = result.IndexOf(" ", StringComparison.Ordinal)) > 0)
                        {
                            var word = result.Substring(0, spaceIndex);
                            result = result.Substring(spaceIndex + 1);
                            callback.Invoke(word);
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(result))
                        {
                            callback.Invoke(result);
                            result = "";
                        }
                    }
                }
            });

            uwr.SetRequestHeader("Content-Type", "application/json");
            uwr.SetRequestHeader("Authorization", $"Bearer {_apiKey.apiKey}");
            
            yield return uwr.SendWebRequest();

            if (uwr.result != UnityWebRequest.Result.Success)
            {
                callback.Invoke("Lỗi rồi mày ơi! Lỗi: " + uwr.error);
                Debug.LogError($"Error: {uwr.error}");
            }
        }
    }
}