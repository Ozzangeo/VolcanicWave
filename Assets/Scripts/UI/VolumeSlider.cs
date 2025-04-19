using UnityEngine;
using UnityEngine.UI;

namespace UI {
    public class VolumeSlider : MonoBehaviour {
        [field: SerializeField] public Slider Slider { get; private set; }
        [SerializeField] private Button _addButton;
        [SerializeField] private Button _subButton;

        [field: SerializeField] public float Progress { get; private set; }

        private void Awake() {
            Slider.onValueChanged.AddListener(OnValueChanged);
            _addButton.onClick.AddListener((() => {
                Progress = Mathf.Clamp(Progress + 0.1f, 0f, 1f);
                
                Slider.value = Progress;
            }));

            _subButton.onClick.AddListener((() => {
                Progress = Mathf.Clamp(Progress - 0.1f, 0f, 1f);

                Slider.value = Progress;
            }));
        }

        private void OnValueChanged(float value) {
            Progress = value;
        }

        public void SetVolume(float volume) { 
            Progress = volume;
            
            Slider.value = Progress;
        }
    }
}