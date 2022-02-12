using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

public class SettingsMenu : MonoBehaviour
{
    public AudioMixer audioMixer;
    public TMP_Text masterVolumeText;
    public TMP_Text gameplayVolumeText;
    public TMP_Text musicVolumeText;
    public TMP_Text multiplayerVolumeText;
    public Toggle friendlyChatVolumeToggle;
    public Toggle neutralChatVolumeToggle;
    public Toggle enemyChatVolumeToggle;


    private float ConvertFloatToDecible(float input)
    {
        return Mathf.Log10(input) * 20;
    }
    private int ConvertDecibleToPercent(float decible)
    {
        return Mathf.RoundToInt((decible + 80f) * 1.25f);
    }
    public void SetMasterVolume(float volume)
    {
        volume = ConvertFloatToDecible(volume);
        audioMixer.SetFloat("MasterVolume", volume);
        masterVolumeText.SetText($"Master Volume\n{ConvertDecibleToPercent(volume)} %");
    }
    public void SetGameplayVolume(float volume)
    {
        volume = ConvertFloatToDecible(volume);
        audioMixer.SetFloat("GameplayVolume", volume);
        gameplayVolumeText.SetText($"Game Volume\n{ConvertDecibleToPercent(volume)} %");
    }
    public void SetMusicVolume(float volume)
    {
        volume = ConvertFloatToDecible(volume);
        audioMixer.SetFloat("MusicVolume", volume);
        musicVolumeText.SetText($"Music Volume\n{ConvertDecibleToPercent(volume)} %");
    }
    public void SetMultiplayerVolume(float volume)
    {
        volume = ConvertFloatToDecible(volume);
        audioMixer.SetFloat("MultiplayerVolume", volume);
        multiplayerVolumeText.SetText($"Multiplayer Volume\n{ConvertDecibleToPercent(volume)} %");
    }
    public void ToggleFriendlyChat()
    {
        float volume = friendlyChatVolumeToggle.isOn ? 0f : -80f;
        audioMixer.SetFloat("FriendlyChatVolume", volume);
    }
    public void ToggleNeutralChat()
    {
        float volume = neutralChatVolumeToggle.isOn ? 0f : -80f;
        audioMixer.SetFloat("NeutralChatVolume", volume);
    }
    public void ToggleEnemyChat()
    {
        float volume = enemyChatVolumeToggle.isOn ? 0f : -80f;
        audioMixer.SetFloat("EnemyChatVolume", volume);
    }
}
