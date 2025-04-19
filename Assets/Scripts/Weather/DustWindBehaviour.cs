using Manager;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Weather.Object;

namespace Weather {
    public class DustWindBehaviour : MonoBehaviour {
        [field: SerializeField] public DustWindInfo Info { get; private set; }
        [SerializeField] private AudioClip _sfxClip;

        [field: SerializeField] public Text RemainTimeText { get; private set; }

        [SerializeField] private DateTime _endTime;
        [SerializeField] private DateTime _fadeTime;
        [SerializeField] private int _index = 0;

        private void Start() {
            _endTime = DateTime.Now + Info.RemainSpans[_index];
            _fadeTime = _endTime - Info.FadeSpan;
        }

        private void Update() {
            var timer = _endTime - DateTime.Now;

            RemainTimeText.text = $"[{_index + 1}/{Info.RemainTimes.Length}] {timer:mm\\:ss}";

            if (_index >= Info.RemainTimes.Length) {
                return;
            }

            if (DateTime.Now >= _fadeTime && !FadeManager.IsFading) {
                FadeManager.FadeIn(Info.FadeTime);
            }

            if (DateTime.Now >= _endTime) {
                DustWind();


                if (_index + 1 >= Info.RemainTimes.Length) {
                    GameManager.GameOver();

                    return;
                }
                
                ++_index;

                var spans = Info.RemainSpans;
                _endTime += spans[_index];
                _fadeTime += spans[_index];

                FadeManager.FadeOut(Info.FadeTime);
            }
        }

        private void DustWind() {
            Debug.Log("Dust Wind");

            AudioManager.Play(_sfxClip, Manager.AudioType.SFX);

            var grounds = GroundManager.Instance.Grounds;

            var percentage = Info.GroundRemoveRates[_index] * 0.01f;
            var count = (int)(grounds.Count * percentage);
            for (int i = 0; i < count && grounds.Count > 0; ++i) {
                var random_index = UnityEngine.Random.Range(0, grounds.Count);

                var ground = grounds[random_index];

                if (ground.IsRequired) {
                    continue;
                }

                grounds.Remove(ground);
                
                ground.Release();
            }
        }
    }
}