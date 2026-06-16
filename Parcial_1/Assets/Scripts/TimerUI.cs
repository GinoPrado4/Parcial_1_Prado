using TMPro;
using UnityEngine;

public class TimerUI : MonoBehaviour
{
    [SerializeField]
    private TMP_Text timerText;

    private void Update()
    {
        if (GameManager.Instance == null)
            return;

        float time = GameManager.Instance.GetRemainingTime();

        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);

        timerText.text =
            $"{minutes:00}:{seconds:00}";
    }
}
