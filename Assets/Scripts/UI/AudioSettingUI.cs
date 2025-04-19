using Manager;
using UnityEngine;

namespace UI {
    public class AudioSettingUI : MonoBehaviour {
        [SerializeField] private VolumeSlider _bgmVolumeSlider;
        [SerializeField] private VolumeSlider _sfxVolumeSlider;
        [SerializeField] private VolumeSlider _totalVolumeSlider;

        private void Awake() {
            var settings = AudioManager.Instance.Settings;

            _bgmVolumeSlider.SetVolume(settings.bgmVolume);
            _sfxVolumeSlider.SetVolume(settings.sfxVolume);
            _totalVolumeSlider.SetVolume(settings.totalVolume);

            _bgmVolumeSlider.Slider.onValueChanged.AddListener(o => AudioManager.Instance.Settings.bgmVolume = o);
            _sfxVolumeSlider.Slider.onValueChanged.AddListener(o => AudioManager.Instance.Settings.sfxVolume = o);
            _totalVolumeSlider.Slider.onValueChanged.AddListener(o => AudioManager.Instance.Settings.totalVolume = o);
        }
    }
} 
