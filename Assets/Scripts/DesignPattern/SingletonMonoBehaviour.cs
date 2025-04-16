using UnityEngine;

namespace DesignPattern {
    public class SingletonMonoBehaivour<T> : MonoBehaviour where T : SingletonMonoBehaivour<T>, ISingletonable {
        public static T Instance => GetInstance();
        private static T _instance = null;

        protected virtual void OnAwake() { }
        protected void Awake() {
            if (IsAvailableInstance) {
                return;
            }

            _instance = (T)this;

            OnInstanceAvailable();

            OnAwake();
        }

        public static bool IsAvailableInstance => _instance != null;

        private static T GetInstance() {
            if (!IsAvailableInstance) {
                _instance = GameObject.FindAnyObjectByType<T>();

                if (!IsAvailableInstance) {
                    var singleton_mono_behaviour = new GameObject("");

                    _instance = singleton_mono_behaviour.AddComponent<T>();
                }

                OnInstanceAvailable();
            }

            return _instance;
        }
        private static void OnInstanceAvailable() {
            var before_name = _instance.name;
            _instance.name = $"[ Singleton ] {_instance.Name}";

            if (_instance.IsDebugBeforeName) {
                _instance.name += $" ( was called \"{before_name}\" )";
            }

            if (_instance.IsIndestructible) {
                DontDestroyOnLoad(_instance);
            }
        }
    }
}