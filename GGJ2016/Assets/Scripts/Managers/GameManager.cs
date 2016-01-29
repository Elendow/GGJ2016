using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using InControl;

public class GameManager
{
	#region Singleton
	private static GameManager _instance;

	public static GameManager Instance{
		get {
			if(_instance == null)
				_instance = new GameManager(); 
			return _instance; 
		}
	}
	#endregion

	public List<InputDevice> playerDevices;

	public GameManager()
	{
		playerDevices = new List<InputDevice>();
	}
}
