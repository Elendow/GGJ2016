using UnityEngine;
using System.Collections;

public class MusicManager : MonoBehaviour {

	public GameObject soundPrefab;

	public AudioClip clipItemCorrecto;
	public AudioClip clipItemIncorrecto;
	public AudioClip clipPickItem;
	public AudioClip clipTiraItem;
	public AudioClip clipEmpuja;
	public AudioClip clipRecogeItem;
	public AudioClip clipCaeLavaItem;
	public AudioClip clipCaeLavaPJ;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}


	public void  playItemCorrecto() { PlaySound (clipItemCorrecto); }
	public void  playItemIncorrecto() { PlaySound (clipItemIncorrecto); }
	public void  playPickItem() { PlaySound (clipPickItem); }
	public void  playTiraItem() { PlaySound (clipTiraItem); }
	public void  playEmpuja() { PlaySound (clipEmpuja); }
	public void  playRecogeItem() { PlaySound (clipRecogeItem); }
	public void  playCaeLavaItem() { PlaySound (clipCaeLavaItem); }
	public void  playCaeLavaPJ() { PlaySound (clipCaeLavaPJ); }


	/// <summary>
	/// Plaies the sound.
	/// </summary>
	/// <param name="clip">Clip.</param>
	public void PlaySound(AudioClip clip){

		if (clip != null && soundPrefab != null) {
			GameObject instantiated = GameObject.Instantiate (soundPrefab);
			AudioSource aSource = instantiated.GetComponent<AudioSource> ();
			aSource.clip = clip;
			aSource.PlayOneShot (clip);
			Destroy (instantiated, clip.length + 0.5f);
		}

	}
}
