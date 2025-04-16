using Resource.GameData;
using Structure;
using UnityEngine;

namespace Manager.Blueprint {
    [System.Serializable]
    public class GroundBlueprint {
        public BasicStructure instance;
        public StructureDirection direction;

        public BasicStructure preview = null;

        public bool HasPreview => preview != null;

        public void Preview(Vector3 position) {
            if (instance == null) {
                return;
            }

            if (!HasPreview) {
                preview = GameObject.Instantiate(instance);
            }

            preview.gameObject.SetActive(true);
            preview.Direction = direction;

            preview.transform.position = position + Vector3.up;

            // color change or alpha change
        }
        public void UnPreview() {
            if (HasPreview) {
                preview.gameObject.SetActive(false);
            }
        }

        public BasicStructure Confirm() {
            // uncolor

            var structure = preview;

            preview = null;

            return structure;
        }
        public void Cancel() {
            if (HasPreview) {
                preview.Release();
            }
        }
    }
}