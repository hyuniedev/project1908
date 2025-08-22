using System;
using UnityEngine;

namespace Config
{
    [Serializable]
    public class ApiConfig
    {
        public string url;
        public string apiKey;
        public string model;
    }

    public static class ConfigLoader
    {
        private static readonly string apiConfigName = "api_config";
        public static ApiConfig GetApiConfig()
        {
            var configFile = Resources.Load<TextAsset>(apiConfigName);
            if (configFile == null)
            {
                Debug.LogError("Config file not found: " + apiConfigName);
                return null;
            }

            ApiConfig apiConfig = JsonUtility.FromJson<ApiConfig>(configFile.text);
            return apiConfig;
        }
    }
}