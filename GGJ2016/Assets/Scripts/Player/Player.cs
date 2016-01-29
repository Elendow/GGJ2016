using UnityEngine;
using System.Collections;
using InControl;

public class Player : MonoBehaviour {

	public float speed = 5;

	private Rigidbody2D _rigidbody;
	private PlayerInput playerInput;

	void Awake() 
	{
		_rigidbody 	= GetComponent<Rigidbody2D>();
		playerInput = new PlayerInput();
	}
	
	void Update() 
	{
		_rigidbody.velocity = new Vector2(playerInput.move.X * speed, playerInput.move.Y * speed);
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

		//TODO Load values from SaveFile

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
