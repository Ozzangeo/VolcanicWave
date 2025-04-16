using Resource;
using UnityEngine.Pool;

namespace Manager {
    public class ResourceManager : BasicManager<ResourceManager> {
        public IObjectPool<BasicResource> Pool { get; private set; }

        protected override void OnAwake() {
            Pool = new ObjectPool<BasicResource>(ResourceCreate, OnResourceTake, OnResourceReturn, OnResourceDestroy);
        }

        private BasicResource ResourceCreate() {
            return null;
        }

        private void OnResourceTake(BasicResource resource) {
            
        }

        private void OnResourceReturn(BasicResource resource) {
        
        }

        private void OnResourceDestroy(BasicResource resource) {
            Destroy(resource.gameObject);
        }
    }
}