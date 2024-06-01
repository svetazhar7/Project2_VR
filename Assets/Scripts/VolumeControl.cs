using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class VolumeControl : MonoBehaviour
{
    public AudioSource audioSource;
    public Slider volumeSlider;
    public TextMeshProUGUI volumeText;

    private void Start()
    {
        // Устанавливаем значение слайдера в соответствии с текущей громкостью AudioSource
        volumeSlider.value = audioSource.volume;
        UpdateVolumeText();

        // Назначаем обработчик события для Slider
        volumeSlider.onValueChanged.AddListener(SetVolume);
    }

    public void SetVolume(float volume)
    {
        audioSource.volume = volume;
        UpdateVolumeText();
    }

    private void UpdateVolumeText()
    {
        // Отображаем текущий уровень громкости в процентах
        volumeText.text = Mathf.RoundToInt(audioSource.volume * 100) + "%";
    }
}