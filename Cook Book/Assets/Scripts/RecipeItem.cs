using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;


public class RecipeItem : MonoBehaviour {

	public string imageUrl;
	public Image img;
	public Recipe recipe;
	public bool leftPosition;
	public Text titleText;
	public Image[] ratingStars;
	public Texture2D texture;

	void Start () {
		img = GetComponent<Image> ();
		texture = new Texture2D (1, 1);
		//titleText = transform.Find ("Title").Find("TitleText").GetComponent<Text>();
		//ratingStars = transform.Find ("Rating").GetComponentsInChildren<Image>();
	}

	public void LoadImage(){
		//StartCoroutine (LoadImageCoroutine());
		StartCoroutine(GetTexture());
	}

	public void LoadSingle(){
		RecipeLoader.instance.LoadSingleRecipe (recipe);
	}

	public void LoadRecipe(Recipe r){
		imageUrl = r.recipeImage;
		recipe = r;
		LoadImage ();

		if (titleText != null && recipe != null)
			titleText.text = recipe.title;
		else {
			Debug.Log (name);
		}
		Color cF = RecipeLoader.instance.ratingFilledColor;
		Color cE = RecipeLoader.instance.ratingEmptyColor;

		if (ratingStars != null) {
			for (int i = 0; i < recipe.rating; i++) {
				ratingStars [i].color = new Color (cF.r, cF.g, cF.b, 1f);
			}
			for (int j = recipe.rating; j < 5; j++) {
				ratingStars [j].color = new Color (cE.r, cE.g, cE.b, 1f);
			}
		}
	}

	public void UnloadImage(){

	}

	public void MoveDown(){
		Transform lastRecItem;
		lastRecItem = RecipeLoader.instance.recipeItems [RecipeLoader.instance.recipeItems.Count - 1].transform;
		if(leftPosition)
			transform.localPosition = new Vector3 (RecipeLoader.instance.leftItemXPos, lastRecItem.localPosition.y - 530f, 0f);
		else
			transform.localPosition = new Vector3 (RecipeLoader.instance.rightItemXPos, lastRecItem.localPosition.y - 530f, 0f);
		RecipeLoader.instance.recipeItems.Remove (this);

		LoadNextRecipe ();

		if (RecipeLoader.instance.firstLock) {
			RecipeLoader.instance.transferList [1] = this;
			RecipeLoader.instance.recipeItems.Add (RecipeLoader.instance.transferList [0]);
			RecipeLoader.instance.recipeItems.Add (RecipeLoader.instance.transferList [1]);
			RecipeLoader.instance.firstLock = false;
		} else {
			RecipeLoader.instance.transferList [0] = this;
			RecipeLoader.instance.firstLock = true;
		}
//		RecipeLoader.instance.recipeItems.Add (this);

		//transform.parent = RecipeLoader.instance.grid.transform;
	}

	public void ResetImage(){
		StopAllCoroutines ();
		img.sprite = RecipeLoader.instance.recipeSpriteNeutral;
	}

	public void MoveUp(){
		Transform firstRecItem;
		firstRecItem = RecipeLoader.instance.recipeItems [0].transform;
		if(leftPosition)
			transform.localPosition = new Vector3 (RecipeLoader.instance.leftItemXPos, firstRecItem.localPosition.y + 530f, 0f);
		else
			transform.localPosition = new Vector3 (RecipeLoader.instance.rightItemXPos, firstRecItem.localPosition.y + 530f, 0f);
		RecipeLoader.instance.recipeItems.Remove (this);

		LoadPreviousRecipe ();

		if (RecipeLoader.instance.firstLock) {
			RecipeLoader.instance.transferList [1] = this;
			RecipeLoader.instance.recipeItems.Insert (0, RecipeLoader.instance.transferList [0]);
			RecipeLoader.instance.recipeItems.Insert (0, RecipeLoader.instance.transferList [1]);
			RecipeLoader.instance.firstLock = false;
		} else {
			RecipeLoader.instance.transferList [0] = this;
			RecipeLoader.instance.firstLock = true;
		}

		RecipeLoader.instance.i--;
		//		RecipeLoader.instance.recipeItems.Add (this);

		//transform.parent = RecipeLoader.instance.grid.transform;
	}

	void OnDestroy(){

	}

	public void LoadNextRecipe(){
		if (RecipeLoader.instance.i < Recipes.instance.recipeList.Count) {
			//Ovde moze da se samo uzme poslednji trenutni recept dole, vidi se njegov
			//index i ucita se sledeci ako postoji
			LoadRecipe (Recipes.instance.recipeList [RecipeLoader.instance.i++]);
		}
		//Hendlovati ako nije manje
	}

	public void LoadPreviousRecipe(){
		int index = RecipeLoader.instance.i - 1;
//		if(!RecipeLoader.instance.firstLock)
//			index = RecipeLoader.instance.recipeItems [0].recipe.index -1;
//		else
//			index = RecipeLoader.instance.recipeItems [0].recipe.index -2;
		
		if (index > 0) {
			LoadRecipe (Recipes.instance.recipeList [index]);
			RecipeLoader.instance.i--;
		}
		//Hendlovati ako nije vece
	}

	void OnTriggerEnter2D(Collider2D coll){
		StopAllCoroutines ();

		if (coll.name == "TopTrigger") {
			Debug.Log ("TopTrigger entered");

			MoveDown ();
		} else {
			Debug.Log ("BottomTrigger entered");

			MoveUp ();
		}
	}

	// Update is called once per frame
	void Update () {

	}

	IEnumerator LoadImageCoroutine() {
		WWW www = new WWW(imageUrl);
		yield return www;
		//img.sprite = Sprite.Create(www.texture, new Rect(0, 0, www.texture.width, www.texture.height), new Vector2(0, 0));
		if (string.IsNullOrEmpty(www.error)) {
			www.LoadImageIntoTexture (texture);
			img.sprite = Sprite.Create (texture, new Rect (0, 0, www.texture.width, www.texture.height), Vector2.zero);
			DestroyImmediate(www.texture);
		}
		www.Dispose ();
		Resources.UnloadUnusedAssets();
	}

	IEnumerator GetTexture() {
		UnityWebRequest www = UnityWebRequestTexture.GetTexture(imageUrl);
		www.SetRequestHeader("Accept", "image/*");
		yield return www.SendWebRequest();

		if(www.isNetworkError) {
			Debug.Log(www.error);
		}
		else {
			//texture = ((DownloadHandlerTexture)www.downloadHandler).texture;
			texture = DownloadHandlerTexture.GetContent (www);
			if (texture != null) {
				img.sprite = Sprite.Create (texture, new Rect (0, 0, texture.width, texture.height), Vector2.zero);
			} else {
				Debug.Log ("Texture is null");
				StartCoroutine (GetTexture());
			}
			//Destroy (texture);
			www.Dispose ();
			//Resources.UnloadUnusedAssets();
		}


	}
}
