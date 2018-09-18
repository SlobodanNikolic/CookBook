using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Recipe{

	[SerializeField]
	public string title;
	[SerializeField]
	public string recipeImage;
	[SerializeField]
	public Dictionary<string, string> ingredients;
	[SerializeField]
	public List<string> tags;
	[SerializeField]
	public int rating;
	[SerializeField]
	public Dictionary<string, string> instructions;
	[SerializeField]
	public int index;

	public Recipe(){
		ingredients = new Dictionary<string, string> ();
		tags = new List<string> ();
		instructions = new Dictionary<string, string> ();
	}

	public Recipe(string recTitle, string recipeImg, Dictionary<string,
		string> ingred, List<string> recTags, int recRating, Dictionary<string, string> instr){
	
		ingredients = new Dictionary<string, string> ();
		tags = new List<string> ();
		instructions = new Dictionary<string, string> ();
		title = recTitle;
		recipeImage = recipeImg;
		ingredients = ingred;
		tags = recTags;
		rating = recRating;
		instructions = instr;
	}

	public Recipe (string recTitle, Dictionary<string,
		string> ingred, int recRating, Dictionary<string, string> instr){

		ingredients = new Dictionary<string, string> ();
		tags = new List<string> ();
		instructions = new Dictionary<string, string> ();
		title = recTitle;
		recipeImage = null;
		ingredients = ingred;
		tags = null;
		rating = recRating;
		instructions = instr;	
	}
}
