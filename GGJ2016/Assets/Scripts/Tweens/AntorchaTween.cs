using UnityEngine;
using System.Collections;
using DG.Tweening;

public class AntorchaTween : MonoBehaviour {

	float delay;
	float duration;
	public float scalation = 2f;

	public float minDelay;
	public float maxDelay;

	public float minDuration;
	public float maxDuration;



	// Use this for initialization
	void Start () {
		delay = Random.Range (minDelay, maxDelay);
		duration = Random.Range (minDuration, maxDuration);

		transform.DOScale (transform.localScale.x *scalation, duration).SetDelay (delay).SetLoops (-1, LoopType.Yoyo).SetEase(Ease.InOutFlash);





	}
	

}
