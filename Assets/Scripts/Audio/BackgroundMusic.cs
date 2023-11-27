using UnityEngine;
using UnityEngine.SceneManagement;

public class BackgroundMusic : MonoBehaviour
{
    public AudioClip backgroundMusicClip; // Drag and drop your audio clip here in the Unity Editor
    private AudioSource audioSource;
    public float volumeLoweringFactor = 0.5f;

    void Start()
    {
        // Create an AudioSource component if not already attached
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Set the audio clip to play
        audioSource.clip = backgroundMusicClip;

        // Play the background music
        PlayBackgroundMusic();
    }

    void PlayBackgroundMusic()
    {
        // Check if the audio clip is set and not already playing
        if (backgroundMusicClip != null && !audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }

    void Update()
    {
        // Check if the scene is restarted
        if (Input.GetKeyDown(KeyCode.R))
        {
            // Stop the current background music
            audioSource.Stop();

            // Play the background music again
            PlayBackgroundMusic();
        }
    }

    // You can also use this method to play the background music when the scene is reloaded
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Play the background music when the scene is loaded
        PlayBackgroundMusic();
    }

    public void LowerVolume()
    {
        audioSource.volume *= volumeLoweringFactor;
    }
}
