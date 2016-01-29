using UnityEngine;
using System.Collections;
using InControl;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour {

	public GameObject[] playerTick;
	public int maxPlayer = 2;

	int index = 0;
	PlayerInputSelector playerInputSel;

	private void Start()
	{
		playerInputSel 	= new PlayerInputSelector();
	}

	private void Update()
	{
		if(index < maxPlayer)
		{
			if(playerInputSel.accept.IsPressed && !GameManager.Instance.playerDevices.Contains(playerInputSel.Device))
			{
				GameManager.Instance.playerDevices.Add(playerInputSel.Device);
				playerTick[index].SetActive(true);
				index++;
			}
		}
		else
		{
			if(playerInputSel.accept)
				SceneManager.LoadScene("Gameplay");

			if(index >= 0 && playerInputSel.cancel.IsPressed)
			{
				index--;
				playerTick[index].SetActive(true);
			}
		}
	}
}

public class PlayerInputSelector : PlayerActionSet
{
	public PlayerAction accept, cancel;

	public PlayerInputSelector()
	{
		accept = CreatePlayerAction("Accept");
		cancel = CreatePlayerAction("Cancel");

		accept.AddDefaultBinding(InputControlType.Action1);
		cancel.AddDefaultBinding(InputControlType.Action2);
	}
}
