using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Wave Scoring")]
    public float maxTimePerWave = 60f;
    public List<float> waveScores = new();

    private float waveStartTime;
    private bool waveActive = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        if (waveActive)
        {
            float currentTime = Time.time - waveStartTime;
            Debug.Log($"Wave in progress... Time: {currentTime:F2}s"); // Remove or replace with UI
        }
    }

    public void StartWave()
    {
        waveStartTime = Time.time;
        waveActive = true;
    }

    public void EndWave(int waveIndex)
    {
        float timeTaken = Time.time - waveStartTime;
        float score = Mathf.Max(0f, maxTimePerWave - timeTaken);
        waveScores.Add(score);
        waveActive = false;

        Debug.Log($"Wave {waveIndex + 1} completed in {timeTaken:F2}s. Score: {score:F1}");
    }

    public float GetTotalScore()
    {
        float total = 0f;
        foreach (float s in waveScores)
            total += s;
        return total;
    }

    public void PrintTotalScore()
    {
        Debug.Log($"Total Score: {GetTotalScore():F1}");
    }
}
