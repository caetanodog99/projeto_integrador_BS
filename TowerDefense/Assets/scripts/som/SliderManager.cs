using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderManager: MonoBehaviour
{
    [SerializeField] Slider volumeSlider;
    [SerializeField] AudioSource musica;
    void Start()
    {
        if (!PlayerPrefs.HasKey("musicVolume"))
        {
            PlayerPrefs.SetFloat("musicVolume", 1);
            Load();
        }
        else
        {
            Load();
        }
    }
    public void MudarVolume()
    {
        musica.volume = volumeSlider.value;
        save();
    }
    private void Load()
    {
        volumeSlider.value = PlayerPrefs.GetFloat("musicVolume");
    }
    private void save()
    {
        PlayerPrefs.SetFloat("musicVolume",volumeSlider.value);
    }
}
