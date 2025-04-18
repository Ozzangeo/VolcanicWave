using Resource;

namespace Structure.Infomation.Structure {
    [System.Serializable]
    public class StorageResource {
        public ResourceBehaviour target;
        public int count;
        public int maxCount;

        public float Progress => count / maxCount;
        public bool IsInsertable => count < maxCount;
        public bool IsEmpty => count <= 0;

        public StorageResource Copy() =>
            new() {
                target = target,
                count = count,
                maxCount = maxCount
            };
    }
}