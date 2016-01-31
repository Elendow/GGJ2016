using UnityEngine;
using System.Collections;

public class SpriteAnimator : MonoBehaviour {

	public float frameRate;
	public Sprite[] spriteList;

	private float _timePerFrame;
	private int _index = 0;
	private float _timer = 0f;
	private SpriteRenderer _sp;

	private void Awake()
	{
		_sp = GetComponent<SpriteRenderer>();
		_timePerFrame = 1f / frameRate;
	}

	private void Update() 
	{
		_timer += Time.deltaTime;
		_timePerFrame =  1 / frameRate;

		if(_timer > _timePerFrame)
		{
			_index++;
			if(_index >= spriteList.Length)
				_index = 0;
			
			_sp.sprite = spriteList[_index];
			_timer = 0;
		}
	}
}
