using PlazmaGames.Audio;
using PlazmaGames.Core;
using PlazmaGames.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameView : View
{
    private void Puase()
    {
        GameManager.GetMonoSystem<IUIMonoSystem>().Show<PausedView>();
    }


    public override void Init()
    {

    }

    private void Update()
    {
        if (Keyboard.current[Key.Escape].wasPressedThisFrame)
        {
            Puase();
            GameManager.GetMonoSystem<IAudioMonoSystem>().PlayAudio("ButtonClick", PlazmaGames.Audio.AudioType.Sfx, false, true);
        }
    }
}
