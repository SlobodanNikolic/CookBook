using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Items : MonoBehaviour {

	public static Items instance;
	public List<ItemData> itemDataList;
	public Dictionary<string, ItemData> itemDataDict;
	public string itemsFilePath;
	public bool itemsRead;
	public List<ItemData> testList;
	public InputField searchInput;

	void Awake(){
		instance = this;
		itemDataDict = new Dictionary<string, ItemData> ();
	}

	// Use this for initialization
	void Start () {
		ParseFile ();
		itemDataList.Sort((x, y) => x.itemName.CompareTo(y.itemName));
//		for (int i = 0; i < itemDataList.Count; i++) {
//			itemDataList [i].index = i;
//		}
	}
		

	// Update is caled once per frame
	void Update () {
		
	}

	public void Search(){
		if (searchInput.text == "") {
			ItemLoader.instance.searchedItemsScroll.gameObject.SetActive (false);
			ItemLoader.instance.scrollObject.SetActive (true);
		} else {
			SearchItemNames (searchInput.text);
		}
	}

	public void SearchItemNames(string name){
		testList = itemDataList.FindAll (s => s.itemName.IndexOf(name, System.StringComparison.OrdinalIgnoreCase) >= 0);
//		ItemLoader.instance.LoadSearched (testList);
		ItemLoader.instance.LoadSearchedToSlideView(testList);
	}

	public void ParseFile(){
		TextAsset itemsFile = Resources.Load("maxi3") as TextAsset;
		int ind = 0;
		string[] linesInFile = itemsFile.text.Split ('\n');
		string line;
		//Read the text from directly from the test.txt file
		int lineIndex = 0;
		while (true) {
			if (lineIndex > linesInFile.Length - 1) {
				Debug.Log ("Done reading items");
				itemsRead = true;
				return;
			}
			line = linesInFile[lineIndex++];
			ItemData r = new ItemData ();
			line = line.Trim ();
			if (!line.StartsWith ("********")) {
				Debug.Log ("Done reading items");
				itemsRead = true;
				return;
			}
			line = line.Remove (0, 8);
			line = line.Remove (line.Length-9, 9);
			r.itemName = line;
			while(true){
				line = linesInFile[lineIndex++].Trim ();
				if (line.StartsWith ("dish image")) {
					line = line.Remove (0, 11);
					r.imgUrl = line.Trim ();
					break;
				}
			}

			line = linesInFile [lineIndex++].Trim ();
			line = line.Remove (0, 7);
			r.price = line;
			lineIndex++;
			if (!itemDataDict.ContainsKey (r.itemName)) {
				itemDataDict.Add (r.itemName, r);
				itemDataList.Add (r);
				r.index = ind++;
			}
		}

	}
}
