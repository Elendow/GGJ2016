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
		for(int i = 0; i < _controllers.Length; i++)
		{
			if(_controllers[i] != null)
			{
				if(_controllers[i].Action1.IsPressed)
					masks[i].DOPlay();
				else if(_controllers[i].Action4.IsPressed)
					SceneManager.LoadScene("Gameplay");
			}
			else if(Input.GetKeyDown(KeyCode.Return))
				masks[3].DOPlay();
			else if(Input.GetKeyDown(KeyCode.Space))
				SceneManager.LoadScene("Gameplay");
			else if(Input.GetKeyDown(KeyCode.Escape))
				Application.Quit();
		}
	}
}
