using System;
using UnityEngine;

namespace Config
{
    [Serializable]
    public class AddressAsset
    {
        public string speechConnection = "SpeechConnection";
    }
    
    [CreateAssetMenu(fileName = "AddressAssetSO", menuName = "Config/AddressAssetSO")]
    public class AddressAssetSO : ScriptableObject
    {
        public AddressAsset address;
    }
}