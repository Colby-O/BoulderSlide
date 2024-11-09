using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class Player : MonoBehaviour
{
	private IGridMonoSystem _gridMs;

	[SerializeField] private Transform _grid;
	[SerializeField] private int _myGridId;
	private float _gridCellSize;
	private PlayerInput _input;
	private Animator _animator;

	private Vector2Int _inputDirection = Vector2Int.zero;
	private Vector2Int _position = Vector2Int.zero;
	private Vector2Int _moving = Vector2Int.zero;
	private float _moveStart = 0;
	[SerializeField] private float _moveSpeed = 0.25f;

	void Start()
	{
		_gridMs = LJGameManager.GetMonoSystem<IGridMonoSystem>();
		_gridCellSize = _gridMs.CellSize();
		_input = GetComponent<PlayerInput>();
		_animator = GetComponent<Animator>();

		_input.actions["Movement"].performed += HandleMovement;
	}

	void FixedUpdate()
	{
		if (_moving != Vector2Int.zero)
		{
			if (Time.time < _moveStart + _moveSpeed)
			{
				transform.Translate((Vector2)_moving * _gridCellSize * Time.deltaTime / _moveSpeed);
			}
			else
			{
				Tile next = _gridMs.TileAt(_myGridId, _position + _moving);
				if (next != null && next.type == TileType.Ice && !next.hasBoulder)
				{
					_moveStart = Time.time;
					_position += _moving;
				}
				else
				{
					_moving = Vector2Int.zero;
				}
			}
		}

		if (_moving == Vector2Int.zero && _inputDirection != Vector2Int.zero)
		{
			if (_gridMs.IsWalkableAt(_myGridId, _position + _inputDirection))
			{
				_moveStart = Time.time;
				_moving = _inputDirection;
				_position += _moving;
			}
		}

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
}
