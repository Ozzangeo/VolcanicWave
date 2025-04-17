using Resource;
using Structure.Interface;
using UnityEngine;

namespace Structure {
    public class BasicConsumer : StructureBehaviour, IResourceReceivable {
        [field: SerializeField] public int Resources { get; set; } = new();
        
        public bool IsResourceReceivable(BasicResource resource) {
            return true;
        }

        public void ReceiveResource(BasicResource resource, IStructure from) {
            Resources++;

            resource.Release();
        }
    }
}