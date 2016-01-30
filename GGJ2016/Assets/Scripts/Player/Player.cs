using UnityEngine;
using System.Collections;
using InControl;

public class Player : MonoBehaviour {

	public float speed = 5;
	public int playerNum;
	public GameObject itemPosition;

	private float _repickDelay = 1.5f;
	private Item _item;
	private Item _lastItem;
	private Rigidbody2D _rigidbody;
	private PlayerInput _playerInput;

	void Awake() 
	{
		_rigidbody 		= GetComponent<Rigidbody2D>();

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
		if(_playerInput != null)
		{
			//Movement
			_rigidbody.velocity = new Vector2(_playerInput.move.X * speed, _playerInput.move.Y * speed);

			//Throw item logic
			if(_item != null)
			{
				if(_playerInput.shoot.IsPressed)
				{
					_item.Throw(Mathf.Atan2(_playerInput.shoot.Y, _playerInput.shoot.X) * Mathf.Rad2Deg);
					_lastItem 	= _item;
					_item 		= null;
					itemPosition.SetActive(false);
				}
			}
		}

		//Delay applied in order to not re-pick up an item too fast
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
					_item.Throw(_i.Angle);
					_item = _i;
					_item.PickUp(itemPosition.transform);
				}
			}
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
