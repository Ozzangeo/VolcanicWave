using UnityEngine;

namespace Manager {
    public enum AudioType {
        BGM,
        SFX
    }

    [RequireComponent(typeof(AudioSource))]
    public class AudioManager : BasicManager<AudioManager> {
        public override string Name => "Audio Manager";

        [field: SerializeField] public AudioSource SourceBGM { get; private set; }
        [field: SerializeField] public AudioSource SourceSFX { get; private set; }

        [field: SerializeField] public Manager.GameData.AudioSettings Settings { get; private set; } = new();

        protected override void OnAwake() {
            SourceBGM = GetComponent<AudioSource>();
            SourceSFX = gameObject.AddComponent<AudioSource>();
        }

        public static void Play(AudioClip clip, AudioType type) {
            var instance = Instance;
            
            switch (type) {
                case AudioType.BGM:
                    instance.SourceBGM.clip = clip;
                    instance.SourceBGM.volume = instance.Settings.BgmVolume;

                    break;
                case AudioType.SFX:
                    instance.SourceBGM.PlayOneShot(clip, instance.Settings.SfxVolume);

                    break;
            }
        }
    }
}