using Resource.Infomation;

namespace Structure.Infomation {
    [System.Serializable]
    public class ConveryorBeltInfo {
        public ResourceType type = ResourceType.None;
        public float speed = 1.0f;
        public float maxWeight = 30.0f;
    }
}