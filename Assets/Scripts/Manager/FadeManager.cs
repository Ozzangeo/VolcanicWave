using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Manager {
    public class FadeManager : BasicManager<FadeManager> {
        public override string Name => "Fade Manager";

        [SerializeField] private CanvasGroup _fadeCanvas;
        private IEnumerator _fade;

        public static bool IsFading => Instance._fade != null;

        public CanvasGroup FadeCanvas {
            get {
                if (_fadeCanvas == null) {
                    var group = Resources.Load<CanvasGroup>("Fade Canvas");

                    _fadeCanvas = Instantiate(group);
                    _fadeCanvas.alpha = 0.0f;
                    _fadeCanvas.interactable = false;
                    _fadeCanvas.blocksRaycasts = false;
                }

                return _fadeCanvas;
            }
        }

        private IEnumerator FadeInCoroutine(float fade_time, Action on_fade_in_end = null) {
            FadeCanvas.interactable = true;
            FadeCanvas.blocksRaycasts = true;

            var time = fade_time * FadeCanvas.alpha;
            while (time < fade_time) {
                var progress = time / fade_time;

                FadeCanvas.alpha = progress;

                time += Time.deltaTime;

                yield return null;
            }

            FadeCanvas.alpha = 1.0f;

            on_fade_in_end?.Invoke();

            _fade = null;
        }
        private IEnumerator FadeOutCoroutine(float fade_time, Action on_fade_out_end = null) {
            var time = fade_time * FadeCanvas.alpha;
            while (time > 0.0f) {
                var progress = time / fade_time;

                FadeCanvas.alpha = progress;

                time -= Time.deltaTime;

                yield return null;
            }

            FadeCanvas.alpha = 0.0f;
            FadeCanvas.blocksRaycasts = false;
            FadeCanvas.interactable = false;
            
            on_fade_out_end?.Invoke();

            _fade = null;
        }
        private IEnumerator FadeInOutCoroutine(float fade_time, Action on_fade_in_end = null) {
            if (FadeCanvas.interactable) {
                on_fade_in_end?.Invoke();
            } else {
                yield return FadeInCoroutine(fade_time, on_fade_in_end);
            }

            yield return FadeOutCoroutine(fade_time);

            _fade = null;
        }

        private void FadeInLocal(float fade_time, Action on_fade_in_end = null) {
            if (_fade != null) {
                StopCoroutine(_fade);
            }
            
            StartCoroutine(_fade = FadeInCoroutine(fade_time, on_fade_in_end));
        }
        private void FadeOutLocal(float fade_time, Action on_fade_out_end = null) {
            if (_fade != null) {
                StopCoroutine(_fade);
            }

            StartCoroutine(_fade = FadeOutCoroutine(fade_time, on_fade_out_end));
        }
        private void FadeInOutLocal(float fade_time, Action on_fade_in_end = null) {
            if (_fade != null) {
                StopCoroutine(_fade);
            }

            StartCoroutine(_fade = FadeInOutCoroutine(fade_time, on_fade_in_end));
        }

        public static void FadeIn(float fade_time, Action on_fade_in_end = null) => Instance.FadeInLocal(fade_time, on_fade_in_end);
        public static void FadeOut(float fade_time, Action on_fade_out_end = null) => Instance.FadeOutLocal(fade_time, on_fade_out_end);
        public static void FadeInOut(float fade_time, Action on_fade_in_end = null) => Instance.FadeInOutLocal(fade_time, on_fade_in_end);
    }
}