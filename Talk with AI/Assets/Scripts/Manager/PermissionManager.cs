using UnityEngine;

namespace Manager
{
    /// <summary>
    /// Manages Android runtime permissions for Unity.
    /// This script checks if the user has granted the Microphone permission
    /// and requests it if it has not been granted yet. 
    /// Useful for apps that require audio recording functionality.
    /// </summary>
    public class PermissionManager : MonoBehaviour
    {
        private bool permissionRequested = false;

        void Start()
        {
            CheckMicrophonePermission();
        }

        void Update()
        {
#if UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX || UNITY_STANDALONE_LINUX || UNITY_EDITOR
            if(Microphone.devices.Length > 0){
                gameObject.SetActive(false);
                permissionRequested = false;
            }
#elif UNITY_ANDROID
            if (permissionRequested)
            {
                if (UnityEngine.Android.Permission.HasUserAuthorizedPermission(UnityEngine.Android.Permission.Microphone))
                {
                    Debug.Log("User granted Microphone permission.");
                    permissionRequested = false;
                    gameObject.SetActive(false);
                }
                else
                {
                    Debug.LogWarning("User denied Microphone permission.");
                }
            }
#endif
        }

        void CheckMicrophonePermission()
        {
#if UNITY_ANDROID
            if (!UnityEngine.Android.Permission.HasUserAuthorizedPermission(UnityEngine.Android.Permission.Microphone))
            {
                UnityEngine.Android.Permission.RequestUserPermission(UnityEngine.Android.Permission.Microphone);
                permissionRequested = true;
            }
            else
            {
                Debug.Log("Microphone permission already granted.");
            }
#endif
        }
    }
}
