using UnityEngine;
using System.Collections;

public class MusicManager : MonoBehaviour {

	public AudioClip clipBGGameplay;

	public AudioSource soundPrefab;

	public AudioClip clipItemCorrecto;
	public AudioClip clipItemIncorrecto;
	public AudioClip clipPickItem;
	public AudioClip clipTiraItem;
	public AudioClip clipEmpuja;
	public AudioClip clipRecogeItem;
	public AudioClip clipCaeLavaItem;
	public AudioClip clipCaeLavaPJ;



	public float pitchVariation = 0.2f;

	public void  playItemCorrecto() { PlaySound (clipItemCorrecto); }
	public void  playItemIncorrecto() { PlaySound (clipItemIncorrecto); }
	public void  playPickItem() { PlaySound (clipPickItem); }
	public void  playTiraItem() { PlaySound (clipTiraItem); }
	public void  playEmpuja() { PlaySound (clipEmpuja); }
	public void  playRecogeItem() { PlaySound (clipRecogeItem); }
	public void  playCaeLavaItem() { PlaySound (clipCaeLavaItem); }
	public void  playCaeLavaPJ() { PlaySound (clipCaeLavaPJ); }

	private AudioSource _audioSource;

	void Start(){
		_audioSource = GetComponent<AudioSource> ();

		GameOverManager goManager = FindObjectOfType<GameOverManager> ();
		goManager.OnGameStart = HandleOnGameStartDelegate;
		goManager.OnGameOver = HandleOnGameOverDelegate;
	}

	void HandleOnGameOverDelegate (int ganador)
	{
		_audioSource.Stop ();
	}

	void HandleOnGameStartDelegate ()
	{
		_audioSource.clip = clipBGGameplay;
		_audioSource.Play ();
	}

	/// <summary>
	/// Plaies the sound.
	/// </summary>
	/// <param name="clip">Clip.</param>
	public void PlaySound(AudioClip clip){

		if (clip != null && soundPrefab != null) {
			
			AudioSource aSource = GameObject.Instantiate (soundPrefab) as AudioSource;
			aSource.clip = clip;
			aSource.PlayOneShot (clip);
			aSource.pitch += Random.Range (-pitchVariation, pitchVariation);
			Destroy (aSource.gameObject, clip.length + 0.5f);
		}

	}
}
