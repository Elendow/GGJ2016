using UnityEngine;
using System.Collections;

public class Teleporter : MonoBehaviour {

	public Transform target;
	
	private void OnTriggerEnter2D(Collider2D other){
		other.transform.position = target.position;
	}
}
