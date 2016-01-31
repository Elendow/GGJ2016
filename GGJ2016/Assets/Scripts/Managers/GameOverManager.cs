using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using DG.Tweening;
public class GameOverManager : MonoBehaviour {

	public delegate void OnGameStartDelegate();
	public delegate void OnGameOverDelegate(int ganador);

	public OnGameStartDelegate OnGameStart = delegate {};
	public OnGameOverDelegate OnGameOver = delegate {};

	public Text txtGameOver;
	public Text txtCountdown;

	void Start(){
		GameManager.Instance.isInGame = false;
		GameStart ();
	}


	public void GameOver(){
		GameManager.Instance.isInGame = false;
		EvaluaPuntuaciones ();
	}

	public void GameStart(){
		
		StartCoroutine (RutinaStartGame ());
	}

	IEnumerator RutinaStartGame(){

		txtCountdown.text = "3";
		txtCountdown.gameObject.SetActive (true);
		yield return new WaitForSeconds (1);
		txtCountdown.text = "2";
		yield return new WaitForSeconds (1);
		txtCountdown.text = "1";
		OnGameStart (); //Aqui para que empiece la musica y todo se prepare
		yield return new WaitForSeconds (1);
		txtCountdown.text = "GO";
		txtCountdown.DOFade (0f, 2f);
		Vector3 prevScale = txtCountdown.rectTransform.localScale;
		txtCountdown.rectTransform.DOScale(prevScale * 2f ,2f);
		yield return new WaitForSeconds (1);


		yield return new WaitForSeconds (1);
		txtCountdown.gameObject.SetActive (false);
		txtCountdown.rectTransform.localScale = prevScale;
		GameManager.Instance.isInGame = true;


	}

	void EvaluaPuntuaciones(){

		List<Totem> totemsOrdenados = new List<Totem> (FindObjectsOfType<Totem> ());
		totemsOrdenados.Sort((t1, t2) => t2.score.CompareTo(t1.score));
		int ganador;
	
		if (totemsOrdenados [0].score == totemsOrdenados [1].score) {
			//Empate
			ganador = -1;
			txtGameOver.text = "NO WINNER. EVERYBODY IS DEAD";

		} else {
			//El ganador es totemsOrdenados [0].playerNum
			ganador = totemsOrdenados [0].playerNum;
			txtGameOver.text = "PLAYER " + ganador + "WINS";
		}
		txtGameOver.gameObject.SetActive (true);
		OnGameOver (ganador);
		 

	}
}
