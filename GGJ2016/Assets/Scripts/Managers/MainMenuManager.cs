using UnityEngine;
using System.Collections;
using InControl;

public class MainMenuManager : MonoBehaviour {

	bool[] playerReady;
	PlayerInputSelector playerInputSel;

	private void Start()
	{
		playerReady = new bool[4]{false,false,false,false};
		playerInputSel = new PlayerInputSelector();
	}
}

public class PlayerInputSelector : PlayerActionSet
{

}
