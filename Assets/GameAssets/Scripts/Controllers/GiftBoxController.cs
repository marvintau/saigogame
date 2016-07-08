using UnityEngine;
using System.Collections;

public class GiftBoxController : MonoBehaviour {

	SprintGearsController gearsController;

	void Awake(){
		gearsController = GameObject.FindObjectOfType<SprintGearsController> ();
	}

	// Use this for initialization
	void Start () {
	
	}
	
	public void SetButton(){
		UIButton boxButton = gameObject.GetComponent<UIButton> ();
		if (gearsController.IsOkayToPurchase()){
			boxButton.defaultColor = Color.white;
		} else {
			boxButton.defaultColor = Color.gray;
			boxButton.isEnabled = false;
		}
	}

	void OnClick(){
		gearsController.gearParts.AppendRandomComponent (
			gearsController.grid,
			key => !gearsController.IsUnlocked (key),
			gearsController.AppendButton);
		gearsController.UpdateUIComponents ();

	}

	// Update is called once per frame
	void Update () {
	
	}
}
