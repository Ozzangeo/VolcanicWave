using Define;
using Ground.GameData;
using Manager.Blueprint;
using Resource.GameData;
using Structure;
using UnityEngine;

namespace Ground {
    public class BasicGround : MonoBehaviour {
        [field: SerializeField] public GroundData Data { get; protected set; } = new();

        [field: SerializeField] public BasicStructure Structure { get; protected set; }
        [field: SerializeField] public GroundBlueprint Blueprint { get; protected set; } = new();
        [field: SerializeField] public GroundBlueprint Guide { get; protected set; } = new();

        public bool IsConstrutible => Data.isConstructible;
        public bool HasStructure => Structure != null;

        private void Awake() {
            if (Physics.Raycast(transform.position, Vector3.up, out var hit, 1.0f, LayerMasks.StructureMask)) {
                if (hit.transform.TryGetComponent<BasicStructure>(out var structure)) {
                    Structure = structure;
                }
            }
        }

        public void Preview(BasicStructure structure, StructureDirection direction = StructureDirection.None) {
            if (!IsConstrutible) {
                return;
            }
            
            Blueprint.instance = structure;
            Blueprint.direction = direction;

            Blueprint.Preview(transform.position);
        }
        public void StartGuide(BasicStructure structure, StructureDirection direction) {
            if (!IsConstrutible) {
                return;
            }

            if (HasStructure) {
                Structure.gameObject.SetActive(false);
            }
            if (Blueprint.HasPreview) {
                Blueprint.UnPreview();
            }

            Guide.instance = structure;
            Guide.direction = direction;

            Guide.Preview(transform.position);
        }

        [ContextMenu("Start Guide")]
        public void StartGuide() {
            if (!IsConstrutible) {
                return;
            }

            if (HasStructure) {
                Structure.gameObject.SetActive(false);
            }
            if (Blueprint.HasPreview) {
                Blueprint.UnPreview();
            }

            Guide.Preview(transform.position);
        }
        [ContextMenu("Exit Guide")]
        public void ExitGuide() {
            if (Blueprint.HasPreview) {
                Blueprint.Preview(transform.position);
            }
            else if (HasStructure) {
                Structure.gameObject.SetActive(true);
            }

            Guide.UnPreview();
        }

        public void ReleaseGuide() {
            Guide.Cancel();
        }

        [ContextMenu("Preview")]
        private void Preview() {
            if (!IsConstrutible) {
                return;
            }

            Blueprint.Preview(transform.position);
        }

        [ContextMenu("Confirm")]
        public void Confirm() {
            Destroy();

            Structure = Blueprint.Confirm();

            Structure.ChainLink();
        }

        [ContextMenu("Cancel")]
        public void Cancel() {
            Blueprint.Cancel();

            if (HasStructure) {
                Structure.gameObject.SetActive(true);
            }
        }

        [ContextMenu("Destroy")]
        public void Destroy() {
            if (Structure != null) {
                Structure.Release();

                Structure = null;
            }
        }
    }
}