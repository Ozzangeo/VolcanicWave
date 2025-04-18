using UnityEngine;

namespace Structure {
    public class NullStructure : StructureBehaviour {
        public override void OnConfirm() {
            Destroy(gameObject);    
        }
    }
}