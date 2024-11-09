using PlazmaGames.Audio;
using PlazmaGames.Core;
using PlazmaGames.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ControlView : View
{
    [SerializeField] private Button _back;

    private void Back()
    {
        GameManager.GetMonoSystem<IUIMonoSystem>().ShowLast();
    }

    public override void Init()
    {
        _back.onClick.AddListener(Back);
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
