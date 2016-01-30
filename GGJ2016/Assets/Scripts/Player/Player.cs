using UnityEngine;
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
	private float _repickCounter = 0;
	private float _reviveCounter = 0;
	private Item _item;
	private Item _lastItem;
	private Vector2 _initPos;
	private Rigidbody2D _rigidbody;
	private PlayerInput _playerInput;
	private Collider2D _collider;

	void Awake() 
	{
		_rigidbody 	= GetComponent<Rigidbody2D>();
		_collider 	= GetComponent<Collider2D>();
		_initPos 	= transform.position;

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
	}
	
	void Update() 
	{
		if(_alive)
		{
			if(_playerInput != null)
			{
				//Movement
				_rigidbody.velocity = new Vector2(_playerInput.move.X * speed, _playerInput.move.Y * speed);

				//Right Joystick Logic
				if(_playerInput.shoot.IsPressed)
				{
					float _angle = Mathf.Atan2(_playerInput.shoot.Y, _playerInput.shoot.X);

					//Throw Item
					if(_item != null)
					{
						_item.Throw(_angle, throwForce);
						_lastItem 	= _item;
						_item 		= null;
						itemPosition.SetActive(false);
					}
					//Push Player
					else
					{
						Ray2D ray = new Ray2D(transform.position, new Vector2(Mathf.Cos(_angle), Mathf.Sin(_angle) * pushDistance));
						Debug.DrawLine(transform.position, transform.position + new Vector3(Mathf.Cos(_angle), Mathf.Sin(_angle) * pushDistance, 0)); 
					}
				}
			}

			//Delay applied in order to not re-pick up an item too fast
			if(_lastItem != null)
			{
				_repickCounter += Time.deltaTime;
				if(repickDelay < _repickCounter)
				{
					_repickCounter 	= 0f;
					_lastItem 		= null;
				}
			}
		}
		else
		{
			_reviveCounter += Time.deltaTime;
			if(reviveDelay < _reviveCounter)
			{
				transform.DOPause();
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
		itemPosition.SetActive(false);

		_rigidbody.velocity *= 0.5f;
		_collider.enabled 	= false;
		_alive 				= false;
		_lastItem 			= null;
		_item 				= null;

	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		//Pick up an item
		if(other.gameObject.CompareTag("Item"))
		{
			Item _i = other.GetComponent<Item>();
			if(_lastItem != _i)
			{
				if(_item == null)
				{
					_item =  _i;
					itemPosition.SetActive(true);
					_item.PickUp(itemPosition.transform);
				}
				else if(_item != null && _item.IsThrown)
				{
					_lastItem = _item;
					_item.Throw(_i.Angle, throwForce);
					_item = _i;
					_item.PickUp(itemPosition.transform);
				}
			}
		}
		if(other.gameObject.CompareTag("Lava"))
		{
			Dead();
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

			_shootLeft.AddDefaultBinding(InputControlType.RightStickLeft);
			_shootRight.AddDefaultBinding(InputControlType.RightStickRight);
			_shootUp.AddDefaultBinding(InputControlType.RightStickUp);
			_shootDown.AddDefaultBinding(InputControlType.RightStickDown);
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
