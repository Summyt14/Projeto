using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SetVolume : MonoBehaviour
{
    [SerializeField] private AudioMixer mixer;
    [SerializeField] private String slider;

    private void Start()
    {
        if (mixer.GetFloat(slider, out float value))
            GetComponent<Slider>().value = Mathf.Pow(10, value / 20f);
    }

    public void SetLevelMaster(float sliderValue)
    {
        SetLevel(sliderValue, "MasterVol");
    }
    
    public void SetLevelSounds(float sliderValue)
    {
        SetLevel(sliderValue, "SoundsVol");
    }
    
    public void SetLevelMusic(float sliderValue)
    {
        SetLevel(sliderValue, "MusicVol");
    }
    
    private void SetLevel(float sliderValue, string mixerParam)
    {
        mixer.SetFloat(mixerParam, Mathf.Log10(sliderValue) * 20);
    }
}