using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Audio Mixer")]
    [SerializeField] private AudioMixer audioMixer;

    [Header("Audio Sources")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;

    private const string MusicVolumeKey = "musicVolume";
    private const string SfxVolumeKey = "sfxVolume";

    // These must match the exposed Audio Mixer parameter names.
    private const string MusicMixerParameter = "MusicVolume";
    private const string SfxMixerParameter = "SFXVolume";

    private const float MinimumVolumeDb = -80f;

    private void Awake()
    {
        // Prevent multiple AudioManagers from existing.
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        // Keep this object when changing scenes.
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        // Apply saved volume settings.
        ApplyMixerVolume(
            MusicMixerParameter,
            GetMusicVolume()
        );

        ApplyMixerVolume(
            SfxMixerParameter,
            GetSfxVolume()
        );

        // Start the background music.
        if (musicSource != null &&
            musicSource.clip != null &&
            !musicSource.isPlaying)
        {
            musicSource.loop = true;
            musicSource.Play();
        }
    }

    public void SetMusicVolume(float volume)
    {
        volume = Mathf.Clamp01(volume);

        ApplyMixerVolume(MusicMixerParameter, volume);
        PlayerPrefs.SetFloat(MusicVolumeKey, volume);
    }

    public void SetSfxVolume(float volume)
    {
        volume = Mathf.Clamp01(volume);

        ApplyMixerVolume(SfxMixerParameter, volume);
        PlayerPrefs.SetFloat(SfxVolumeKey, volume);
    }

    public float GetMusicVolume()
    {
        return PlayerPrefs.GetFloat(MusicVolumeKey, 1f);
    }

    public float GetSfxVolume()
    {
        return PlayerPrefs.GetFloat(SfxVolumeKey, 1f);
    }

    public void PlaySFX(AudioClip clip)
    {
        if (clip == null || sfxSource == null)
        {
            return;
        }

        sfxSource.PlayOneShot(clip);
    }

    public void PlayMusic(AudioClip clip)
    {
        if (clip == null || musicSource == null)
        {
            return;
        }

        if (musicSource.clip == clip && musicSource.isPlaying)
        {
            return;
        }

        musicSource.Stop();
        musicSource.clip = clip;
        musicSource.loop = true;
        musicSource.Play();
    }

    private void ApplyMixerVolume(string parameterName, float sliderValue)
    {
        if (audioMixer == null)
        {
            Debug.LogWarning("Audio Mixer is not assigned.");
            return;
        }

        // A slider uses 0–1, but the Audio Mixer uses decibels.
        float volumeInDb = sliderValue <= 0.0001f
            ? MinimumVolumeDb
            : Mathf.Log10(sliderValue) * 20f;

        bool parameterFound =
            audioMixer.SetFloat(parameterName, volumeInDb);

        if (!parameterFound)
        {
            Debug.LogWarning(
                $"Audio Mixer parameter '{parameterName}' was not found."
            );
        }
    }

    private void OnApplicationPause(bool paused)
    {
        if (paused)
        {
            PlayerPrefs.Save();
        }
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.Save();
    }
}