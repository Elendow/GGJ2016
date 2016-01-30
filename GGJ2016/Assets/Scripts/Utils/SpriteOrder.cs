/*
This script sorts the sprite using the Y as a reference
*/
using UnityEngine;
using System.Collections;

public class SpriteOrder : MonoBehaviour {

	public bool update = false;

	private SpriteRenderer sp;
	private int i;

	private void Start () 
	{
		sp = GetComponent<SpriteRenderer>();
		i = sp.sortingOrder;
		Sort();
	}
	
	private void Update ()
	{
		if(update)
			Sort();
		else
			enabled = false;
	}

	private void Sort()
	{
		if(sp != null)
		{
			sp.sortingOrder = i + -500 - (int)(sp.bounds.min.y * 10);
		}
	}
}
