using UnityEngine;
using System.Collections;
using DG.Tweening;

public class Item : MonoBehaviour 
{
	public string itemName;
	public int itemID;
	public GameObject lavaParticles;

	private float _angle;
	private bool _isThrown;
	private bool _pickedUp = false;
	private Rigidbody2D _rigidbody;
	private Collider2D[] _colliders;

	private SpriteRenderer _sp;


	public int spawnerOcupado;
	private void Awake() 
	{
		_rigidbody 		= GetComponent<Rigidbody2D>();
		_colliders		= GetComponentsInChildren<Collider2D>() ;
		_sp 			= GetComponent<SpriteRenderer>();
	}

	public void Update()
	{
		if(Mathf.Abs(_rigidbody.velocity.x) < 0.2f && Mathf.Abs(_rigidbody.velocity.y) < 0.2f)
			_isThrown = false;
	}

	public void PickUp(Transform parent)
	{
		Debug.Log(itemName + " is pikcup. IsThrown " + _isThrown);

		transform.parent  		= parent;
		transform.localPosition	= Vector2.zero;
		for (int i = 0; i < _colliders.Length; i++) {
			_colliders [i].enabled = false;
		}
		_rigidbody.isKinematic 	= true; 
		_rigidbody.velocity		= Vector2.zero;
		_sp.sortingLayerName	= "UI";
		_isThrown				= false;
		_pickedUp				= true;
	}

	public void Throw(float angle, float force)
	{
		Vector2 forward;
		_isThrown 				= true;
		_angle 					= angle;
		forward 				= new Vector2(Mathf.Cos(_angle), Mathf.Sin(_angle));
		transform.parent  		= null;
		for (int i = 0; i < _colliders.Length; i++) {
			_colliders [i].enabled = true;
		}
		_rigidbody.velocity 	= Vector2.zero;
		_rigidbody.isKinematic 	= false;
		_rigidbody.AddForce(forward * force);
		_sp.sortingLayerName	= "Default";
		_pickedUp				= false;

		Debug.Log(itemName + " is thrown. Angle " + angle);
	}

	private void OnTriggerStay2D(Collider2D other)
	{
		if(other.gameObject.CompareTag("Lava"))
		{
			if(!_isThrown && !_pickedUp)
			{
				_pickedUp = true;
				_rigidbody.velocity = Vector2.zero;
				transform.DOScale(Vector3.zero, 1f);
				transform.DORotate(new Vector3(0,0,200), 1f);
				transform.DOMove(other.gameObject.transform.position + (other.gameObject.transform.localScale * 0.5f), 1f).OnComplete(LavaFall);
				for (int i = 0; i < _colliders.Length; i++) 
				{
					_colliders[i].enabled = false;
				}
				Destroy (gameObject, 1.1f);
			}
		}
	}

	private void LavaFall()
	{
		Instantiate(lavaParticles, transform.position, Quaternion.identity);
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if(other.gameObject.CompareTag("Paredes"))
		{
			_rigidbody.velocity = Vector2.zero;
		}
	}

	public bool IsThrown
	{
		get { return _isThrown; }
	}

	public bool IsPickedUp
	{
		get { return _pickedUp; }
	}

	public float Angle
	{
		get { return _angle; }
	}

	public Sprite IconSprite
	{
		get { return GetComponent<SpriteRenderer>().sprite; }
	}
}
