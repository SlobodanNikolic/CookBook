using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChecklistItem : MonoBehaviour {

	public Checklist list;
	public int index;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}



	public void OpenChecklist(){
		ChecklistControl.instance.checklistToOpen = list;
		ChecklistControl.instance.selectedList = list;
		ChecklistControl.instance.DisplayChecklist ();
		UIManager.instance.CheckListSingle ();
	}
}
