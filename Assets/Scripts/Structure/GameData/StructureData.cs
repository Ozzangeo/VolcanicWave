namespace Structure.GameData {
    public enum StructureDirection {
        Right,
        Up,
        Left,
        Down,
        None = 10,
    }

    [System.Serializable]
    public class StructureData {
        public StructureDirection direction = StructureDirection.None;
    }
}