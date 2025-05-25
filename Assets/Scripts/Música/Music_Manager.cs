using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Musica_Fondo : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;

    public void MusicController (float musicSlider)
    {
        audioMixer.SetFloat("volume", Mathf.Log10(musicSlider) * 20);
    }
    public void MusicBGMController(float musicSlider)
    {
        audioMixer.SetFloat("volumeBGM", Mathf.Log10(musicSlider) * 20);
    }
    public void MusicSFXController(float musicSlider)
    {
        audioMixer.SetFloat("volumeSFX", Mathf.Log10(musicSlider) * 20);
    }
}
