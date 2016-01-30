using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Recipe : MonoBehaviour {


	public List<Item> itemsRecipe;

	void Start(){

		itemsRecipe = new List<Item>();

	}

	public void AddItem(Item item){

		if (itemsRecipe == null){
			itemsRecipe = new List<Item>();
		}

		itemsRecipe.Add (item);
	}

}
