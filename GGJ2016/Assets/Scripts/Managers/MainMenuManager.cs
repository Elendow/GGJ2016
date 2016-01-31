using UnityEngine;
using System.Collections;
using InControl;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class MainMenuManager : MonoBehaviour {

	public GameObject[] playerTick;
	public int maxPlayer = 2;

	public DOTweenAnimation[] masks;

	private InputDevice[] _controllers;

	private void Start()
	{
		_controllers = new InputDevice[4];
		for(int i = 0; i < InputManager.Devices.Count; i++)
		{
			if(InputManager.Devices[i].IsAttached)
			{
				Debug.Log("Player " + (i+1) + " uses " + InputManager.Devices[i].Name);
				_controllers[i] = InputManager.Devices[i];	
				GameManager.Instance.playerDevices.Add(i);
			}
		}
	}

	private void Update()
	{
		if(Input.GetKeyDown(KeyCode.Alpha1))
			masks[0].DOPlay();
		if(Input.GetKeyDown(KeyCode.Alpha2))
			masks[1].DOPlay();
		if(Input.GetKeyDown(KeyCode.Alpha3))
			masks[2].DOPlay();
		if(Input.GetKeyDown(KeyCode.Alpha4))
			masks[3].DOPlay();
		
		for(int i = 0; i < _controllers.Length; i++)
		{
			if(_controllers[i] != null)
			{
				if(_controllers[i].Action1.IsPressed)
					masks[i].DOPlay();
				//else if(_controllers[i].Action2.IsPressed)
					//playerTick[i].SetActive(false);
				else if(_controllers[i].Action4.IsPressed)
					SceneManager.LoadScene("Gameplay");
			}
		}
	}
}
