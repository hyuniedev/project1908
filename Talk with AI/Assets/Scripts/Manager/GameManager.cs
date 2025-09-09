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
            _humanModel ??= new HumanModel(humanPrefab);
        }
    }
}