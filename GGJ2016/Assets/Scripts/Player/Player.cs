﻿using UnityEngine;
using System.Collections;
using InControl;
using DG.Tweening;

public class Player : MonoBehaviour {

	public int playerNum;

	public float throwForce 	= 200f;
	public float speed 			= 5f;
	public float repickDelay 	= 1.5f;
	public float reviveDelay	= 1f;
	public float pushDistance	= 3f;

	public GameObject itemPosition;

	private bool _alive = true;
	private bool _throw = false;
	private float _repickCounter = 0;
	private float _reviveCounter = 0;
	private Item _item;
	private Item _lastItem;
	private Vector2 _initPos;
	private Vector2 _velocity;
	private Vector2 _force;
	private Rigidbody2D _rigidbody;
	private PlayerInput _playerInput;
	private Collider2D _collider;
	private Animator _animator;
	private MusicManager _musicManager;

	void Awake() 
	{
		_musicManager 	= FindObjectOfType<MusicManager>();
		_rigidbody 		= GetComponent<Rigidbody2D>();
		_collider 		= GetComponent<Collider2D>();
		_animator		= GetComponent<Animator> ();
		_initPos 		= transform.position;

 		if(GameManager.Instance.playerDevices.Count > playerNum - 1)
		{
			//Take the controller assigned on the menu
			_playerInput 		= new PlayerInput(true);
			_playerInput.Device = InputManager.Devices[GameManager.Instance.playerDevices[playerNum - 1]];
			Debug.Log(playerNum + " " + _playerInput.Device.Name);
		}
		else
		{
			//Test only
			Debug.Log("Controllers " + InputManager.Devices.Count);

			if(InputManager.Devices.Count > playerNum - 1)
			{
				_playerInput = new PlayerInput(true);
				_playerInput.Device = InputManager.Devices[playerNum - 1];
			}
			else
				_playerInput = new PlayerInput(false);
			
			Debug.LogWarning("No input for player " + playerNum);
		}

		FindObjectOfType<GameOverManager>().OnGameOver += HandleGameOver;
	}

	void HandleOnGameOverDelegate (int ganador)
	{
		
	}
	
	void Update() 
	{
		if(_alive && GameManager.Instance.isInGame)
		{
			if(_playerInput != null)
			{
				//Movement
				_velocity = new Vector2(_playerInput.move.X * speed, _playerInput.move.Y * speed) + _force;
				_rigidbody.velocity = _velocity;

				//Right Joystick Logic
				if(_playerInput.shoot.IsPressed)
				{
					float angle = Mathf.Atan2(_playerInput.shoot.Y, _playerInput.shoot.X);

					//Throw Item
					if(_item != null)
					{
						_item.Throw(angle, throwForce);
						_lastItem 	= _item;
						_item 		= null;
					}
					//Push Player
					else
					{
						if(_lastItem == null && !_throw)
						{
							_throw = true;
							#if UNITY_EDITOR
							Debug.DrawLine(transform.position, transform.position + (new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * pushDistance)); 
							#endif
							RaycastHit2D hit = Physics2D.Raycast(transform.position, new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0));
							if (hit.collider != null) {
								if(hit.collider.gameObject.CompareTag("Player"))
								{
									float distance = Vector2.Distance(transform.position, hit.point);
									if(distance <= pushDistance)
									{
										hit.collider.gameObject.GetComponent<Player>().Push(angle);
									}
								}
							}
						}
					}
				}

				_force -= new Vector2(0.1f,0.1f);
				if(_force.x < 0) _force.x = 0;
				if(_force.y < 0) _force.y = 0;
			}

			//Delay applied in order to not re-pick up an item too fast
			if(_lastItem != null || _throw)
			{
				_repickCounter += Time.deltaTime;
				if(repickDelay < _repickCounter)
				{
					_repickCounter 	= 0f;
					_lastItem 		= null;
					_throw 			= false;
				}
			}
			//Paso información al animator
			_animator.SetBool("carry", (_item != null));
			_animator.SetFloat ("speed", _velocity.magnitude);
		}
		else
		{
			_reviveCounter += Time.deltaTime;
			if(reviveDelay < _reviveCounter)
			{
				_reviveCounter 			= 0f;
				_alive 					= true;
				transform.position 		= _initPos;
				transform.rotation 		= Quaternion.identity;
				transform.localScale 	= Vector3.one;
				_collider.enabled 		= true;
			}
		}
	}

	private void Dead()
	{
		if(_item != null)
			Destroy(_item.gameObject);

		transform.DOScale(Vector3.zero, 0.5f);
		transform.DORotate(new Vector3(0,0,200), 0.5f);
		_musicManager.playCaeLavaPJ();

		_rigidbody.velocity *= 0.5f;
		_collider.enabled 	= false;
		_alive 				= false;
		_lastItem 			= null;
		_item 				= null;
		_force 				= Vector2.zero;
	}

	public void Push(float angle)
	{
		if(_item != null)
		{
			_item.Throw(angle, throwForce);
			_lastItem 	= _item;
			_item 		= null;
		}
		Vector2 forward = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
		_force = forward * (speed + 4);
	}

	private void HandleGameOver(int ganador){
		if (playerNum != ganador) {
			//EXPLOTA
			Debug.Log("Player" + playerNum + " pierde" );
		}
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		//Pick up an item
		if(other.gameObject.CompareTag("Item"))
		{
			Item _i = other.transform.parent.GetComponent<Item>();
			if(!_i.IsPickedUp && _lastItem != _i)
			{
				if(_item == null && (_i.IsThrown || _lastItem == null))
				{
					_lastItem 	= _item;
					_item 		=  _i;
					_musicManager.playPickItem();
					_item.PickUp(itemPosition.transform);

					if(_item.IsThrown)
					{
						Vector2 forward = new Vector2(Mathf.Cos(_item.Angle), Mathf.Sin(_item.Angle));
						_force += forward * (speed + 4);
					}

					if (_item.spawnerOcupado != null) {
						_item.spawnerOcupado.itsFree = true;
						_item.spawnerOcupado  = null;
					}
				}
				else if(_item != null && _i.IsThrown)
				{
					_lastItem 	= _item;
					_item 		=  _i;
					_musicManager.playPickItem();
					_lastItem.Throw(_i.Angle, throwForce);
					_item.PickUp(itemPosition.transform);
					Vector2 forward = new Vector2(Mathf.Cos(_item.Angle), Mathf.Sin(_item.Angle));
					_force += forward * (speed + 4);
				}
			}
		}
		else if(other.gameObject.CompareTag("Lava"))
		{
			Dead();
			_rigidbody.velocity = Vector2.zero;
			transform.DOMove(other.gameObject.transform.position + (other.gameObject.transform.localScale * 0.5f), 0.5f);
		}
	}
}

