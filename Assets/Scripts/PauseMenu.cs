using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static PauseMenu Instance { get; private set; }

    public static bool gamePaused = false;
    public static bool gameOver = false;

    public GameObject pauseMenuUI;
    public GameObject gameOverUI;
    public GameObject gameWinUI;
    public TextMeshProUGUI score;
    public TextMeshProUGUI score2;
    public GameObject player;

    public void Start()
    {
        Time.timeScale = 1.0f;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !gameOver)
        {
            if (gamePaused)
            {
                GameResume();
            }
            else
            {
                GamePause();
            }
        }

        if(player == null && !gamePaused)
        {
            GameOver();
        }

    }

    public void GameResume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1.0f;
        gamePaused = false;
    }

    public void GamePause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        gamePaused = true;
    }

    public void GameOver()
    {
        gameOverUI.SetActive(true);
        Time.timeScale = 0f;
        score.text = GameManager.Instance.GetTotalScore().ToString();
        gameOver = true;
    }

    public void GameWin(float scoreNumber)
    {
        gameWinUI.SetActive(true);
        Time.timeScale = 0f;
        score2.text = scoreNumber.ToString();
        gameOver = true;
    }

    public void RestartLevel()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitGame()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("Main Menu");
    }
}
