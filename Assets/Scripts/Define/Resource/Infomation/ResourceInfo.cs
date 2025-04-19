namespace Resource.Infomation {
    [System.Flags]
    public enum ResourceType {
        None    = 0b0000_0000,
        Water   = 0b0000_0001,
        Sun     = 0b0000_0010,
        Dirt    = 0b0000_0100,
    }

    [System.Serializable]
    public class ResourceInfo {
        public string name;
        public ResourceType type = ResourceType.None;

        public float weight = 10.0f;
    }
}