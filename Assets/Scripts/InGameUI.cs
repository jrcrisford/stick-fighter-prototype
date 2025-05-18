using TMPro;
using UnityEngine;

public class InGameUI : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI waveTimerText;
    [SerializeField] private TextMeshProUGUI waveNumberText;

    private void Update()
    {
        if (GameManager.Instance == null) return;

        // Update timer
        if (GameManager.Instance.IsWaveActive)
        {
            float currentTime = GameManager.Instance.CurrentWaveTime;
            waveTimerText.text = $"{currentTime:F2}s";
        }
        else
        {
            waveTimerText.text = $"0.00s";
        }

        // Update wave number
        waveNumberText.text = $"Wave {GameManager.Instance.CurrentWaveNumber + 1}";
    }
}
