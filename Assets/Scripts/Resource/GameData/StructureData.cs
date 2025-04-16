namespace Resource.GameData {
    public enum StructureDirection {
        None,
        Up,
        Right,
        Down,
        Left,
    }

    [System.Serializable]
    public class StructureData {
        public StructureDirection direction = StructureDirection.None;
    }
}