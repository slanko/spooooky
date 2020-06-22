﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class settingsScript : MonoBehaviour
{
    public Dropdown settingsDropdown;
    public Slider musicSlider, sfxSlider;

    private void Start()
    {
        QualitySettings.SetQualityLevel(PlayerPrefs.GetInt("qualityLevel", 6) , true);
        settingsDropdown.value = QualitySettings.GetQualityLevel();
        musicSlider.value = PlayerPrefs.GetFloat("musicVolume", 0.4f);
        sfxSlider.value = PlayerPrefs.GetFloat("audioVolume");
        if (SceneManager.GetActiveScene().name == "MainMenuTest")
        {
            GameObject.Find("menuMuzik").GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("musicVolume");
        }
    }
    public void settingsChange()
    {
        QualitySettings.SetQualityLevel(settingsDropdown.value, true);
        PlayerPrefs.SetInt("qualityLevel", settingsDropdown.value);
        PlayerPrefs.Save();
    }

    public void audioChange()
    {
        PlayerPrefs.SetFloat("musicVolume", musicSlider.value);
        if (SceneManager.GetActiveScene().name == "MainMenuTest")
        {
            GameObject.Find("menuMuzik").GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("musicVolume");
        }
        PlayerPrefs.Save();
    }

    public void SFXchange()
    {
        PlayerPrefs.SetFloat("audioVolume", sfxSlider.value);
        Debug.Log(PlayerPrefs.GetFloat("audioVolume"));
        PlayerPrefs.Save();
    }

    public void audioChangeAtRuntime()
    {
        PlayerPrefs.SetFloat("musicVolume", musicSlider.value);
        PlayerPrefs.Save();
        GameObject.Find("GOD").GetComponent<godScript>().muzik.volume = PlayerPrefs.GetFloat("musicVolume");
        if(SceneManager.GetActiveScene().name == "MainMenuTest")
        {
            GameObject.Find("menuMuzik").GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("musicVolume");
        }
    }

    public void sfxChangeAtRuntime()
    {
        PlayerPrefs.SetFloat("audioVolume", sfxSlider.value);
        Debug.Log(PlayerPrefs.GetFloat("audioVolume"));
        PlayerPrefs.Save();
        GameObject.Find("GOD").GetComponent<godScript>().globalAudioVolume = PlayerPrefs.GetFloat("audioVolume");
    }


}
