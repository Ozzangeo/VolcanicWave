namespace Resource.Structure.Interface {
    public interface IResourceReceivable {
        public bool IsResourceReceivable(BasicResource resource);
        public void ReceiveResource(BasicResource resource, IStructure from = null);
    }
}