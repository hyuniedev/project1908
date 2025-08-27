using System;
using System.Collections;
using Model;
using UnityEngine;

namespace Manager
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private GameObject humanPrefab;
        private HumanModel _humanModel;
        
        private void Start()
        {
            StartCoroutine(Initialize());
        }

        private IEnumerator Initialize()
        {
            #if !UNITY_EDITOR
            while (!UnityEngine.Android.Permission.HasUserAuthorizedPermission(UnityEngine.Android.Permission
                       .Microphone))
            {
                yield return null;
            }
            #endif
            
            _humanModel ??= new HumanModel(humanPrefab);
            
            yield return null;
            
            _humanModel.LoadSkinMesh();
        }
    }
}