using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class VictoryUI : MonoBehaviour
{
    [SerializeField]
    private TMP_Text winnerText;

    [SerializeField]
    private TMP_Text waitingText;

    [SerializeField]
    private Button replayButton;

    [SerializeField]
    private Button mainMenuButton;

    private void Start()
    {
        winnerText.text =
            "GANADOR\n\n" +
            "Jugador " +
            GameResults.WinnerClientId +
            "\n\nPuntos: " +
            GameResults.WinnerScore;

        bool isHost =
            NetworkManager.Singleton != null &&
            NetworkManager.Singleton.IsHost;

        replayButton.gameObject.SetActive(isHost);
        mainMenuButton.gameObject.SetActive(isHost);

        waitingText.gameObject.SetActive(!isHost);
    }
}