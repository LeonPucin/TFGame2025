using TriInspector;
using UnityEngine;

namespace DoubleDCore.Optimization
{
    [RequireComponent(typeof(MeshRenderer))]
    public class GpuInstancingEnabler : MonoBehaviour
    {
        [ReadOnly, SerializeField] private MeshRenderer _meshRenderer;

        private void Awake() =>
            _meshRenderer.SetPropertyBlock(new MaterialPropertyBlock());

        private void OnValidate()
        {
            _meshRenderer = GetComponent<MeshRenderer>();
        }
    }
}