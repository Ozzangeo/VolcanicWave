namespace Resource.GameData {
    public enum StructureDirection {
        Right,
        Up,
        Left,
        Down,
        None,
    }

    [System.Serializable]
    public class StructureData {
        public StructureDirection direction = StructureDirection.None;
    }
}