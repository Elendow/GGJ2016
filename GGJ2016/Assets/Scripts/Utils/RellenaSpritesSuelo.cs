using UnityEngine;
using System.Collections;

public class RellenaSpritesSuelo : MonoBehaviour {

	public Sprite sprite;
	public float variacionPosibleColor = 0.02f;
	public void Build(){



		for (int i = 0; i < 32; i++) {
			for (int j = 0; j < 18; j++) {
				GameObject instantiated = new GameObject ("Suelo");
				instantiated.transform.SetParent (this.transform);
				instantiated.transform.localPosition = new Vector3 (i, j, 0f);
				SpriteRenderer sr = instantiated.AddComponent<SpriteRenderer> ();
				sr.sprite = sprite;
				sr.color = new Color(Random.Range(1f-variacionPosibleColor, 1f),Random.Range(1f-variacionPosibleColor, 1f),Random.Range(1f-variacionPosibleColor, 1f));



			}
		}

	}
}
