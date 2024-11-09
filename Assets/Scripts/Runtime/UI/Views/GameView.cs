using PlazmaGames.Audio;
using PlazmaGames.Core;
using PlazmaGames.Core.Utils;
using PlazmaGames.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameView : View
{
    [SerializeField] TMP_Text _highscore;
    [SerializeField] TMP_Text _time;

    private float _timer = 600f;
    private bool _timerEnable = false;
    
    public void StartTimer()
    {
        _timer = 600f;
        _timerEnable = true;
    }

    public void StopTimer()
    {
        _timerEnable = false;
    }

    private void Puase()
    {
        GameManager.GetMonoSystem<IUIMonoSystem>().Show<PausedView>();
    }

    public override void Init()
    {

    }

    public override void Show()
    {
        base.Show();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public override void Hide()
    {
        base.Hide();
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }

    private void Update()
    {
        _highscore.text = $"Score: {LJGameManager.numberCompletedInRow}";

        if (Keyboard.current[Key.Escape].wasPressedThisFrame)
        {
            Puase();
            GameManager.GetMonoSystem<IAudioMonoSystem>().PlayAudio("ButtonClick", PlazmaGames.Audio.AudioType.Sfx, false, true);
        }

        if (_timerEnable) _timer -= UnityEngine.Time.deltaTime;

        if (_timer <= 0)
        {
            if (LJGameManager.highscore < LJGameManager.numberCompletedInRow)
            {
                LJGameManager.highscore = LJGameManager.numberCompletedInRow;
            }

            GameManager.GetMonoSystem<IAudioMonoSystem>().StopAudio(PlazmaGames.Audio.AudioType.Music);
            GameManager.GetMonoSystem<IAudioMonoSystem>().PlayAudio(0, PlazmaGames.Audio.AudioType.Music, true, false);
            GameManager.GetMonoSystem<IAudioMonoSystem>().PlayAudio("Beep", PlazmaGames.Audio.AudioType.Sfx, false, true);
            StopTimer();
            GameManager.GetMonoSystem<IUIMonoSystem>().Show<MainMenuView>();
        }

        TimeSpan t = TimeSpan.FromSeconds(_timer);
        _time.text = "Timer: " + t.ToString(@"mm\:ss");
    }
}
