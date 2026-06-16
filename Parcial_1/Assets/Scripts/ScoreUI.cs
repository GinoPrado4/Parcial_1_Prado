using TMPro;
using UnityEngine;

public class ScoreUI : MonoBehaviour
{
    [SerializeField]
    private TMP_Text scoreText;

    private void Update()
    {
        if (GameManager.Instance == null)
            return;

        string text = "PUNTAJES\n\n";

        foreach (var player in GameManager.Instance.PlayerScores)
        {
            text +=
                $"Jugador {player.ClientId}: {player.Score}\n";
        }

        scoreText.text = text;
    }
}