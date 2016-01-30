using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Recipe {

	public List<int> itemsRecipe;
	public List<int> itemsDone; //-1 bad, 0 none, 1 good

	public void Initialize()
	{
		itemsRecipe = new List<int>();
		itemsDone = new List<int>();
	}

	public void AddItem(int itemID){
		if (itemsRecipe == null){
			itemsRecipe = new List<int>();
			itemsDone = new List<int>();
		}

		itemsDone.Add(0);
		itemsRecipe.Add(itemID);
	}
}
