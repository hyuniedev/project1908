using Manager;
using UnityEngine;

namespace Model
{
    public class HumanModel
    {
        private GameObject _humanAIPrefab;
        private SkinnedMeshRenderer _skinnedMeshRenderer;
        public HumanModel(GameObject prefab)
        {
            _humanAIPrefab = Object.Instantiate(prefab, Vector3.zero, Quaternion.identity);
            _humanAIPrefab.AddComponent<BrainManager>();
            _skinnedMeshRenderer = _humanAIPrefab.GetComponentInChildren<SkinnedMeshRenderer>();
        }

        public void LoadSkinMesh()
        {
            if (_skinnedMeshRenderer == null)
            {
                _skinnedMeshRenderer = _humanAIPrefab.GetComponent<SkinnedMeshRenderer>();
            }

            if (_skinnedMeshRenderer != null)
            {
                Mesh mesh = _skinnedMeshRenderer.sharedMesh;
                int blendShapeCount = mesh.blendShapeCount;

                Debug.Log("Tổng số BlendShape: " + blendShapeCount);

                for (int i = 0; i < blendShapeCount; i++)
                {
                    string shapeName = mesh.GetBlendShapeName(i);
                    Debug.Log($"Index {i}: {shapeName}");
                }
            }
            else
            {
                Debug.LogError("Không tìm thấy SkinnedMeshRenderer!");
            }if (_skinnedMeshRenderer == null)
            {
                _skinnedMeshRenderer = _humanAIPrefab.GetComponent<SkinnedMeshRenderer>();
            }

            if (_skinnedMeshRenderer != null)
            {
                Mesh mesh = _skinnedMeshRenderer.sharedMesh;
                int blendShapeCount = mesh.blendShapeCount;

                Debug.Log("Tổng số BlendShape: " + blendShapeCount);

                for (int i = 0; i < blendShapeCount; i++)
                {
                    string shapeName = mesh.GetBlendShapeName(i);
                    Debug.Log($"Index {i}: {shapeName}");
                }
            }
            else
            {
                Debug.LogError("Không tìm thấy SkinnedMeshRenderer!");
            }
        }
    }
}