//Incontrol player action set class
public class PlayerInput : PlayerActionSet
{ 
	public PlayerTwoAxisAction move;
	public PlayerTwoAxisAction shoot;

	private PlayerAction _left, _right, _up, _down;
	private PlayerAction _shootLeft, _shootRight, _shootUp, _shootDown;

	public PlayerInput(bool haveGamepad)
	{
		_left 		= CreatePlayerAction("Move Left");
		_right 		= CreatePlayerAction("Move Right");
		_up 		= CreatePlayerAction("Move Up");
		_down 		= CreatePlayerAction("Move Down");
		move		= CreateTwoAxisPlayerAction(_left, _right, _down, _up);

		_shootLeft 	= CreatePlayerAction("Shoot Left");
		_shootRight	= CreatePlayerAction("Shoot Right");
		_shootUp 	= CreatePlayerAction("Shoot Up");
		_shootDown 	= CreatePlayerAction("Shoot Down");
		shoot		= CreateTwoAxisPlayerAction(_shootLeft, _shootRight, _shootDown, _shootUp);

		//Gamepad
		if(haveGamepad)
		{
			_left.AddDefaultBinding(InputControlType.LeftStickLeft);
			_right.AddDefaultBinding(InputControlType.LeftStickRight);
			_up.AddDefaultBinding(InputControlType.LeftStickUp);
			_down.AddDefaultBinding(InputControlType.LeftStickDown);

			_left.AddDefaultBinding(InputControlType.DPadLeft);
			_right.AddDefaultBinding(InputControlType.DPadRight);
			_up.AddDefaultBinding(InputControlType.DPadUp);
			_down.AddDefaultBinding(InputControlType.DPadDown);

			_shootLeft.AddDefaultBinding(InputControlType.RightStickLeft);
			_shootRight.AddDefaultBinding(InputControlType.RightStickRight);
			_shootUp.AddDefaultBinding(InputControlType.RightStickUp);
			_shootDown.AddDefaultBinding(InputControlType.RightStickDown);

			_shootLeft.AddDefaultBinding(InputControlType.Action3);
			_shootRight.AddDefaultBinding(InputControlType.Action2);
			_shootUp.AddDefaultBinding(InputControlType.Action4);
			_shootDown.AddDefaultBinding(InputControlType.Action1);
		}
		else
		{
			_left.AddDefaultBinding(Key.LeftArrow);
			_right.AddDefaultBinding(Key.RightArrow);
			_up.AddDefaultBinding(Key.UpArrow);
			_down.AddDefaultBinding(Key.DownArrow);

			_shootLeft.AddDefaultBinding(Key.A);
			_shootRight.AddDefaultBinding(Key.D);
			_shootUp.AddDefaultBinding(Key.W);
			_shootDown.AddDefaultBinding(Key.S);
		}
	}
}
