using UnityEngine;
using System.Collections;

public class NewItemPanelController : MonoBehaviour {

	GameObject light1, light2;

	Vector3 z = new Vector3 (0, 0, 1);

	bool isShowing = false;

	// Use this for initialization
	void Start () {

		light1 = GameObject.Find ("Light1");
		light2 = GameObject.Find ("Light2");
	
	}

	public void Show(bool isToShow){
		isShowing = isToShow;
//		GameObject.Find ("Debug").GetComponent<UILabel> ().text = "Running";

		gameObject.GetComponent <UIPanel>().alpha = isShowing ? 1 : 0;
	}

	// Update is called once per frame
	void Update () {
		if (isShowing && (Input.touchCount == 1)) {

			if(Input.GetTouch (0).phase == TouchPhase.Began){
				Show (false);
			}

		}

		light1.transform.RotateAround (Vector3.zero, z, 0.1f);
		light2.transform.RotateAround (Vector3.zero, z, 0.2f);
	}
}
