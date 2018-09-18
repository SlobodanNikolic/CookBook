using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlideView : MonoBehaviour {

	public Vector3 screenPoint;
	public Vector3 offset;
	public RectTransform thisRect;

	public Vector3 lastPos;
	public Vector3 delta;
	public float deltaSum;
	public int moveCoef = 0;

	public RectTransform currentView;

	public List<RectTransform> views;

	public float slideTime;
	public float currentX;
	public int currViewIndex = 0;
	public int left = 0;
	public int right = 3;
	public bool clicks = true;
	public GameObject clickBlocker;
	public float time; 
	public bool itemView = false;

	// Use this for initialization
	void Start () {
		SetupStartPos ();
	}

	public void SetupStartPos(){
		currentView = views [currViewIndex];
	}
	
	// Update is called once per frame
	void Update () {
		
		if ( Input.GetMouseButtonDown(0) )
		{
			lastPos = Input.mousePosition;
			time = Time.time;
		}
		else if ( Input.GetMouseButton(0) )
		{
			delta = Input.mousePosition - lastPos;
			deltaSum += delta.x;
			if (deltaSum > 2f || deltaSum < -2f && clicks) {
				DisableClicks ();
			}
			// Do Stuff here
			thisRect.anchoredPosition += new Vector2 (delta.x, 0);

			// End do stuff

			lastPos = Input.mousePosition;
		}
		if (Input.GetMouseButtonUp (0)) {
			float speed = Mathf.Abs(deltaSum) / (Time.time - time);
			Debug.Log (speed.ToString());
			if (deltaSum >= 300f || (deltaSum > 0f && speed > 1000f)) {
				//Idi desno
				SlideRight();
				if (left < 2) {
					MoveLeft(views [views.Count - 1]);
				}
				//currentView = views [++currViewIndex];

			} else if (deltaSum <= -300f || (deltaSum < 0f && speed > 1000f)) {
				//Idi levo
				SlideLeft();
				if (left >= 2) {
					MoveRight(views[0]);
				}
				//currentView = views [++currViewIndex];

			} else {
				currentX = thisRect.localPosition.x;
				StartCoroutine (Slide (moveCoef * 1080f));

			}

			deltaSum = 0f;
			EnableClicks ();
		}

	}

	public void SlideLeft(){
		currentX = thisRect.localPosition.x;
		moveCoef--;
		left++;
		right--;
		StartCoroutine (Slide (moveCoef * 1080f));
//		currViewIndex = (currViewIndex + 1) % 4;
//		currentView = views [currViewIndex];
	}

	public void SlideRight(){
		currentX = thisRect.localPosition.x;
		moveCoef++;
		right++;
		left--;
		StartCoroutine (Slide (moveCoef * 1080f));
//		currViewIndex = (currViewIndex - 1) % 4;
//		currentView = views [currViewIndex];
	}

	public void DisableClicks(){
		clickBlocker.SetActive (true);
		clicks = false;
	}

	public void EnableClicks(){
		clickBlocker.SetActive (false);
		clicks = true;
	}
	//levo - element 2
	//desno - element 0
	public void MoveLeft(RectTransform view){
		view.transform.localPosition = new Vector3 (view.transform.localPosition.x - views.Count * 1080f, view.transform.localPosition.y);
		views.RemoveAt (views.Count-1);
		views.Insert (0, view);
		right--;
		left++;
		if (itemView) {
			ResetItemImagesInView (view);
			ItemLoader.instance.LoadNextItemsToView (views[0]);
		} else {
			ResetImages (view);
			RecipeLoader.instance.LoadPreviousRecipesToView (views [0]);
		}
	}

	public void MoveRight(RectTransform view){
		view.transform.localPosition = new Vector3 (view.transform.localPosition.x + views.Count * 1080f, view.transform.localPosition.y);
		views.RemoveAt (0);
		views.Add (view);
		left--;
		right++;
		if (itemView) {
			ResetItemImagesInView (view);
			ItemLoader.instance.LoadNextItemsToView (views[2]);
		} else {
			ResetImages (view);
			RecipeLoader.instance.LoadNextRecipesToView (views [2]);
		}
	}

	public void ResetImages(RectTransform view){
		foreach (Transform recIt in view) {
			recIt.GetComponent<RecipeItem> ().ResetImage ();
		}
	}

	public void ResetItemImagesInView(RectTransform view){
		foreach (Transform recIt in view) {
			recIt.GetComponent<Item> ().ResetImage ();
		}
	}

	public IEnumerator Slide(float targetXPos){
		for (float i = 0f; i <= slideTime;) {
			i += Time.deltaTime;
			float currentTime = Mathf.Min (i / slideTime, 1f);
			thisRect.localPosition = new Vector3 (Easing.EaseOutQuad (currentX, targetXPos, currentTime), thisRect.localPosition.y);


			yield return null;
		}
		Resources.UnloadUnusedAssets();
	}

	//keanuReeves is the top plate. The chosen one.
//	public IEnumerator MoveUp(float inc, float time, bool keanuReeves){
//		while (isMoving) {
//			yield return null;
//		}
//		isMoving = true;
//		if (keanuReeves) {
//			//Ispitati ovo na par fora
//			if (Gameplay3.instance.plates.Contains (this)) {
//				Gameplay3.instance.SwitchLeftPlates ();
//			} else {
//				Gameplay3.instance.SwitchRightPlates ();
//			}
//		}
//
//		currentY = transform.localPosition.y;
//		for (float i = 0f; i <= time;) {
//			i += Time.deltaTime;
//			float currentTime = Mathf.Min (i / time, 1f);
//			transform.localPosition = new Vector3 (transform.localPosition.x, Easing.EaseOutBack (currentY, currentY + inc, currentTime));
//
//
//			yield return null;
//		}
//
//		isMoving = false;
//		if (keanuReeves) {
//			if (Gameplay3.instance.plates.Contains (this)) {
//				Gameplay3.instance.MoveFirstLeftPlateDown ();
//			} else {
//				Gameplay3.instance.MoveFirstRightPlateDown ();
//			}
//		}
//	}

}
