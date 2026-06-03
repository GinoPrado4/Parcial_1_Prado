using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject playButton;

    private void Start()
    {
        playButton.SetActive(false);
    }

    public void StartHost()
    {
        NetworkManager.Singleton.StartHost();

        playButton.SetActive(true);
    }

    public void StartClient()
    {
        NetworkManager.Singleton.StartClient();

        playButton.SetActive(false);
    }

    public void StartGame()
    {
        if (!NetworkManager.Singleton.IsHost)
            return;

        NetworkManager.Singleton.SceneManager.LoadScene(
            "SampleScene",
            LoadSceneMode.Single
        );
    }
}