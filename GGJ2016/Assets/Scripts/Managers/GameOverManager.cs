using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using DG.Tweening;
using InControl;
using UnityEngine.SceneManagement;


public class GameOverManager : MonoBehaviour {

	public delegate void OnGameStartDelegate();
	public delegate void OnGameOverDelegate(int ganador);

	public OnGameStartDelegate OnGameStart = delegate {};
	public OnGameOverDelegate OnGameOver = delegate {};

	public Text txtGameOver;
	public Text txtCountdown;

	private bool canRestart = false;

	void Start(){
		GameManager.Instance.isInGame = false;
		GameStart ();
	}

	void Update(){
		if (canRestart) {
			if (InputManager.ActiveDevice.AnyButton) {
				SceneManager.LoadScene ("Gameplay");

			}
		}

	}


	public void GameOver(){
		GameManager.Instance.isInGame = false;
		EvaluaPuntuaciones ();
		canRestart = true;
	}

	public void GameStart(){
		
		StartCoroutine (RutinaStartGame ());
	}

	IEnumerator RutinaStartGame(){
		txtGameOver.gameObject.SetActive (false);
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
		txtCountdown.DOFade (1f, 0f);
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
			string color = "";

			if(ganador == 1) color = "<color='red'>";
			else if(ganador == 2) color = "<color='green'>";
			else if(ganador == 3) color = "<color='purple'>";
			else if(ganador == 4) color = "<color='blue'>";

			txtGameOver.text = color +"PLAYER " + ganador + " WINS</color>";
		}
		txtGameOver.gameObject.SetActive (true);
		OnGameOver (ganador);
		 

	}
}
