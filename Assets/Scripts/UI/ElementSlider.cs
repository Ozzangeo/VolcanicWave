using UnityEngine;
using UnityEngine.UI;

namespace UI {
    public class ElementSlider : MonoBehaviour {
        [SerializeField] private Slider _slider;
        [SerializeField] private Text _text;

        public void SetValue(float value, float max_value) {
            var progress = value / max_value;

            _slider.value = progress;
            _text.text = $"{value:###,###,###,##0}";
        }
    }
}