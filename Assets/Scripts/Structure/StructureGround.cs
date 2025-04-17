using Define;
using Structure.GameData;
using Manager.Blueprint;
using Resource.GameData;
using UnityEngine;

namespace Structure {
    public class StructureGround : MonoBehaviour {
        [field: SerializeField] public GroundData Data { get; protected set; } = new();

        [field: SerializeField] public StructureBehaviour Structure { get; protected set; }
        [field: SerializeField] public StructureBlueprint Preview { get; protected set; } = new();
        [field: SerializeField] public StructureBlueprint Guide { get; protected set; } = new();

        public bool IsConstrutible => Data.isConstructible;
        public bool IsDestroyable => Data.isDestroyable;
        public bool HasStructure => Structure != null;

        private void Awake() {
            if (Physics.Raycast(transform.position, Vector3.up, out var hit, 1.0f, LayerMasks.StructureMask)) {
                if (hit.transform.TryGetComponent<StructureBehaviour>(out var structure)) {
                    Structure = structure;
                }
            }
        }

        [ContextMenu("Show Preview")]
        public void ShowPreview() {
            if (!IsConstrutible) {
                return;
            }

            if (Preview.HasBlueprint) {
                Preview.Cancel();
            }

            Preview.blueprint = Guide.Move();
            Preview.structure = Guide.structure;
            Preview.direction = Guide.direction;

            Preview.Preview(transform.position + Vector3.up, transform);
        }

        [ContextMenu("Show Guide")]
        public void ShowGuide() {
            if (!IsConstrutible) {
                return;
            }

            if (Preview.HasBlueprint) {
                Preview.UnPreview();
            }

            Guide.Preview(transform.position + Vector3.up, transform);
        }
        public void ShowGuide(StructureBehaviour structure, StructureDirection direction) {
            Guide.structure = structure;
            Guide.direction = direction;

            ShowGuide();
        }

        [ContextMenu("Hide Guide")]
        public void HideGuide() {
            if (Preview.HasBlueprint) {
                Preview.Preview(transform.position + Vector3.up, transform);
            }

            Guide.UnPreview();
        }

        [ContextMenu("Release Guide")]
        public void ReleaseGuide() {
            Guide.Cancel();
        }

        [ContextMenu("Confirm")]
        public bool Confirm() {
            if (HasStructure && !IsDestroyable) {
                Preview.Cancel();

                return false;
            }

            Destroy();

            Structure = Preview.Confirm();
            
            Structure.ConnectAround();
            Structure.Connect();

            return true;
        }

        [ContextMenu("Preview Connect Around")]
        public void PreviewConnectAround() {
            Preview.ConnectAround();
        }

        [ContextMenu("Cancel")]
        public void Cancel() {
            Preview.Cancel();
        }

        [ContextMenu("Destroy")]
        public void Destroy() {
            if (!IsDestroyable) {
                return;
            }

            if (HasStructure) {
                Structure.gameObject.layer = LayerIndexs.BlueprintLayer;
                Structure.ConnectLayer = LayerMasks.BlueprintMask;

                Structure.Release();

                Structure = null;
            }
        }
    }
}