using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SettingsMenu : MonoBehaviour
{
    public AudioMixer audioMixer;
    public void SetVolume(string _audioType, float volume)
    {
        switch (_audioType)
        {
            case ("master"):
                audioMixer.SetFloat("MasterVolume", volume);
                break;
            case ("gameplay"):
                audioMixer.SetFloat("GameplayVolume", volume);
                break;
            case ("music"):
                audioMixer.SetFloat("MusicVolume", volume);
                break;
            case ("multiplayer"):
                audioMixer.SetFloat("MultiplayerVolume", volume);
                break;
            default:
                break;
        }
    }
    public void SetMasterVolume(float volume)
    {
        audioMixer.SetFloat("MasterVolume", volume);
    }
    public void SetGameplayVolume(float volume)
    {
        audioMixer.SetFloat("GameplayVolume", volume);
    }
    public void SetMusicVolume(float volume)
    {
        audioMixer.SetFloat("MusicVolume", volume);
    }
    public void SetMultiplayerVolume(float volume)
    {
        audioMixer.SetFloat("MultiplayerVolume", volume);
    }
}
