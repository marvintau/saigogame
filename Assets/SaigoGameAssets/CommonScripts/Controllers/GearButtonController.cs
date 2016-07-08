using UnityEngine;
using System.Collections;
using System;

public class GearButtonController : MonoBehaviour {

	SprintGearsController gearsCtrl;

	void SetSprite(Sprite sprite){

		UI2DSprite gearSprite = gameObject.AddComponent<UI2DSprite> ();
		gearSprite.sprite2D = sprite;
		gearSprite.localSize.Set (116, 302);
		gearSprite.height = 302;
		gearSprite.width = 116;
	
	}

	void SetDraggable(){
	
		NGUITools.AddWidgetCollider (gameObject);
		gameObject.AddComponent<UIDragScrollView> ();
	
	}

	void CreateLabel(){
	
		GameObject labelObject = NGUITools.AddChild (gameObject);
		labelObject.name = gameObject.name;
	
	}


	void Awake(){
		gearsCtrl = GameObject.FindObjectOfType<SprintGearsController> ();
	}

	void Start(){
	}

	void SetButtonComponent(){
		UIButton gearButton = gameObject.AddComponent<UIButton> ();

		gearButton.defaultColor = Color.white;
		gearButton.tweenTarget = gameObject;
		gearButton.hoverSprite = "Window";

	}

	public void SetButton(Sprite sprite){
		SetSprite (sprite);
		SetButtonComponent ();
		SetDraggable ();
		CreateLabel ();
	}

	void OnClick(){
		string name = gameObject.name.Split ('-') [1];

			gearsCtrl.Select (name);
			gearsCtrl.UpdateUIComponents ();

	}

}
