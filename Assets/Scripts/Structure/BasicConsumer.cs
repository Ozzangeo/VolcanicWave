using Resource;
using Structure.Interface;
using UnityEngine;

namespace Structure {
    public class BasicConsumer : StructureBehaviour, IResourceReceivable {
        [field: SerializeField] public int Resources { get; set; } = new();
        
        public bool IsResourceReceivable(ResourceBehaviour resource) {
            return true;
        }

        public void ReceiveResource(ResourceBehaviour resource, IStructure from) {
            Resources++;

            resource.Release();
        }
    }
}