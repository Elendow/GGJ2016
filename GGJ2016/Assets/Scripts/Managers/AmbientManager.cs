using UnityEngine;
using System.Collections;

public class AmbientManager : MonoBehaviour {

	public Color defaultColor;

	public Color[] colores;

	SpriteRenderer spriteRenderer;

	void Awake(){
		spriteRenderer = GetComponent<SpriteRenderer> ();
	}

	public void CambiaAmbiente(int player){
		StartCoroutine (RutinaCambiaAmbiente (colores[player -1]));
	}

	IEnumerator RutinaCambiaAmbiente(Color newColor){
		spriteRenderer.color = newColor;
		yield return new WaitForSeconds (1f);
		spriteRenderer.color = defaultColor;

	}

}
