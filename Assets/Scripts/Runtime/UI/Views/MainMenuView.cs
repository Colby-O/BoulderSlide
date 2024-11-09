using PlazmaGames.Audio;
using PlazmaGames.Core;
using PlazmaGames.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuView : View
{
    [SerializeField] private Button _play;
    [SerializeField] private Button _settings;
    [SerializeField] private Button _exit;
    [SerializeField] private Button _help;

    [SerializeField] private TMP_Text _highscore;

    private void Play()
    {
        LJGameManager.numOfHoles = 2;
        GameManager.GetMonoSystem<IUIMonoSystem>().GetView<GameView>().StartTimer();
        GameManager.GetMonoSystem<IGridMonoSystem>().NewGrid(LJGameManager.numOfHoles);
        Player[] players = FindObjectsOfType<Player>();
        foreach (Player p in players) p.SetPosition(new Vector2Int(1, 1));
        LJGameManager.numberCompletedInRow = 0;
        GameManager.GetMonoSystem<IAudioMonoSystem>().StopAudio(PlazmaGames.Audio.AudioType.Music);
        GameManager.GetMonoSystem<IAudioMonoSystem>().PlayAudio(1, PlazmaGames.Audio.AudioType.Music, true, false);
        GameManager.GetMonoSystem<IUIMonoSystem>().Show<GameView>();
    }

    private void Help()
    {
        GameManager.GetMonoSystem<IUIMonoSystem>().Show<ControlView>();
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
        GameManager.GetMonoSystem<IAudioMonoSystem>().PlayAudio(0, PlazmaGames.Audio.AudioType.Music, true, false);
        _play.onClick.AddListener(Play);
        _settings.onClick.AddListener(Settings);
        _exit.onClick.AddListener(Exit);
        _help.onClick.AddListener(Help);
    }

    private void Update()
    {
        _highscore.text = $"Your High Score is {LJGameManager.highscore}.";
    }
}
