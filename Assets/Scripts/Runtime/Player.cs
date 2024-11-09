using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;
using PlazmaGames.Core;
using PlazmaGames.Audio;

public class Player : MonoBehaviour
{
	private IGridMonoSystem _gridMs;

	[SerializeField] private Transform _gridTransform;
	[SerializeField] private int _myGridId;
    [SerializeField] private AudioSource _audio;
    [SerializeField] private Player _other;
    private float _gridCellSize;
	private PlayerInput _input;
	private Animator _animator;

	private Vector2Int _inputDirection = Vector2Int.zero;
	private Vector2Int _position = new Vector2Int(1, 1);
	private Vector2Int _moving = Vector2Int.zero;
	private float _moveStart = 0;
	[SerializeField] private float _moveSpeed = 0.25f;

	private bool _isPushingBoulder = false;
	private Vector2Int _boulderPushDirection;
	private Vector2Int _boulderPushEndPos;
	private float _boulderPushStart;
	private Transform _boulderPushingTransform;
	[SerializeField] private float _boulderMoveSpeed = 0.25f;

	public void SetPosition(Vector2Int pos)
	{
		_position = pos;
		SyncPosition();
	}

	void Start()
	{
		_gridMs = GameManager.GetMonoSystem<IGridMonoSystem>();
		_gridCellSize = _gridMs.CellSize();
		_gridTransform = _gridMs.GridTransform(_myGridId);
		transform.parent = _gridTransform;
		SyncPosition();
		_input = GetComponent<PlayerInput>();
		_animator = GetComponentInChildren<Animator>();

		_input.actions["Movement"].performed += HandleMovement;
	}

	private void Finish()
	{
        GameManager.GetMonoSystem<IAudioMonoSystem>().PlayAudio("Win", PlazmaGames.Audio.AudioType.Sfx, false, true);
        LJGameManager.numberCompletedInRow++;
        _moving = Vector2Int.zero;
		LJGameManager.numOfHoles = Mathf.Min(LJGameManager.numOfHoles + 1, LJGameManager.maxNumOfHoles);
        _gridMs.NewGrid(LJGameManager.numOfHoles);
        SetPosition(new Vector2Int(1, 1));
        _other.SetPosition(new Vector2Int(1, 1));
    }

    private void Death()
	{
		_moving = Vector2Int.zero;
        _gridMs.ResetGrid();
        SetPosition(new Vector2Int(1, 1));
        _other.SetPosition(new Vector2Int(1, 1));
    }

