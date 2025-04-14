using UnityEngine;

namespace DesignPattern {
    public class SingletonMonoBehaivour<T> : MonoBehaviour where T : MonoBehaviour, ISingletonable {
        public static T Instance => GetInstance();
        private static T _instance = null;

        public static bool IsAvailableInstance => _instance != null;

        private static T GetInstance() {
            if (!IsAvailableInstance) {
                _instance = GameObject.FindAnyObjectByType<T>();

                if (!IsAvailableInstance) {
                    var singleton_mono_behaviour = new GameObject("");

                    _instance = singleton_mono_behaviour.AddComponent<T>();
                }

                var before_name = _instance.name;
                _instance.name = $"[ Singleton ] {_instance.Name}";

                if (_instance.IsDebugBeforeName) {
                    _instance.name += $" ( was called \"{before_name}\" )";
                }

                if (_instance.IsIndestructible) {
                    DontDestroyOnLoad(_instance);
                }
            }

            return _instance;
        }
    }
}