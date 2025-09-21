using Alchemy.Inspector;
using UnityEngine;

namespace Assets._00.Work.YHB.Scripts.Players
{
    public class RunnerVisualObject : MonoBehaviour
    {
        [TabGroup("Setting", "Render")]
        [field: SerializeField] public MeshFilter Filter { get; private set; }
        [TabGroup("Setting", "Render")]
        [field: SerializeField] public Material[] SharedMaterials { get; private set; }

        [TabGroup("Setting", "Physics")]
        [Tooltip("if this use collider excluding mesh collider, you have to set value.")]
        [field: SerializeField] public Collider UseCollider { get; private set; }
        public Vector3 groundCheckerSize = Vector3.one;

        public bool HaveCollider => UseCollider != null;

#if UNITY_EDITOR
        [Button]
        [ContextMenu("SetFilterAndCollider")]
        public void SetFilterAndCollider()
        {
            Filter = GetComponent<MeshFilter>();
            Debug.Assert(Filter != null, "filter is null");

            var collider = GetComponent<Collider>();
            if (collider is not MeshCollider)
                UseCollider = collider;
            else
                UseCollider = null;
            SetSharedMaterial();
        }

        private void OnValidate()
        {
            SetSharedMaterial();
        }

        private void SetSharedMaterial()
        {
            if (transform.TryGetComponent<MeshRenderer>(out MeshRenderer meshRenderer))
                SharedMaterials = meshRenderer.sharedMaterials;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(transform.position, groundCheckerSize);
        }
#endif
    }
}
