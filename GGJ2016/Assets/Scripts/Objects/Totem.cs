using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Totem : MonoBehaviour {

	public int playerNum;
	public SpriteRenderer[] ingredientsIcon;

	private Recipe _recipe;
	private RecipeManager _recipeManager;

	private void Start()
	{
		_recipeManager = GameObject.FindObjectOfType<RecipeManager>();

		if(_recipeManager == null)
			Debug.LogError("Recipe Manager is Missing!");

		_recipe = _recipeManager.recipes[playerNum - 1];
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if(other.gameObject.CompareTag("Item"))
		{
			Item _i = other.GetComponent<Item>();

			if(_recipe.itemsRecipe.Contains(_i.itemID))
			{
				int index = -1;
				for(int i = 0; i < _recipe.itemsRecipe.Count; i++)
				{
					if(_recipe.itemsRecipe[i] == _i.itemID && index == -1)
						index = i;
				}

				_recipe.itemsDone[index] = 1;
				ingredientsIcon[index].color = Color.green;
			}
			else
			{
				List<int> _missingItems = new List<int>();
				for(int i = 0; i < _recipe.itemsRecipe.Count; i++)
				{
					if(_recipe.itemsDone[i] == 0)
						_missingItems.Add(i);
				}

				int _screwItem = _missingItems[Random.Range(0, _missingItems.Count)];
				_recipe.itemsDone[_screwItem] = -1;
				ingredientsIcon[_screwItem].color = Color.red;
			}
		}
	}
}
