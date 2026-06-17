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
        if (NetworkManager.Singleton == null)
        {
            SceneManager.LoadScene("Title");
            return;
        }

       
        if (NetworkManager.Singleton.IsHost)
        {
            var sceneManager = NetworkManager.Singleton.SceneManager;

            sceneManager.LoadScene("Title", LoadSceneMode.Single);

            sceneManager.OnLoadEventCompleted += OnReturnToMenuCompleted;
        }
    }

    private void OnReturnToMenuCompleted(
        string sceneName,
        LoadSceneMode loadSceneMode,
        System.Collections.Generic.List<ulong> clientsCompleted,
        System.Collections.Generic.List<ulong> clientsTimedOut)
    {
        if (NetworkManager.Singleton != null)
        {
            NetworkManager.Singleton.Shutdown();
        }

        
        NetworkManager.Singleton.SceneManager.OnLoadEventCompleted -= OnReturnToMenuCompleted;

        
        if (SceneManager.GetActiveScene().name != "Title")
        {
            SceneManager.LoadScene("Title");
        }
    }
}