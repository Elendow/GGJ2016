﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class RecipeManager : MonoBehaviour {


	public List<Item> allItems;
	public List<Recipe> recipes;

//	public List<Item> randomItems;
//	public List<Recipe> randomRecipes;


	// Use this for initialization
	void Start () {
	
		recipes = new List<Recipe>(GetComponentsInChildren<Recipe> ());
		OrganizeRecipes ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	//Cada item lo tengan 3 de los personajes. a excepción del último asignado que lo asignaremos únicamente
	private void OrganizeRecipes(){

		List<Item> randomItems = Shuffle (allItems);
		List<Recipe> randomRecipes = Shuffle(recipes);
		randomItems = Shuffle (allItems);
		randomRecipes = Shuffle(recipes);


		int flag_recipe = 0;

		//Asigno 4 items aleatorios distribuyendolos cada 3
		for (int i_item = 0; i_item <=3; i_item++){
			for (int j_recipe = 0; j_recipe <3; j_recipe++){ //Hacerlo para 3 recetas
				recipes [flag_recipe].AddItem (randomItems [i_item]);
//				Debug.Log("1 Asignado " + randomItems[i_item].Name + " a Player " + (flag_recipe +1));
				flag_recipe++;
				if (flag_recipe > 3) {
					flag_recipe = 0; // Si se pasa de receta vuelve a la primera
				}
			}
		}


		//Los 4 ultimos items los asigno cada 2
		for (int i_item = 4; i_item <=7; i_item++){
			for (int j_recipe = 0; j_recipe <2; j_recipe++){ //Hacerlo para 2 recetas
				recipes [flag_recipe].AddItem (randomItems [i_item]);
//				Debug.Log("2 Asignado " + randomItems[i_item].Name + " a Player " + (flag_recipe +1));
				flag_recipe++;
				if (flag_recipe > 3) { 
					flag_recipe = 0; // Si se pasa de receta vuelve a la primera
				}
			}
		}


	

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


}