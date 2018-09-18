using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemData{

	[SerializeField]
	public string itemName;
	[SerializeField]
	public string imgUrl;
	[SerializeField]
	public string price;
	[SerializeField]
	public int index;
	[SerializeField]
	public float amount;
	[SerializeField]
	public bool chosen;
	[SerializeField]
	public string unit;

	public ItemData(){
		imgUrl = "";
		itemName = "";
		price = "";
		index = -1;
		unit = "kom";
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