	void FixedUpdate()
	{
		if (_isPushingBoulder)
		{
			if (Time.time < _boulderPushStart + _boulderMoveSpeed)
			{
                _boulderPushingTransform.Translate((Vector2)_boulderPushDirection * _gridCellSize * Time.deltaTime / _boulderMoveSpeed);
			}
			else
			{
                _isPushingBoulder = false;
				Tile tile = _gridMs.TileAt(_myGridId, _boulderPushEndPos);
				if (tile != null && tile.type == TileType.Hole)
				{
                    GameManager.GetMonoSystem<IAudioMonoSystem>().PlayAudio("RockDrop", PlazmaGames.Audio.AudioType.Sfx, false, true);
                    Tile fall = _gridMs.TileAt(0, _boulderPushEndPos);
					tile.hasBoulder = false;
					fall.hasBoulder = true;
					_gridMs.SetTileAt(0, _boulderPushEndPos, TileType.HoleFilled);
                    fall.boulderGameObject = tile.boulderGameObject;
					_boulderPushingTransform.parent = _gridMs.GridTransform(0);
				}

				Vector3 pos = (Vector2)_boulderPushEndPos * _gridCellSize;
				pos.z = -1;
				_boulderPushingTransform.localPosition = pos;
			}
		}
		else if (_moving != Vector2Int.zero)
		{
			if (Time.time < _moveStart + _moveSpeed)
			{
				transform.Translate((Vector2)_moving * _gridCellSize * Time.deltaTime / _moveSpeed);
			}
			else
			{
				SyncPosition();
				Tile next = _gridMs.TileAt(_myGridId, _position + _moving);
				if (next != null && next.type == TileType.Ice && !next.hasBoulder)
				{
					_moveStart = Time.time;
					_position += _moving;
				}
				else
				{
                    _moving = Vector2Int.zero;
					if (next != null && next.type == TileType.Water)
					{
                        if (_myGridId == 0) GameManager.GetMonoSystem<IAudioMonoSystem>().PlayAudio("Splash", PlazmaGames.Audio.AudioType.Sfx, false, true);
                        Death();
					}
					else if (next != null && next.type == TileType.End)
					{
						Finish();
					}
					else
					{
                        if (_myGridId == 0) GameManager.GetMonoSystem<IAudioMonoSystem>().PlayAudio("RockHit", PlazmaGames.Audio.AudioType.Sfx, false, true);
                    }
				}
			}
		}

		if (!_isPushingBoulder && _moving == Vector2Int.zero && _inputDirection != Vector2Int.zero)
		{
			Tile tile = _gridMs.TileAt(_myGridId, _position + _inputDirection);
			if (_gridMs.IsWalkableAt(_myGridId, _position + _inputDirection))
			{
				_moveStart = Time.time;
				_moving = _inputDirection;
				_position += _moving;
			}
			else if (
				_myGridId == 1 &&
				tile != null &&
				tile.hasBoulder && (
					_gridMs.IsWalkableAt(_myGridId, _position + _inputDirection * 2) ||
					_gridMs.TileAt(_myGridId, _position + _inputDirection * 2).type == TileType.Hole
				)
			)
			{
                if (_gridMs.TileAt(_myGridId, _position + _inputDirection * 2).type != TileType.Hole) GameManager.GetMonoSystem<IAudioMonoSystem>().PlayAudio("RockMove", PlazmaGames.Audio.AudioType.Sfx, false, true);
                _isPushingBoulder = true;
				_boulderPushDirection = _inputDirection;
				_boulderPushEndPos = _position + _inputDirection * 2;
				_boulderPushingTransform = tile.boulderGameObject.transform;
				_boulderPushStart = Time.time;
				Tile from = _gridMs.TileAt(_myGridId, _position + _inputDirection);
				Tile to = _gridMs.TileAt(_myGridId, _position + _inputDirection * 2);
				from.hasBoulder = false;
				to.hasBoulder = true;
				to.boulderGameObject = from.boulderGameObject;
			}
		}

		if (_moving.magnitude < 0.01f) _audio.Stop();
		else if (!_audio.isPlaying) _audio.Play();

        if (_moving.x > 0) _animator.SetInteger("Move", 4);
		else if (_moving.x < 0) _animator.SetInteger("Move", 3);
		else if (_moving.y > 0) _animator.SetInteger("Move", 2);
		else if (_moving.y < 0) _animator.SetInteger("Move", 1);
		else _animator.SetInteger("Move", 0);
	}

	private void HandleMovement(InputAction.CallbackContext e)
	{
		Vector2 raw = e.ReadValue<Vector2>();
		Debug.Log(raw);
		_inputDirection = new Vector2Int((int)Mathf.Round(raw.x), (int)Mathf.Round(raw.y));
		if (_inputDirection.x != 0 && _inputDirection.y != 0) _inputDirection.y = 0;
	}

	private void SyncPosition()
	{
		Vector3 pos = (Vector2)_position * _gridCellSize;
		pos.z = -2.0f;
		transform.localPosition = pos;
	}

    private void Update()
    {
		if (_myGridId == 0 && Keyboard.current[Key.R].wasPressedThisFrame)
		{
            GameManager.GetMonoSystem<IAudioMonoSystem>().PlayAudio("Restart", PlazmaGames.Audio.AudioType.Sfx, false, true);
            Death();
		}
    }
}
