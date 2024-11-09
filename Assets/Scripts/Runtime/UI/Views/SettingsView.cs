using PlazmaGames.Audio;
using PlazmaGames.Core;
using PlazmaGames.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class SettingsView : View
{
    [SerializeField] private Button _back;
    [SerializeField] private Slider _overall;
    [SerializeField] private Slider _sfx;
    [SerializeField] private Slider _music;

    private void Back()
    {
        GameManager.GetMonoSystem<IUIMonoSystem>().ShowLast();
    }

    private void Overall(float val)
    {
        GameManager.GetMonoSystem<IAudioMonoSystem>().SetOverallVolume(val);
    }

    private void SfX(float val)
    {
        GameManager.GetMonoSystem<IAudioMonoSystem>().SetSfXVolume(val);
        GameManager.GetMonoSystem<IAudioMonoSystem>().SetAmbientVolume(val);
    }

    private void Music(float val)
    {
        GameManager.GetMonoSystem<IAudioMonoSystem>().SetMusicVolume(val);
    }

    public override void Init()
    {
        _back.onClick.AddListener(Back);

        _overall.onValueChanged.AddListener(Overall);
        _sfx.onValueChanged.AddListener(SfX);
        _music.onValueChanged.AddListener(Music);

        GameManager.GetMonoSystem<IAudioMonoSystem>().SetOverallVolume(1f);
        GameManager.GetMonoSystem<IAudioMonoSystem>().SetMusicVolume(0.08f);
        GameManager.GetMonoSystem<IAudioMonoSystem>().SetSfXVolume(0.5f);
        GameManager.GetMonoSystem<IAudioMonoSystem>().SetAmbientVolume(0.5f);

        _overall.value = GameManager.GetMonoSystem<IAudioMonoSystem>().GetOverallVolume();
        _music.value = GameManager.GetMonoSystem<IAudioMonoSystem>().GetMusicVolume();
        _sfx.value = GameManager.GetMonoSystem<IAudioMonoSystem>().GetSfXVolume();
    }

    public override void Show()
    {
        base.Show();
        _overall.value = GameManager.GetMonoSystem<IAudioMonoSystem>().GetOverallVolume();
        _music.value = GameManager.GetMonoSystem<IAudioMonoSystem>().GetMusicVolume();
        _sfx.value = GameManager.GetMonoSystem<IAudioMonoSystem>().GetSfXVolume();
    }

    private void Update()
    {
        if (Keyboard.current[Key.Escape].wasPressedThisFrame)
        {
            Back();
            GameManager.GetMonoSystem<IAudioMonoSystem>().PlayAudio("ButtonClick", PlazmaGames.Audio.AudioType.Sfx, false, true);
        }
    }
}
