namespace Resource.GameData {
    [System.Serializable]
    public class ResourceData {
        public float progress = 0.0f;
        public float tickRate = 0.0f;
        public bool isRest = false;

        public void Reset() {
            progress = 0.0f;
            isRest = false;
        }
    }
}