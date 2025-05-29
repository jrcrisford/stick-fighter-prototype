using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Slider masterVolumeSlider;
    [SerializeField] private Slider musicVolumeSlider;

    private void Start()
    {
        float masterVol = 0.5f;
        float musicVol = 0.5f;

        if (masterVolumeSlider != null)
        {
            masterVolumeSlider.value = masterVol;
            float db = Mathf.Log10(masterVol) * 20f;
            MusicManager.Instance?.SetMasterVolume(db);
        }

        if (musicVolumeSlider != null)
        {
            musicVolumeSlider.value = musicVol;
            float db = Mathf.Log10(musicVol) * 20f;
            MusicManager.Instance?.SetMusicVolume(db);
        }
    }

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
