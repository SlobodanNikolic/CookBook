using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Checklist{

	[SerializeField]
	public string name;
	[SerializeField]
	public string creator;
	[SerializeField]
	public string creationDate;
	[SerializeField]
	public List<Item> items = new List<Item>();
	[SerializeField]
	public List<ItemData> itemsData = new List<ItemData>();

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
