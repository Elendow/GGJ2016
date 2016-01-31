using UnityEngine;
using System.Collections;

public class FallingMask : MonoBehaviour {

	public float finalPos;
	private int _done = 3;
	private AudioSource _audio;

	private void Awake()
	{
		_audio = GetComponent<AudioSource>();
	}

	private void Update()
	{
		if(transform.localPosition.y > finalPos - 2 && transform.localPosition.y < finalPos + 2  && _done > 0)
		{
			Debug.Log("sound");
			_done--;
			PlaySound(_audio.clip, 0.8f);
		}
	}


	public void PlaySound(AudioClip a, float volumeFX){
		AudioSource fx 	= gameObject.AddComponent<AudioSource>();
		fx.minDistance 	= 1;
		fx.maxDistance 	= 2;
		fx.pitch		= Random.Range(0.8f,1.2f);
		fx.volume 		= volumeFX;
		fx.PlayOneShot(a, volumeFX);
		Destroy (fx, a.length);
	}
}
