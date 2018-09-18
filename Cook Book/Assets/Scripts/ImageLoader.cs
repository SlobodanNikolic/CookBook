using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageLoader : MonoBehaviour {

	public string imageUrl;
	public Image img;



	// Use this for initialization
	void Start () {
		img = GetComponent<Image> ();
	}

	public void LoadImage(){
		StartCoroutine (LoadImageCoroutine());
	}

	public void UnloadImage(){
		
	}

	void OnDestroy(){
	
	}

	// Update is called once per frame
	void Update () {
		
	}

	IEnumerator LoadImageCoroutine() {
		WWW www = new WWW(imageUrl);
		yield return www;
		img.sprite = Sprite.Create(www.texture, new Rect(0, 0, www.texture.width, www.texture.height), new Vector2(0, 0));
	}
}
