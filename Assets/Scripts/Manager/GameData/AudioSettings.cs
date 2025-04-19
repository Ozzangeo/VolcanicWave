namespace Manager.GameData {
    [System.Serializable]
    public class AudioSettings {
        public float bgmVolume = 1.0f;
        public float sfxVolume = 1.0f;
        public float totalVolume = 1.0f;

        public float BgmVolume => bgmVolume * totalVolume;
        public float SfxVolume => sfxVolume * totalVolume;
    }
}