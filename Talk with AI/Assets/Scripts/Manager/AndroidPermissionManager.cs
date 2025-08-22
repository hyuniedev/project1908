using UnityEngine;

namespace Manager
{
    /// <summary>
    /// Manages Android runtime permissions for Unity.
    /// This script checks if the user has granted the Microphone permission
    /// and requests it if it has not been granted yet. 
    /// Useful for apps that require audio recording functionality.
    /// </summary>
    public class AndroidPermissionManager : MonoBehaviour
    {
        private bool permissionRequested = false;

        void Start()
        {
            CheckMicrophonePermission();
        }

        void Update()
        {
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
        }

        void CheckMicrophonePermission()
        {
            if (!UnityEngine.Android.Permission.HasUserAuthorizedPermission(UnityEngine.Android.Permission.Microphone))
            {
                UnityEngine.Android.Permission.RequestUserPermission(UnityEngine.Android.Permission.Microphone);
                permissionRequested = true;
            }
            else
            {
                Debug.Log("Microphone permission already granted.");
            }
        }
    }

}
