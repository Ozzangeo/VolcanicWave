using UnityEngine.SceneManagement;

namespace Manager {
    public class SceneManager : BasicManager<SceneManager> {
        public override string Name => "Scene Manager";

        public static void LoadScene(string name) {
            FadeManager.FadeInOut(1.0f, () => UnityEngine.SceneManagement.SceneManager.LoadScene(name));
        }
    }
}