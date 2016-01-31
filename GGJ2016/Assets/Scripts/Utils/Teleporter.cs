using UnityEngine;
using System.Collections;

public class Teleporter : MonoBehaviour {

	public Transform target;
	
	private void OnTriggerEnter2D(Collider2D other){

		if (!other.CompareTag ("Item")) {
			other.transform.position = target.position;
		}

	}
}
