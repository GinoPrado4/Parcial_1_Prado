using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoryManager : MonoBehaviour
{
    public void Replay()
    {
        if (NetworkManager.Singleton != null &&
            NetworkManager.Singleton.IsHost)
        {
            NetworkManager.Singleton.SceneManager.LoadScene(
                "GameplayScene",
                LoadSceneMode.Single);
        }
    }

    public void MainMenu()
    {
        if (NetworkManager.Singleton != null)
        {
            NetworkManager.Singleton.Shutdown();
        }

        SceneManager.LoadScene(
            "Title");
    }
}
