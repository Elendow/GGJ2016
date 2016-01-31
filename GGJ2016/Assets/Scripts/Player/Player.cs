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
	public GameObject pushParticles;
	public GameObject lavaParticles;
	public GameObject deadParticles;
	public GameObject respawnParticles;

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
			else if(playerNum == 4)
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
		if(GameManager.Instance.isInGame)
		{
			if(_alive)
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
								Vector3 dir =  new Vector3(Mathf.Cos(angle), Mathf.Sin(angle));
								RaycastHit2D hit = Physics2D.Raycast(transform.position, dir.normalized, pushDistance);

								if (hit.collider != null) 
								{
									if(hit.collider.gameObject.CompareTag("Player"))
									{
										float distance = Vector2.Distance(transform.position, hit.point);
										hit.collider.transform.DOScale (hit.collider.transform.localScale * 1.5f, 0.2f).From ();

										if (distance <= pushDistance) {
											hit.collider.gameObject.GetComponent<Player> ().Push (angle);
											GameObject goPart = Instantiate (pushParticles, hit.point, Quaternion.identity) as GameObject;
											Destroy (goPart, 3f);

										} 

										this.ReBounce (angle);
									}
								}
							}
						}
						_rigidbody.velocity = _rigidbody.velocity / 2;
					}

					if(_force.x > 0.2f)
						_force.x -= 0.1f;
					else if(_force.x < -0.2f)
						_force.x += 0.1f;
					else
						_force.x = 0;
					
					if(_force.y > 0.2f)
						_force.y -= 0.1f;
					else if(_force.y < -0.2f)
						_force.y += 0.1f;
					else
						_force.y = 0;
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
					Instantiate(respawnParticles, transform.position, Quaternion.identity);
				}
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
		Vector2 forward = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)).normalized;
		_force = forward * speed;
	}

	private void ReBounce(float angle)
	{
		if(_item != null)
		{
			_item.Throw(angle, throwForce);
			_lastItem 	= _item;
			_item 		= null;
		}
		Vector2 forward = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)).normalized;
		_force = forward * (-speed /3f);
	}

	private void HandleGameOver(int ganador){
		if (playerNum != ganador) {
			Instantiate(deadParticles, transform.position, Quaternion.identity);
			_velocity = Vector2.zero;
			_force = Vector2.zero;
			gameObject.SetActive(false);
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

					_item.transform.DOScale (_item.transform.localScale * 1.5f, 0.2f).From ();

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
					_item.transform.DOScale (_item.transform.localScale * 1.5f, 0.2f).From ();
					Vector2 forward = new Vector2(Mathf.Cos(_item.Angle), Mathf.Sin(_item.Angle));
					_force += forward * (speed + 4);
				}
			}
		}
		else if(other.gameObject.CompareTag("Lava"))
		{
			Dead();
			_rigidbody.velocity = Vector2.zero;
			transform.DOMove(other.gameObject.transform.position + (other.gameObject.transform.localScale * 0.5f), 0.5f).OnComplete(LavaFall);
		}
	}

	private void LavaFall()
	{
		Destroy(Instantiate(lavaParticles, transform.position, Quaternion.identity), 2f);
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
