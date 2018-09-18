using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

[System.Serializable]
public class Item : MonoBehaviour {

	public int position;
	public string imageUrl;
	public Image img;
	public ItemData data;
	public Text titleText;
	public Text price;
	public bool chosen;
	public GameObject chosenOutline;
	public Texture2D texture;


	void Start () {
		//img = GetComponent<Image> ();
		//titleText = transform.Find ("Title").Find("TitleText").GetComponent<Text>();
		//ratingStars = transform.Find ("Rating").GetComponentsInChildren<Image>();
		texture = new Texture2D (1, 1);

	}

	public void Choose(){
		if (!chosen) {
			chosenOutline.SetActive (true);
			chosen = true;
			data.amount = 1f;
			ChecklistControl.instance.selectedList.itemsData.Add (data);
		} else {
			chosen = false;
			chosenOutline.SetActive (false);
			ChecklistControl.instance.selectedList.itemsData.Remove(data);
		}
	}

	//TODO: Ovo treba resiti! Chosen i unchosen kad se resetuju item kockice
	public void UnChoose(){
		chosen = false;
		chosenOutline.SetActive (false);
	}

	public void LoadImage(){
		//StartCoroutine (LoadImageCoroutine());
		StartCoroutine (GetTexture());

	}

	public void LoadItem(ItemData r){
		imageUrl = r.imgUrl;
		data = r;
		LoadImage ();

		if (titleText != null && data != null)
			titleText.text = data.itemName;
		else {
			Debug.Log (name);
		}
		if (price != null && data != null)
			price.text = data.price;
	}
		
	public void MoveDown(){

		UnChoose ();
		Transform lastItem;
		lastItem = ItemLoader.instance.items [ItemLoader.instance.items.Count - 1].transform;
		if(position == 0)
			transform.localPosition = new Vector3 (ItemLoader.instance.leftItemXPos, lastItem.localPosition.y - 340f, 0f);
		else if(position == 1)
			transform.localPosition = new Vector3 (ItemLoader.instance.middleItemXPos, lastItem.localPosition.y - 340f, 0f);
		else
			transform.localPosition = new Vector3 (ItemLoader.instance.rightItemXPos, lastItem.localPosition.y - 340f, 0f);

		ItemLoader.instance.items.Remove (this);

		LoadNextItem ();

		if (ItemLoader.instance.locker == 2) {
			ItemLoader.instance.transferList [2] = this;
			ItemLoader.instance.items.Add (ItemLoader.instance.transferList [0]);
			ItemLoader.instance.items.Add (ItemLoader.instance.transferList [1]);
			ItemLoader.instance.items.Add (ItemLoader.instance.transferList [2]);
			ItemLoader.instance.locker = 0;
		} else if (ItemLoader.instance.locker == 1) {
			ItemLoader.instance.transferList [1] = this;
			++ItemLoader.instance.locker;
		} else {
			ItemLoader.instance.transferList [0] = this;
			++ItemLoader.instance.locker;
		}

	
	}


	public void MoveUp(){

		UnChoose ();
		Transform firstItem;
		firstItem = ItemLoader.instance.items [0].transform;
		if(position == 0)
			transform.localPosition = new Vector3 (ItemLoader.instance.leftItemXPos, firstItem.localPosition.y + 340f, 0f);
		else if(position == 1)
			transform.localPosition = new Vector3 (ItemLoader.instance.middleItemXPos, firstItem.localPosition.y + 340f, 0f);
		else
			transform.localPosition = new Vector3 (ItemLoader.instance.rightItemXPos, firstItem.localPosition.y + 340f, 0f);
		
		ItemLoader.instance.items.Remove (this);

		LoadPreviousItem ();

		if (ItemLoader.instance.locker == 2) {
			ItemLoader.instance.transferList [2] = this;
			ItemLoader.instance.items.Insert (0, ItemLoader.instance.transferList [0]);
			ItemLoader.instance.items.Insert (0, ItemLoader.instance.transferList [1]);
			ItemLoader.instance.items.Insert (0, ItemLoader.instance.transferList [2]);
			ItemLoader.instance.locker = 0;
		} else if (ItemLoader.instance.locker == 1) {
			ItemLoader.instance.transferList [1] = this;
			++ItemLoader.instance.locker;
		} else {
			ItemLoader.instance.transferList [0] = this;
			++ItemLoader.instance.locker;
		}

		ItemLoader.instance.i--;
	
	}

	void OnDestroy(){

	}

	public void LoadNextItem(){
		if (ItemLoader.instance.i < Items.instance.itemDataList.Count && ItemLoader.instance.i >= 0) {
			//Ovde moze da se samo uzme poslednji trenutni recept dole, vidi se njegov
			//index i ucita se sledeci ako postoji
			Debug.Log(ItemLoader.instance.i.ToString());
			LoadItem (Items.instance.itemDataList [ItemLoader.instance.i++]);
		}
		//Hendlovati ako nije manje
	}

	//TODO: //OVDE MOZE DA BUDE GRESKA! RAZMISLITI! sutra...
	public void LoadPreviousItem(){
		int index = ItemLoader.instance.i - 1;
//		if (ItemLoader.instance.locker == 0)
//			index = ItemLoader.instance.items [0].data.index - 1;
//		else if (ItemLoader.instance.locker == 1)
//			index = ItemLoader.instance.items [0].data.index - 2;
//		else {
//			index = ItemLoader.instance.items [0].data.index - 3;
//
//		}

		if (index > 0) {
			LoadItem (Items.instance.itemDataList [index]);
			ItemLoader.instance.i--;
		}
		//TODO://Hendlovati ako nije vece
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
		yield return www.SendWebRequest ();

		if(www.isNetworkError) {
			Debug.Log(www.error);
		}
		else {
			texture = DownloadHandlerTexture.GetContent (www);
			if (texture != null)
				img.sprite = Sprite.Create (texture, new Rect (0, 0, texture.width, texture.height), Vector2.zero);
			else {
				StartCoroutine (GetTexture());

			}
			//DestroyImmediate (texture);
			www.Dispose ();
			Resources.UnloadUnusedAssets();
		}

	}

	public void ResetImage(){
		StopAllCoroutines ();
		img.sprite = RecipeLoader.instance.recipeSpriteNeutral;
	}
}
