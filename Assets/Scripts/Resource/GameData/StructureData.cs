namespace Resource.GameData {
    public enum StructureDirection {
        None,
        Up,
        Down,
        Left,
        Right
    }

    [System.Serializable]
    public class StructureData {
        public StructureDirection direction = StructureDirection.None;
    }
}