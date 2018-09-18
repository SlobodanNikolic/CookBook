using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIManager : MonoBehaviour {

	public GameObject mainPanel;
	public GameObject topMenu;
	public GameObject bottomMenu;
	public GameObject recipePanel;
	public GameObject recipeSinglePanel;
	public GameObject checklistPanel;
	public GameObject listsPanel;
	public GameObject addNewListPanel;
	public GameObject checklistSinglePanel;
	public GameObject addItemsPanel;	

	//Medium stuff
	public GameObject maxiItemsScrollView;
	public GameObject customItemView;
	public GameObject shopItemsSlideView;

	public EventSystem eventSystem;
	public static UIManager instance;
	public string activePanel;

	//small stuff
	public GameObject noListsTitle;
	public GameObject customUnderline;
	public GameObject shopUnderline;

	// Use this for initialization
	void Start () {
		instance = this;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void OpenRecipeSingle(){
		topMenu.SetActive (false);
		bottomMenu.SetActive (false);
		recipePanel.SetActive (false);
		recipeSinglePanel.SetActive (true);
		checklistPanel.SetActive (false);
		checklistSinglePanel.SetActive (false);

		addNewListPanel.SetActive (false);
		activePanel = "RecipeSingle";
		addItemsPanel.SetActive (false);

	}

	public void CloseRecipeSingle(){
		topMenu.SetActive (true);
		bottomMenu.SetActive (true);
		recipePanel.SetActive (true);
		recipeSinglePanel.SetActive (false);
		checklistPanel.SetActive (false);
		addNewListPanel.SetActive (false);
		checklistSinglePanel.SetActive (false);

		activePanel = "RecipePanel";
		addItemsPanel.SetActive (false);

	}

	public void RecipesMain(){
		topMenu.SetActive (true);
		bottomMenu.SetActive (true);
		recipePanel.SetActive (true);
		recipeSinglePanel.SetActive (false);
		checklistSinglePanel.SetActive (false);

		checklistPanel.SetActive (false);
		addNewListPanel.SetActive (false);
		activePanel = "RecipePanel";
		addItemsPanel.SetActive (false);
		ItemLoader.instance.ClosePanel ();

		ChecklistControl.instance.DestroyChecklistSingleItems ();
	}

	public void CheckListMain(){
		topMenu.SetActive (true);
		bottomMenu.SetActive (true);
		recipePanel.SetActive (false);
		checklistSinglePanel.SetActive (false);

		recipeSinglePanel.SetActive (false);
		checklistPanel.SetActive (true);
		addNewListPanel.SetActive (false);
		if (ChecklistControl.instance.checklists.Count == 0 ||
		   ChecklistControl.instance.checklists == null) {
			noListsTitle.SetActive (true);
		}
		else
			noListsTitle.SetActive (false);

		activePanel = "ChecklistMain";
		addItemsPanel.SetActive (false);
		ItemLoader.instance.ClosePanel ();

		ChecklistControl.instance.DestroyChecklistSingleItems ();
	}

	public void CheckListSingle(){
		topMenu.SetActive (true);
		bottomMenu.SetActive (true);
		recipePanel.SetActive (false);
		recipeSinglePanel.SetActive (false);
		checklistPanel.SetActive (false);
		addNewListPanel.SetActive (false);
		checklistSinglePanel.SetActive (true);
		activePanel = "ChecklistSingle";
		addItemsPanel.SetActive (false);
		ItemLoader.instance.UnchooseAll ();
		ItemLoader.instance.ClosePanel ();

	}

	public void AddNewList(){
		topMenu.SetActive (true);
		bottomMenu.SetActive (true);
		recipePanel.SetActive (false);
		recipeSinglePanel.SetActive (false);
		checklistPanel.SetActive (false);
		addNewListPanel.SetActive (true);
		addItemsPanel.SetActive (false);
		checklistSinglePanel.SetActive (false);
		ChecklistControl.instance.DestroyChecklistSingleItems ();
	}


	public void PlusButton(){
		switch (activePanel) {
		case "ChecklistMain":
			AddNewList ();
			break;
		case "RecipePanel":
			break;
		case "ChecklistSingle":
			AddItems ();
			break;
		case "":
			Debug.Log ("Plus button clicked");

			break;
		default:
			Debug.Log ("Plus button clicked");
			break;
		}
	
	}

	public void AddItems(){	
		topMenu.SetActive (true);
		bottomMenu.SetActive (true);
		recipePanel.SetActive (false);
		recipeSinglePanel.SetActive (false);
		checklistPanel.SetActive (false);
		addNewListPanel.SetActive (false);
		checklistSinglePanel.SetActive (false);
		addItemsPanel.SetActive (true);
		maxiItemsScrollView.SetActive (false);
		customItemView.SetActive (true);
		activePanel = "AddItems";

		//Ovo ubaciti kada se klikne na tab
//		if(!ItemLoader.instance.initialized)
//			ItemLoader.instance.LoadFirst ();
	}

	public void CustomItemsTab(){
		customItemView.SetActive (true);
		shopItemsSlideView.SetActive (false);
		shopUnderline.SetActive (false);
		customUnderline.SetActive (true);
	}

	public void ShopItemsTab(){
		customItemView.SetActive (false);
		shopItemsSlideView.SetActive (true);
		shopUnderline.SetActive (true);
		customUnderline.SetActive (false);
		ItemLoader.instance.LoadFirstToSlideView ();
	}
}
