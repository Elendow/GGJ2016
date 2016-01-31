﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class RecipeManager : MonoBehaviour {

	public List<Item> allItems;
	public List<Recipe> recipes;
	public List<Spawner> spawners;

	public AnimationCurve balanceTiempoEspera;

	private float timeStarted;

	void Start(){
		GameOverManager goManager = FindObjectOfType<GameOverManager> ();
		goManager.OnGameStart = HandleOnGameStartDelegate;
		goManager.OnGameOver = HandleOnGameOverDelegate;
	}

	void HandleOnGameOverDelegate (int ganador)
	{
		
	}

	void HandleOnGameStartDelegate ()
	{
		timeStarted = Time.time;
	}

	private void Awake() 
	{

		spawners = new List<Spawner>(FindObjectsOfType<Spawner> ());
		recipes = new List<Recipe>();
		for(int i = 0; i < 4; i++)
			recipes.Add(new Recipe());

		for(int i = 0; i < 4; i++)
			recipes[i].Initialize();
		
		OrganizeRecipes();

		//Comentar esto para activarlo cuando vengamos del menu
		//GameManager.Instance.isInGame = true;

		StartCoroutine(RoutineSpawn());

	}

	//Cada item lo tengan 3 de los personajes. a excepción del último asignado que lo asignaremos únicamente
	private void OrganizeRecipes()
	{
		List<Item> randomItems = Shuffle (allItems);
		randomItems = Shuffle (allItems);

		int flag_recipe = 0;

		//Asigno 4 items aleatorios distribuyendolos cada 3
		for (int i_item = 0; i_item <=3; i_item++){
			for (int j_recipe = 0; j_recipe <3; j_recipe++){ //Hacerlo para 3 recetas
				recipes [flag_recipe].AddItem (randomItems[i_item].itemID, randomItems[i_item].IconSprite);
				//Debug.Log("1 Asignado " + randomItems[i_item].Name + " a Player " + (flag_recipe +1));
				flag_recipe++;
				if (flag_recipe > 3) {
					flag_recipe = 0; // Si se pasa de receta vuelve a la primera
				}
			}
		}

		//Los 4 ultimos items los asigno a los restantes
		for (int i_item = 4; i_item <=7; i_item++){
			recipes [i_item - 4].AddItem (randomItems[i_item].itemID, randomItems[i_item].IconSprite);
		}

		recipes = Shuffle(recipes);
	}

	private List<T> Shuffle<T>(List<T> list) {
		int n = list.Count;
		System.Random rnd = new System.Random();
		while (n > 1) {
			int k = (rnd.Next(0, n) % n);
			n--;
			T value = list[k];
			list[k] = list[n];
			list[n] = value;
		}
		return list;
	}


	public void SpawnItems(){
		List<Spawner> spawnerLibres = spawners.FindAll(s => s.itsFree);
		if (spawnerLibres.Count > 0) {
			InstanciateItemInSpawner (allItems [Random.Range (0, allItems.Count)], spawnerLibres [Random.Range (0, spawnerLibres.Count)]);
		}
	}

	public void SpawnItemsInitial(){
		//Coge los iniciales
		List<Spawner> spawnerIniciales = spawners.FindAll(s => s.initialSpawner);
		foreach (Spawner spawner in spawnerIniciales) {
			InstanciateItemInSpawner (allItems [Random.Range (0, allItems.Count)], spawner);
		}
	}

	private void InstanciateItemInSpawner(Item item, Spawner spawner){
		GameObject instantiatedItem = GameObject.Instantiate (item.gameObject);
		instantiatedItem.transform.position = spawner.transform.position;
		item.spawnerOcupado = spawner;
		spawner.itsFree = false;

	}


	IEnumerator RoutineSpawn(){
		
		SpawnItemsInitial ();
		yield return new WaitForSeconds (balanceTiempoEspera.Evaluate(Time.time - timeStarted));

		while (GameManager.Instance.isInGame) {
			SpawnItems ();

			yield return new WaitForSeconds (balanceTiempoEspera.Evaluate(Time.time - timeStarted));
				
		}
	}


}
