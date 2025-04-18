namespace Manager {
    public class GameManager : BasicManager<GameManager> {
        public override string Name => "Game Manager";
        public override bool IsIndestructible => false;

        private void GameOverLocal() {
            SceneLoader.LoadGameOverScene();
        }
        private void GameEndingLocal() {
            SceneLoader.LoadEndingScene();
        }

        public static void GameOver() => Instance.GameOverLocal();
        public static void GameEnding() => Instance.GameEndingLocal();
    }
}