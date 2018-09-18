using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class RecipeLoader : MonoBehaviour {

	public GameObject gridObject;
	public GridLayoutGroup grid;
	public static RecipeLoader instance;
	public List<RecipeItem> recipeItems;
	public int i = 0;
	public int j = 0;
	public int test = 0;
	public float leftItemXPos;
	public float rightItemXPos;
	public bool firstLock = false;
	public List<RecipeItem> transferList;
	public Color ratingFilledColor;
	public Color ratingEmptyColor;


	//Objects for single recipe
	public RectTransform ingredients;
	public RectTransform ingredObject;
	public RectTransform instructionParrentObject;
	public RectTransform instructionObject;
	public Image recipeSingleImage;
	public Text recipeSingleTitle;
	public Vector3 lastIngredPosition;
	public List<GameObject> ingredObjList;
	public List<GameObject> instructObjList;

	public float lastInstrHeight;
	public float lastInstRectHeight;

	public bool initialized;
	public Sprite recipeSpriteNeutral;
	public SlideView recipeSlideView;

	// Use this for initialization
	void Start () {
		ingredObjList = new List<GameObject> ();
		instructObjList = new List<GameObject> ();
		instance = this;
		grid = gridObject.GetComponent<GridLayoutGroup> ();
		Recipes.instance.ParseFile (Recipes.instance.recipeFilePath);
	}

	public void Initialize(){
		if (!initialized) {
			UIManager.instance.RecipesMain ();
			LoadFirst ();
			initialized = true;
		} else {
			UIManager.instance.RecipesMain ();
		}
	}

	public void LoadFirst(){
//		bool leftPos = true;
//		leftItemXPos = recipeItems [0].transform.localPosition.x;
//		rightItemXPos = recipeItems [1].transform.localPosition.x;
//		foreach(RecipeItem r in recipeItems){
//			r.LoadRecipe(Recipes.instance.recipeList [i++]);
//			r.leftPosition = leftPos;
//			leftPos = !leftPos;
//		}
		foreach(RecipeItem r in recipeItems){
			r.LoadRecipe(Recipes.instance.recipeList [i++]);
		}
	}

	public void LoadSingleRecipe(Recipe rec){
		recipeSingleTitle.text = rec.title;
		LoadSingleIngreds (rec);
		LoadSingleInstructions (rec);
		LoadImage (rec.recipeImage);
		recipeSlideView.deltaSum = 0f;
		UIManager.instance.OpenRecipeSingle ();
	}

	public void LoadSingleIngreds(Recipe rec){
		int i = 0;
		ingredObjList = new List<GameObject> ();

		foreach(KeyValuePair<string, string> pair in rec.ingredients){
			GameObject instIngredObj = Instantiate (ingredObject.gameObject, ingredObject.parent) as GameObject;
			instIngredObj.GetComponent<RectTransform> ().localPosition = 
				new Vector3 (ingredObject.localPosition.x, ingredObject.localPosition.y - i*108f, ingredObject.position.z);
			ingredObjList.Add (instIngredObj);
			i++;
			instIngredObj.SetActive (true);
			instIngredObj.transform.Find ("IngredientName").GetComponent<Text>().text = pair.Key;
			instIngredObj.transform.Find ("IngredientAmount").GetComponent<Text>().text = pair.Value;

		}
	}

	public void LoadNextRecipesToView(RectTransform view){
		foreach (Transform recIt in view) {
			recIt.GetComponent<RecipeItem> ().LoadNextRecipe ();
		}
	}

	public void LoadPreviousRecipesToView(RectTransform view){
		foreach (Transform recIt in view) {
			recIt.GetComponent<RecipeItem> ().LoadPreviousRecipe ();
		}
	}

	public void LoadSingleInstructions(Recipe rec){
		int i = 0;
		RectTransform lastIngred = ingredObjList[ingredObjList.Count - 1].GetComponent<RectTransform>();
		instructionParrentObject.gameObject.SetActive (true);
		instructionParrentObject.localPosition = 
			new Vector3 (lastIngred.localPosition.x, lastIngred.localPosition.y - 130f, ingredObject.position.z);

		instructObjList = new List<GameObject> ();


		foreach(KeyValuePair<string, string> pair in rec.instructions){				
			
			if (i == 0) {
				GameObject instInstructionObj = Instantiate (instructionObject.gameObject, instructionParrentObject) as GameObject;
				instInstructionObj.SetActive (true);
				RectTransform instrText = instInstructionObj.transform.Find ("InstructionText").GetComponent<RectTransform> ();
				instrText.GetComponent<Text> ().text = pair.Key;
				instrText.GetComponent<ContentSizeFitter> ().verticalFit = ContentSizeFitter.FitMode.PreferredSize;
				StartCoroutine (SetSize (instrText));
				instructObjList.Add (instInstructionObj);
				i++;
			} else {
				GameObject instInstructionObj = Instantiate (instructionObject.gameObject, instructionParrentObject) as GameObject;
				instInstructionObj.SetActive (true);
				RectTransform instrText = instInstructionObj.transform.Find ("InstructionText").GetComponent<RectTransform> ();
				instrText.GetComponent<Text> ().text = pair.Key;
				instrText.GetComponent<ContentSizeFitter> ().verticalFit = ContentSizeFitter.FitMode.PreferredSize;
				StartCoroutine (SetSize (instrText));
				instructObjList.Add (instInstructionObj);
				i++;
			}


		}
	}
		
	public IEnumerator SetSize(RectTransform recta){
		yield return new WaitForEndOfFrame ();
		if(lastInstRectHeight > recta.rect.height)
			recta.parent.localPosition = new Vector3 (0f, lastInstrHeight - lastInstRectHeight, 0f);
		else
			recta.parent.localPosition = new Vector3 (0f, lastInstrHeight - recta.rect.height, 0f);

		lastInstRectHeight = recta.rect.height;
		lastInstrHeight = recta.parent.localPosition.y;
		//Debug.Log (recta.rect.height.ToString());

	}

	// Update is called once per frame
	void Update () {
//		if (Input.GetKeyDown (KeyCode.R)) {
//			recipeItems [test].MoveDown ();
//			recipeItems [test+1].MoveDown ();
//			test += 2;
//		}
	}

	public void LoadImage(string imageUrl){
		StartCoroutine (LoadImageCoroutine(imageUrl));
	}

	IEnumerator LoadImageCoroutine(string imageUrl) {
		//Moze se optimizovati da cita sliku iz cache-a
		WWW www = new WWW(imageUrl);
		yield return www;
		recipeSingleImage.sprite = Sprite.Create(www.texture, new Rect(0, 0, www.texture.width, www.texture.height), new Vector2(0, 0));
	}



	public void DestroyLoaded(){
		foreach (GameObject obj in ingredObjList) {
			Destroy (obj);
		}
		foreach (GameObject obj in instructObjList) {
			Destroy (obj);
		}

		lastIngredPosition = Vector3.zero;
		lastInstRectHeight = 0f;
		lastInstrHeight = 0f;
	}
}
