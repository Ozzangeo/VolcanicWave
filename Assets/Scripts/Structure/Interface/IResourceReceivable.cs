using Resource;

namespace Structure.Interface {
    public interface IResourceReceivable {
        public bool IsResourceReceivable(ResourceBehaviour resource);
        public void ReceiveResource(ResourceBehaviour resource, IStructure from = null);
    }
}