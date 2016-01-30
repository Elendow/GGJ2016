using UnityEngine;
using System.Collections;
using InControl;

public class Player : MonoBehaviour {

	public float speed = 5;
	public int playerNum;

	private float _repickDelay = 1.5f;
	private Item _item;
	private Item _lastItem;
	private Rigidbody2D _rigidbody;
	private PlayerInput _playerInput;

	void Awake() 
	{
		_rigidbody 		= GetComponent<Rigidbody2D>();
		_playerInput 	= new PlayerInput();

		if(GameManager.Instance.playerDevices.Count > playerNum - 1)
		{
			_playerInput.Device = InputManager.Devices[GameManager.Instance.playerDevices[playerNum - 1]];
			Debug.Log(playerNum + " " + _playerInput.Device.Name);
		}
		else
		{
			//SOLO PARA TESTEO!
			_playerInput.Device = InputManager.Devices[playerNum - 1];
			Debug.LogWarning("No input for player " + playerNum);
		}
	}
	
	void Update() 
	{
		if(_playerInput != null)
		{
			_rigidbody.velocity = new Vector2(_playerInput.move.X * speed, _playerInput.move.Y * speed);

			if(_item != null)
			{
				if(_playerInput.shoot.IsPressed)
				{
					_item.Throw(Mathf.Atan2(_playerInput.shoot.Y, _playerInput.shoot.X) * Mathf.Rad2Deg);
					_lastItem 	= _item;
					_item 		= null;
				}
			}
		}

		if(_lastItem != null)
		{
			_repickDelay -= Time.deltaTime;
			if(_repickDelay < 0)
			{
				_repickDelay 	= 1.5f;
				_lastItem 		= null;
			}
		}
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if(other.gameObject.CompareTag("Item"))
		{
			_item = other.GetComponent<Item>();
			if(_lastItem != _item)
				_item.PickUp(transform);
		}
	}
}

public class PlayerInput : PlayerActionSet
{ 
	public PlayerTwoAxisAction move;
	public PlayerTwoAxisAction shoot;

	private PlayerAction _left, _right, _up, _down;
	private PlayerAction _shootLeft, _shootRight, _shootUp, _shootDown;

	public PlayerInput()
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

		shoot.LowerDeadZone = 0.5f;

		//Keyboard
		_left.AddDefaultBinding(Key.LeftArrow);
		_right.AddDefaultBinding(Key.RightArrow);
		_up.AddDefaultBinding(Key.UpArrow);
		_down.AddDefaultBinding(Key.DownArrow);

		_shootLeft.AddDefaultBinding(Key.A);
		_shootRight.AddDefaultBinding(Key.D);
		_shootUp.AddDefaultBinding(Key.W);
		_shootDown.AddDefaultBinding(Key.S);

		//Gamepad
		_left.AddDefaultBinding(InputControlType.LeftStickLeft);
		_right.AddDefaultBinding(InputControlType.LeftStickRight);
		_up.AddDefaultBinding(InputControlType.LeftStickUp);
		_down.AddDefaultBinding(InputControlType.LeftStickDown);

		_shootLeft.AddDefaultBinding(InputControlType.RightStickLeft);
		_shootRight.AddDefaultBinding(InputControlType.RightStickRight);
		_shootUp.AddDefaultBinding(InputControlType.RightStickUp);
		_shootDown.AddDefaultBinding(InputControlType.RightStickDown);
	}
}
