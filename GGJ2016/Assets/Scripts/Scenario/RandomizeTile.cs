using UnityEngine;
using System.Collections;

public class RandomizeTile : MonoBehaviour {


	//public bool randomRotation = false;
	public float randomColorness = 0f;
	public float randomMovementX = 0f;
	public float randomMovementY = 0f;
	public float randomScale = 0f;

	// Use this for initialization
	void Start () {
	//	if (randomRotation) { RandomizeRotation ();}
		if (randomColorness > 0f) { RandomizeColor ();}
		if (randomColorness > 0f) { RandomizePositionX ();}
		if (randomScale > 0f) { RandomizeScale ();}
	}


	void RandomizeRotation(){



	}

	void RandomizeColor(){
		SpriteRenderer sr = GetComponent<SpriteRenderer> ();
		sr.color = new Color(Random.Range(1f-randomColorness, 1f),Random.Range(1f-randomColorness, 1f),Random.Range(1f-randomColorness, 1f));
	}

	void RandomizePositionX(){
		this.transform.localPosition = new Vector3 (this.transform.localPosition.x + Random.Range (-randomMovementX, randomMovementX), this.transform.localPosition.y, this.transform.localPosition.z); 
	}
	void RandomizePositionY(){
		this.transform.localPosition = new Vector3 (this.transform.localPosition.x, this.transform.localPosition.y + + Random.Range (-randomMovementY, randomMovementY), this.transform.localPosition.z); 
	}

	void RandomizeScale(){
		this.transform.localScale = new Vector3 (this.transform.localScale.x + Random.Range (0.05f, randomScale), this.transform.localScale.y + Random.Range (0.05f, randomScale) , this.transform.localScale.z); 
	}

}
