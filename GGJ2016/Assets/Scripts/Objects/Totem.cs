using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Totem : MonoBehaviour {

	public int playerNum;
	public SpriteRenderer[] ingredientsBackground;
	public SpriteRenderer[] ingredientsIcon;

	private Recipe _recipe;
	private RecipeManager _recipeManager;

	private void Start()
	{
		_recipeManager = GameObject.FindObjectOfType<RecipeManager>();

		if(_recipeManager == null)
			Debug.LogError("Recipe Manager is Missing!");

		_recipe = _recipeManager.recipes[playerNum - 1];

		for(int i = 0; i < _recipe.itemsRecipe.Count; i++)
		{
			ingredientsIcon[i].sprite = _recipe.itemsSprites[i];
		}
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if(other.gameObject.CompareTag("Item"))
		{
			Item _i = other.GetComponent<Item>();

			if(_recipe.itemsRecipe.Contains(_i.itemID))
			{
				int _index = -1;
				for(int i = 0; i < _recipe.itemsRecipe.Count; i++)
				{
					if(_recipe.itemsRecipe[i] == _i.itemID && _index == -1)
						_index = i;
				}

				_recipe.itemsDone[_index] = 1;
				ingredientsBackground[_index].color = Color.green;
				other.gameObject.SetActive(false);
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
				ingredientsBackground[_screwItem].color = Color.red;
				ingredientsIcon[_screwItem].sprite = null;
				other.gameObject.SetActive(false);
			}
		}
	}
}
