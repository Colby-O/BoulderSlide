using PlazmaGames.Animation;
using PlazmaGames.Audio;
using PlazmaGames.Core;
using PlazmaGames.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LJGameManager : GameManager
{
    [Header("Holders")]
    [SerializeField] private GameObject _monoSystemParnet;

    [Header("MonoSystems")]
    [SerializeField] private UIMonoSystem _uiMonoSystem;
    [SerializeField] private AnimationMonoSystem _animationMonoSystem;
    [SerializeField] private AudioMonoSystem _audioMonoSystem;

    /// <summary>
    /// Adds all events listeners
    /// </summary>
    private void AddListeners()
    {

    }

    /// <summary>
    /// Removes all events listeners
    /// </summary>
    private void RemoveListeners()
    {

    }

    /// <summary>
    /// Attaches all MonoSystems to the GameManager
    /// </summary>
    private void AttachMonoSystems()
    {
        AddMonoSystem<UIMonoSystem, IUIMonoSystem>(_uiMonoSystem);
        AddMonoSystem<AnimationMonoSystem, IAnimationMonoSystem>(_animationMonoSystem);
        AddMonoSystem<AudioMonoSystem, IAudioMonoSystem>(_audioMonoSystem);
    }

    private void AddEvents()
    {

    }

    public override string GetApplicationName()
    {
        return nameof(LJGameManager);
    }

    protected override void OnInitalized()
    {
        // Ataches all MonoSystems to the GameManager
        AttachMonoSystems();

        // Adds Event Listeners
        AddEvents();

        // Adds Event Listeners
        AddListeners();

        // Ensures all MonoSystems call Awake at the same time.
        _monoSystemParnet.SetActive(true);
    }

    private void Awake()
    {

    }

    private void Start()
    {

    }

    public override string GetApplicationVersion()
    {
        return "Beta V0.0.1";
    }
}
