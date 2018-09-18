using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ChecklistControl : MonoBehaviour {

	public string newChecklistName;
	public InputField nameField;
	public GameObject checklistObject;
	public List<Checklist> checklists = new List<Checklist>();
	public Transform gridObject;
	public static ChecklistControl instance;
	public Checklist selectedList;
	public Checklist checklistToOpen;
	public Text listSingleTitle;
	public GameObject cheklistItemSingle;
	public Transform gridSingleObject;
	public List<GameObject> instSingleListItems;

	// Use this for initialization
	void Start () {
		instance = this;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void CreateNew(bool addItems){
		if (nameField.text == "")
			return;
		Checklist newList = new Checklist ();
		newList.name = nameField.text;
		checklists.Add (newList);
		UpdateLists (newList);

		if (addItems) {
			UIManager.instance.AddItems ();
			selectedList = newList;

		} else {
			UIManager.instance.CheckListMain ();
		}
	}

	public void UpdateLists(Checklist c){
		GameObject l = Instantiate (checklistObject, gridObject) as GameObject;
		l.SetActive (true);
		ChecklistItem item = l.GetComponent<ChecklistItem> ();
		item.list = c;
		l.transform.Find ("TitleText").GetComponent<Text>().text = c.name;
	}

	public void DisplayChecklist(){
		listSingleTitle.text = checklistToOpen.name;
		instSingleListItems = new List<GameObject> ();
		for (int i = 0; i < checklistToOpen.itemsData.Count; i++) {
			GameObject single = Instantiate (cheklistItemSingle, gridSingleObject) as GameObject;
			single.transform.Find ("Number").GetComponent<Text>().text = (i+1).ToString()+".";
			single.transform.Find ("TitleText").GetComponent<Text>().text = checklistToOpen.itemsData[i].itemName;
			single.transform.Find ("Quantity").GetComponent<Text>().text = checklistToOpen.itemsData[i].amount.ToString();
			single.transform.Find ("Unit").GetComponent<Text>().text = checklistToOpen.itemsData[i].unit;
			LoadImage(single.transform.Find ("Image").GetComponent<Image>(), checklistToOpen.itemsData[i].imgUrl);

			single.SetActive (true);
			instSingleListItems.Add (single);
		}
	}

	public void DestroyChecklistSingleItems(){
		foreach (GameObject go in instSingleListItems)
			Destroy (go);
		instSingleListItems.Clear ();
		StopAllCoroutines ();
	}

	public void LoadImage(Image img, string url){
		StartCoroutine (LoadImageCoroutine(img, url));
	}

	IEnumerator LoadImageCoroutine(Image img, string url) {
		if (url != "") {
			WWW www = new WWW (url);
			yield return www;
			img.sprite = Sprite.Create (www.texture, new Rect (0, 0, www.texture.width, www.texture.height), new Vector2 (0, 0));
		}
	}
}
