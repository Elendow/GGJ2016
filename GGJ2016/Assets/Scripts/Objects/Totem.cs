using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Com.LuisPedroFonseca.ProCamera2D;


public class Totem : MonoBehaviour {



	public int playerNum;
	public Image[] ingredientsBackground;
	public Image[] ingredientsIcon;
	public Text scoreText;

	public int score = 0;
	private Recipe _recipe;
	private RecipeManager _recipeManager;
	private AmbientManager _ambientManager;

	public DOTweenAnimation TweenOrb;

	MusicManager _musicManager;

	GameOverManager _gameOverManager;
	ProCamera2DShake _shaker;

	private void Start()
	{
		_musicManager =	GameObject.FindObjectOfType<MusicManager> ();
		_recipeManager = GameObject.FindObjectOfType<RecipeManager>();
		_ambientManager = GameObject.FindObjectOfType<AmbientManager>();
		_gameOverManager = GameObject.FindObjectOfType<GameOverManager>();
		_shaker = Camera.main.GetComponent<ProCamera2DShake> ();

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
			Item _i = other.transform.parent.GetComponent<Item>();

			if(_i.IsPickedUp)
			{
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
						_ambientManager.CambiaAmbiente (playerNum);
					}
					else
					{
						BadItem();
					}
					other.transform.parent.gameObject.SetActive(false);
				}
				else
				{
					BadItem();
				}
				CalculateScore();
				other.transform.parent.gameObject.SetActive(false);
				TweenOrb.DORestart();
				_shaker.Shake ();

				if (!_recipe.itemsDone.Contains (0)) {
					_gameOverManager.GameOver ();
				}
			}
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
		score = 0;


	for(int i = 0; i < _recipe.itemsDone.Count; i++)
		{
			score += _recipe.itemsDone[i];
		}

		scoreText.text = score.ToString();


	}

}
