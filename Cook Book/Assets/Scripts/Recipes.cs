using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Recipes : MonoBehaviour {

	public string recipeFilePath;
	[SerializeField]
	public List<Recipe> recipeList;
	public static Recipes instance;


	void Awake(){
		instance = this;
	}

	// Use this for initialization
	void Start () {
		//ParseFile (recipeFilePath);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void ParseFile(string filePath){
		TextAsset recipeFile = Resources.Load("recepti") as TextAsset;
		int ind = 0;
		string[] linesInFile = recipeFile.text.Split ('\n');
		string line;
		//Read the text from directly from the test.txt file
		int lineIndex = 0;
		while (true) {
			if (lineIndex > linesInFile.Length - 1) {
				Debug.Log ("Done reading recipes");
				return;
			}
			line = linesInFile[lineIndex++];
			Recipe r = new Recipe ();
			line = line.Trim ();
			if (!line.StartsWith ("*****")) {
				Debug.Log ("Done reading recipes");
				return;
			}
			line = line.Remove (0, 8);
			line = line.Remove (line.Length-9, 9);
			r.title = line;
			while(true){
				line = linesInFile[lineIndex++].Trim ();
				if (line.StartsWith ("dish image")) {
					line = line.Remove (0, 11);
					r.recipeImage = line.Trim ();
					break;
				}
				r.tags.Add (line);
			}
			r.rating = int.Parse(linesInFile[lineIndex++].Remove(0, 7).Trim());
			lineIndex++;
			while (true) {
				line = linesInFile[lineIndex++];
				if (line.Contains ("****"))
					break;
				string separator = "-*-";
				if (line.Contains (separator)) {
					string[] ingredArray = line.Split (separator.ToCharArray (), System.StringSplitOptions.None);
					if(!r.ingredients.ContainsKey(ingredArray[0]))
						r.ingredients.Add (ingredArray [0], ingredArray [3]);
				} else {
					if(!r.ingredients.ContainsKey(line.Trim()))
						r.ingredients.Add (line.Trim(), null);
				}
			}
			while (true) {
				line = linesInFile[lineIndex++];
				if (line.Contains ("*****************"))
					break;
				string separator = "-*-";
				if (line.Contains (separator)) {
					string[] instructionArray = line.Split (separator.ToCharArray (), System.StringSplitOptions.None);
					if(!r.instructions.ContainsKey(instructionArray[0]))
						r.instructions.Add (instructionArray [0], instructionArray [3]);
				} else {
					if(!r.instructions.ContainsKey(line.Trim()))
						r.instructions.Add (line.Trim(), null);
				}
			}
			r.index = ind++;
			recipeList.Add (r);
		}

	}
}
