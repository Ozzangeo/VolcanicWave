using Resource.Structure.Interface;
using UnityEngine;

namespace Resource.Structure {
    public class BasicConsumer : BasicStructure, IResourceReceivable {
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