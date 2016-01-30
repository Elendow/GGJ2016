using UnityEngine;
using System.Collections;

public class Item : MonoBehaviour 
{
	public string itemName;
	public int itemID;

	private float _angle;
	private bool _isThrown;
	private Rigidbody2D _rigidbody;
	private Collider2D _collider;

	private void Awake() 
	{
		_rigidbody 	= GetComponent<Rigidbody2D>();
		_collider 	= GetComponent<Collider2D>();
	}

	public void Update()
	{
		if(Mathf.Abs(_rigidbody.velocity.x) < 0.2f || Mathf.Abs(_rigidbody.velocity.y) < 0.2f)
			_isThrown = true;
	}

	public void PickUp(Transform parent)
	{
		transform.parent  		= parent;
		transform.localPosition	= Vector2.zero;
		_collider.enabled 		= false;
		_rigidbody.isKinematic 	= true;
	}

	public void Throw(float angle, float force)
	{
		Vector2 forward;
		float angleMagnitud;
		_isThrown 				= true;
		_angle 					= angle;
		forward 				= new Vector2(Mathf.Cos(_angle), Mathf.Sin(_angle));
		transform.parent  		= null;
		_collider.enabled 		= true;
		_rigidbody.isKinematic 	= false;
		_rigidbody.AddForce(forward * force);

		Debug.Log(itemName + " is thrown. Angle " + angle);
	}

	public bool IsThrown
	{
		get { return _isThrown; }
	}

	public float Angle
	{
		get { return _angle; }
	}
}
