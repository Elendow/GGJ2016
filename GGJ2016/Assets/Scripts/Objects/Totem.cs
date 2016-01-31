using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public class Totem : MonoBehaviour {

	public int playerNum;
	public Image[] ingredientsBackground;
	public Image[] ingredientsIcon;
	public Text scoreText;

	private int _score = 0;
	private Recipe _recipe;
	private RecipeManager _recipeManager;

	public DOTweenAnimation TweenOrb;

	MusicManager _musicManager;

	private void Start()
	{
		_musicManager =	GameObject.FindObjectOfType<MusicManager> ();
		_recipeManager = GameObject.FindObjectOfType<RecipeManager>();

		if(_recipeManager == null)
			Debug.LogError("Recipe Manager is Missing!");

		_recipe = _recipeManager.recipes[playerNum - 1];

		for(int i = 0; i < _recipe.itemsRecipe.Count; i++)
		{
			ingredientsIcon[i].sprite = _recipe.itemsSprites[i];
		}

		CalculateScore();
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
					if(_recipe.itemsRecipe[i] == _i.itemID && _index == -1 && _recipe.itemsDone[i] == 0)
						_index = i;
				}

				if(_index != -1)
				{
					_recipe.itemsDone[_index] = 1;
					ingredientsBackground[_index].color = Color.green;
					ingredientsBackground[_index].GetComponent<DOTweenAnimation> ().DORestart ();
					_musicManager.playItemCorrecto ();
				}
				else
				{
					BadItem();
				}
				other.gameObject.SetActive(false);
			}
			else
			{
				BadItem();
			}
			CalculateScore();
			other.gameObject.SetActive(false);	
			TweenOrb.DORestart();
		}
	}

	private void BadItem()
	{
		List<int> _missingItems = new List<int>();
		for(int i = 0; i < _recipe.itemsRecipe.Count; i++)
		{
			if(_recipe.itemsDone[i] == 0)
				_missingItems.Add(i);
		}

		if(_missingItems.Count > 0)
		{
			int _screwItem = _missingItems[Random.Range(0, _missingItems.Count)];
			_recipe.itemsDone[_screwItem] = -1;
			ingredientsBackground[_screwItem].color = Color.red;
			ingredientsBackground[_screwItem].GetComponent<DOTweenAnimation>().DORestart();
		}

		_musicManager.playItemIncorrecto ();
	}

	private void CalculateScore()
	{
		_score = 0;

	for(int i = 0; i < _recipe.itemsDone.Count; i++)
		{
			_score += _recipe.itemsDone[i];
		}

		scoreText.text = _score.ToString();
	}
}
