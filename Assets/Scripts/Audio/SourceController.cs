using Manager;
using UnityEngine;

namespace Audio {
    [RequireComponent(typeof(AudioSource))]
    public class SourceController : MonoBehaviour {
        [SerializeField] private Manager.AudioType _audioType;
        [SerializeField] private AudioSource _source;

        private void Awake() {
            _source = GetComponent<AudioSource>();
        }

        private void Update() {
            _source.volume = TypeByVolume();
        }

        private float TypeByVolume() =>
            _audioType switch {
                Manager.AudioType.BGM => AudioManager.Instance.Settings.BgmVolume,
                Manager.AudioType.SFX => AudioManager.Instance.Settings.SfxVolume,
                _ => 0.0f,
            };
    }
}
