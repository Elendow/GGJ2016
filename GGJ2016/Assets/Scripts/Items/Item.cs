using UnityEngine;
using System.Collections;
using DG.Tweening;

public class Item : MonoBehaviour 
{
	public string itemName;
	public int itemID;

	private float _angle;
	private bool _isThrown;
	private Rigidbody2D _rigidbody;
	private Collider2D _collider;
	private SpriteRenderer _sp;

	private void Awake() 
	{
		_rigidbody 	= GetComponent<Rigidbody2D>();
		_collider 	= GetComponent<Collider2D>();
		_sp 		= GetComponent<SpriteRenderer>();
	}

	public void Update()
	{
		if(Mathf.Abs(_rigidbody.velocity.x) < 0.1f && Mathf.Abs(_rigidbody.velocity.y) < 0.1f)
			_isThrown = false;
	}

	public void PickUp(Transform parent)
	{
		transform.parent  		= parent;
		transform.localPosition	= Vector2.zero;
		_collider.enabled 		= false;
		_rigidbody.isKinematic 	= true;
		_sp.sortingLayerName	= "UI";
	}

	public void Throw(float angle, float force)
	{
		Vector2 forward;
		_isThrown 				= true;
		_angle 					= angle;
		forward 				= new Vector2(Mathf.Cos(_angle), Mathf.Sin(_angle));
		transform.parent  		= null;
		_collider.enabled 		= true;
		_rigidbody.isKinematic 	= false;
		_rigidbody.AddForce(forward * force);
		_sp.sortingLayerName	= "Default";

		Debug.Log(itemName + " is thrown. Angle " + angle);
	}

	private void OnTriggerStay2D(Collider2D other)
	{
		if(other.gameObject.CompareTag("Lava"))
		{
			if(!_isThrown)
			{
				transform.DOScale(Vector3.zero, 0.5f);
				transform.DORotate(new Vector3(0,0,200), 0.5f);
				_rigidbody.velocity = Vector2.zero;
				transform.DOMove(other.gameObject.transform.position + (other.gameObject.transform.localScale * 0.5f), 0.5f);
				_rigidbody.velocity *= 0.5f;
				_collider.enabled 	= false;
				Destroy(gameObject, 1f);
			}
		}
	}

	public bool IsThrown
	{
		get { return _isThrown; }
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
