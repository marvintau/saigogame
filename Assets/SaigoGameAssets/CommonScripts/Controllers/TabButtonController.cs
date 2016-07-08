using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class TabButtonController : MonoBehaviour {

	public string textureName;
	public int stars;

	DiverDifficultyController diverCtrl;

	// Use this for initialization
	void Start () {
		diverCtrl = GameObject.FindObjectOfType <DiverDifficultyController> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void OnClick(){
		print ("Gyas");
		diverCtrl.UpdateDifficulties (stars);
		GameObject.Find ("Tabs").GetComponent<UI2DSprite> ().sprite2D = diverCtrl.sprites[textureName];
	}
}
