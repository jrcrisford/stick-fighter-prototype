using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void Start()
    {
        Time.timeScale = 1.0f;
    }

    public void PlayGame ()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void Pirate()
    {
        SceneManager.LoadScene("Level 1");
    }

    public void TestStage()
    {
        SceneManager.LoadScene("TestingScene");
    }

    public void Quit()
    {
        Debug.Log("QUIT");
        Application.Quit();
    }

}
