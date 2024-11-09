using PlazmaGames.Audio;
using PlazmaGames.Core;
using PlazmaGames.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PausedView : View
{

    [SerializeField] private Button _resume;
    [SerializeField] private Button _settings;
    [SerializeField] private Button _exit;

    private void Resume()
    {
        GameManager.GetMonoSystem<IUIMonoSystem>().ShowLast();
    }

    private void Settings()
    {
        GameManager.GetMonoSystem<IUIMonoSystem>().Show<SettingsView>();
    }

    private void Exit()
    {
        Application.Quit();
    }

    public override void Init()
    {
        _resume.onClick.AddListener(Resume);
        _settings.onClick.AddListener(Settings);
        _exit.onClick.AddListener(Exit);
    }

    private void Update()
    {
        if (Keyboard.current[Key.Escape].wasPressedThisFrame)
        {
            Resume();
            GameManager.GetMonoSystem<IAudioMonoSystem>().PlayAudio("ButtonClick", PlazmaGames.Audio.AudioType.Sfx, false, true);
        }
    }
}
