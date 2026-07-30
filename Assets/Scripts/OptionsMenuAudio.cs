using UnityEngine;
using UnityEngine.UI;

public class OptionsMenuAudio : MonoBehaviour
{
    [Header("Volume Sliders")]
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;

    private void Start()
    {
        if (AudioManager.Instance == null)
        {
            Debug.LogError(
                "No AudioManager was found. " +
                "Make sure the AudioManager exists before opening this scene."
            );

            return;
        }

        // Load the saved values without triggering the slider events.
        musicSlider.SetValueWithoutNotify(
            AudioManager.Instance.GetMusicVolume()
        );

        sfxSlider.SetValueWithoutNotify(
            AudioManager.Instance.GetSfxVolume()
        );

        // Run the methods whenever the sliders move.
        musicSlider.onValueChanged.AddListener(OnMusicVolumeChanged);
        sfxSlider.onValueChanged.AddListener(OnSfxVolumeChanged);
    }

    public void OnMusicVolumeChanged(float volume)
    {
        AudioManager.Instance?.SetMusicVolume(volume);
    }

    public void OnSfxVolumeChanged(float volume)
    {
        AudioManager.Instance?.SetSfxVolume(volume);
    }

    private void OnDestroy()
    {
        // Clean up the listeners when leaving the Options scene.
        if (musicSlider != null)
        {
            musicSlider.onValueChanged.RemoveListener(
                OnMusicVolumeChanged
            );
        }

        if (sfxSlider != null)
        {
            sfxSlider.onValueChanged.RemoveListener(
                OnSfxVolumeChanged
            );
        }

        PlayerPrefs.Save();
    }
}
