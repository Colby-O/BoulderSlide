using PlazmaGames.Core;
using PlazmaGames.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuView : View
{
    [SerializeField] private Button _play;
    [SerializeField] private Button _settings;
    [SerializeField] private Button _exit;

    private void Play()
    {
        GameManager.GetMonoSystem<IUIMonoSystem>().Show<GameView>();
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
        _play.onClick.AddListener(Play);
        _settings.onClick.AddListener(Settings);
        _exit.onClick.AddListener(Exit);
    }
}
