using UnityEngine;
using System.Collections;
using InControl;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour {

	public GameObject[] playerTick;
	public int maxPlayer = 2;

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
					playerTick[i].SetActive(true);
				else if(_controllers[i].Action2.IsPressed)
					playerTick[i].SetActive(false);
				else if(_controllers[i].Action4.IsPressed)
					SceneManager.LoadScene("Gameplay");
			}
		}
	}
}
