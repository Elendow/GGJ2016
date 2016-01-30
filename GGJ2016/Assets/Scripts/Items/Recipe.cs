using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Recipe {

	public List<int> itemsRecipe;
	public List<int> itemsDone; //-1 bad, 0 none, 1 good
	public List<Sprite> itemsSprites;

	public void Initialize()
	{
		itemsRecipe 	= new List<int>();
		itemsDone 		= new List<int>();
		itemsSprites 	= new List<Sprite>();
	}

	public void AddItem(int itemID, Sprite itemIcon){
		if (itemsRecipe == null){
			itemsRecipe 	= new List<int>();
			itemsDone 		= new List<int>();
			itemsSprites 	= new List<Sprite>();
		}

		itemsDone.Add(0);
		itemsRecipe.Add(itemID);
		itemsSprites.Add(itemIcon);
	}
}
