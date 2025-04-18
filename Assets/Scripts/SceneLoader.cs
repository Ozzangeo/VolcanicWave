using Manager;
using UnityEngine;

public class SceneLoader : MonoBehaviour {
    public static void LoadLobbyScene() => SceneManager.LoadScene("Lobby Scene");
    public static void LoadGameScene() => SceneManager.LoadScene("Game Scene");
    public static void LoadGameOverScene() => SceneManager.LoadScene("Game Over Scene");
    public static void LoadEndingScene() => SceneManager.LoadScene("Ending Scene");

    public static void GameQuit() => Application.Quit();
}