using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof(AudioSource))]
public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance { get; private set; }

    [Header("Mixer Setup")]
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private string musicVolumeParam = "MusicVolume"; 
    [SerializeField] private string masterVolumeParam = "MasterVolume";

    private AudioSource musicSource;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        musicSource = GetComponent<AudioSource>();
        musicSource.loop = true;
        musicSource.playOnAwake = true;
        musicSource.Play();
    }
    public void SetMasterVolume(float db)
    {
        audioMixer.SetFloat(masterVolumeParam, db);
    }

    public void SetMusicVolume(float db)
    {
        audioMixer.SetFloat(musicVolumeParam, db);
    }

    public void ChangeTrack(AudioClip newTrack)
    {
        if (newTrack != null && musicSource.clip != newTrack)
        {
            musicSource.clip = newTrack;
            musicSource.Play();
        }
    }
}
