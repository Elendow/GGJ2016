using UnityEngine;
using System.Collections;

public class CameraManager : MonoBehaviour {

	public Camera[] cameras;

	float newAspectRatio = 1.7777777777f;

	private void Start()
	{
		for(int i = 0; i < cameras.Length; i++)
		{
			float difference = Mathf.Abs(newAspectRatio - cameras[i].aspect);
			if (difference > 0.01f) 
				cameras[i].rect = new Rect (0, difference * 0.5f, 1, (1 - difference));
		}
	}
}
