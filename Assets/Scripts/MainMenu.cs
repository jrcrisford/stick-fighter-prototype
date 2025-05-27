using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
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
