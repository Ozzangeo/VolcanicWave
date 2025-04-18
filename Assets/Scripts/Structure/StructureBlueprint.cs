using Define;
using Structure;
using Structure.GameData;
using UnityEngine;

namespace Manager.Blueprint {
    [System.Serializable]
    public class StructureBlueprint {
        public StructureBehaviour structure;
        public StructureDirection direction;

        public StructureBehaviour blueprint = null;

        public bool HasBlueprint => blueprint != null;

        private StructureBehaviour GetBlueprint(Transform parent) {
            var structure = GameObject.Instantiate(this.structure, parent);
            structure.gameObject.layer = LayerIndexs.BlueprintLayer;
            structure.ConnectLayer = LayerMasks.BlueprintMask;

            return structure;
        }

        public void Preview(Vector3 position, Transform parent) {
            if (structure == null) {
                return;
            }

            if (!HasBlueprint) {
                blueprint = GetBlueprint(parent);
            }

            blueprint.Direction = direction;
            blueprint.transform.position = position;

            // color change or alpha change

            blueprint.gameObject.SetActive(true);
        }
        public void UnPreview() {
            if (HasBlueprint) {
                blueprint.gameObject.SetActive(false);
            }
        }

        public void ConnectAround() {
            if (HasBlueprint) {
                blueprint.ConnectAround();
                blueprint.Connect();
            }
        }

        public StructureBehaviour Confirm() {
            var structure = blueprint;
            structure.gameObject.layer = LayerIndexs.StructureLayer;
            structure.ConnectLayer = LayerMasks.StructureMask;

            blueprint = null;

            return structure;
        }
        public StructureBehaviour Move() {
            var structure = blueprint;

            blueprint = null;

            return structure;
        }

        public void Cancel() {
            if (HasBlueprint) {
                blueprint.Release();

                blueprint = null;
            }
        }
    }
}