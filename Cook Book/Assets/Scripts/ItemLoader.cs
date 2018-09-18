using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemLoader : MonoBehaviour {

	public GameObject scrollObject;
	public GameObject gridObject;
	public GridLayoutGroup grid;
	public static ItemLoader instance;
	public List<Item> items; //objekti u gridu
	public int i = 0;
	public int j = 0;
	public int test = 0;
	public float leftItemXPos;
	public float rightItemXPos;
	public float middleItemXPos;
	public int locker = 0;
	public List<Item> transferList;
	public Color ratingFilledColor;
	public Color ratingEmptyColor;
	public int position = 0;

	//Objects for searched items
	public Transform searchedItemsScroll;
	public Transform searchedItemsGrid;

	public bool initialized;
	public GameObject itemObject;

	public InputField itemNameInput;
	public InputField itemQuantInput;
	public GameObject kgBcg;
	public GameObject gBcg;
	public GameObject pieceBcg;
	public Text noticeText;
	public GameObject searchedItemsSlide;
	public GameObject shopItemsSlide;
	public GameObject customItemsPanel;
	public List<Item> searchedSlideItems;
	public int searchIndex = 0;

	// Use this for initialization
	void Start () {
		instance = this;
		grid = gridObject.GetComponent<GridLayoutGroup> ();
		itemQuantInput.text = "1";
	}

	public void LoadFirst(){
		searchedItemsScroll.gameObject.SetActive (false);
		scrollObject.SetActive (true);

		leftItemXPos = items [0].transform.localPosition.x;
		middleItemXPos = items [1].transform.localPosition.x;
		rightItemXPos = items [2].transform.localPosition.x;
		foreach(Item r in items){
			r.LoadItem(Items.instance.itemDataList [i++]);
			r.position = position;
			position = (position + 1) % 3;
		}
		initialized = true;
	}

	public void LoadFirstToSlideView(){
		if (initialized)
			return;
		else {
			foreach (Item r in items) {
				r.LoadItem (Items.instance.itemDataList [i++]);
			}
			initialized = true;
		}
	}

	public void UnchooseAll(){
		foreach (Item it in items) {
			it.UnChoose ();
		}
	}

	public void LoadPreviousItemsToView(RectTransform view){
		foreach (Transform recIt in view) {
			recIt.GetComponent<Item> ().LoadPreviousItem ();
		}
	}

	public void LoadNextItemsToView(RectTransform view){
		foreach (Transform recIt in view) {
			recIt.GetComponent<Item> ().LoadNextItem ();
		}
	}

	public void LoadSearched(List<ItemData> searchedList){
		EmptySearchGrid ();
		searchedItemsScroll.gameObject.SetActive (true);
		scrollObject.SetActive (false);
		int i = 0;
		foreach (ItemData d in searchedList) {
			GameObject itemObj = Instantiate (itemObject, searchedItemsGrid) as GameObject;
			itemObj.GetComponent<BoxCollider2D> ().enabled = false;
			itemObj.GetComponent<Item> ().LoadItem(d);
			i++;
			if (i > 50)
				break;
		}
	}

	public void LoadSearchedToSlideView(List<ItemData> searchedList){

		searchedItemsSlide.SetActive (true);
		shopItemsSlide.SetActive (false);
		customItemsPanel.SetActive (false);

		searchIndex = 0;
		if (initialized)
			return;
		else {
			foreach (ItemData d in searchedList) {
				if (searchIndex >= searchedSlideItems.Count)
					break;
				searchedSlideItems[searchIndex].LoadItem (searchedList[searchIndex++]);
			}
			initialized = true;
		}
	}

	//TODO: Optimizovati!
	public void EmptySearchGrid(){
		foreach (Transform child in searchedItemsGrid)
			Destroy (child.gameObject);
	}

	public void CancelSearch(){
		
	}

	public void ItemQuantityMinus(){
		float itemQuant = 0;
		if (float.TryParse (itemQuantInput.text, out itemQuant)) {
			if (itemQuant > 0) {
				itemQuant--;
				itemQuantInput.text = itemQuant.ToString ();
			}
		}
	}

	public void ItemQuantityPlus(){
		float itemQuant = 0;
		if (float.TryParse (itemQuantInput.text, out itemQuant)) {
			itemQuant++;
			itemQuantInput.text = itemQuant.ToString ();
		}
	}

	public void KgClicked(){
		kgBcg.SetActive (true);
		gBcg.SetActive (false);
		pieceBcg.SetActive (false);
	}

	public void GClicked(){
		kgBcg.SetActive (false);
		gBcg.SetActive (true);
		pieceBcg.SetActive (false);
	}

	public void PieceClicked(){
		kgBcg.SetActive (false);
		gBcg.SetActive (false);
		pieceBcg.SetActive (true);
	}

	public void AddItem(){
		ItemData item = new ItemData ();
		float itemQuant = 0;
		if (float.TryParse (itemQuantInput.text, out itemQuant)) {
			item.amount = itemQuant;
		} else {
			return;
		}
		if (itemNameInput.text != "")
			item.itemName = itemNameInput.text;
		else {
			return;
		}
		if (kgBcg.activeSelf)
			item.unit = "Kg";
		if (gBcg.activeSelf)
			item.unit = "g";
		if (pieceBcg.activeSelf)
			item.unit = "kom";
		
		ChecklistControl.instance.selectedList.itemsData.Add (item);
		itemNameInput.text = "";
		itemQuantInput.text = "1";
		StartCoroutine (BlinkNotice("Stavka uspesno dodata", 2f));
		Debug.Log(JsonUtility.ToJson (ChecklistControl.instance.selectedList));
	}

	public IEnumerator BlinkNotice(string notice, float blinkTime){
		noticeText.text = notice;
		noticeText.gameObject.SetActive (true);
		yield return new WaitForSeconds (2f);
		noticeText.gameObject.SetActive (false);
	}

	public void ClosePanel(){
		noticeText.gameObject.SetActive (false);
		itemNameInput.text = "";
		itemQuantInput.text = "1";
	}

	// Update is called once per frame
	void Update () {
		//		if (Input.GetKeyDown (KeyCode.R)) {
		//			recipeItems [test].MoveDown ();
		//			recipeItems [test+1].MoveDown ();
		//			test += 2;
		//		}

	}
		
}
