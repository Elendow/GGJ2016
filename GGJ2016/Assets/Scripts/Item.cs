using UnityEngine;
using System.Collections;

public class Item : MonoBehaviour {

	private Rigidbody2D _rigidbody;
	private Collider2D _collider;

	private void Awake() 
	{
		_rigidbody 	= GetComponent<Rigidbody2D>();
		_collider 	= GetComponent<Collider2D>();
	}

	public void PickUp(Transform parent)
	{
		transform.parent  		= parent;
		_collider.enabled 		= false;
		_rigidbody.isKinematic 	= true;
	}

	public void Throw(float angle)
	{
		Vector2 forward;
		float angleMagnitud;

		transform.Rotate(new Vector3(0,0,angle));

		angleMagnitud 			= transform.eulerAngles.magnitude * Mathf.Deg2Rad;
		forward 				= new Vector2(Mathf.Cos(angleMagnitud), Mathf.Sin(angleMagnitud));
		transform.parent  		= null;
		_collider.enabled 		= true;
		_rigidbody.isKinematic 	= false;

		_rigidbody.AddForce(forward * 50);
	}
}
