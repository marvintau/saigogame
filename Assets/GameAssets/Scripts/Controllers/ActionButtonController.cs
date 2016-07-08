using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;

public class ActionButtonController : MonoBehaviour {

	static Font font = Resources.Load ("Seravek-Bold") as Font;

	DiverDifficultyController diverCtrl;

	string label = "";

	void SetLabel(string labelName){ 
		UI2DSprite actionSprite = gameObject.AddComponent<UI2DSprite> ();
		actionSprite.sprite2D = diverCtrl.sprites ["difficulty-back-high"];
		actionSprite.MakePixelPerfect ();

		UILabel actionLabel = gameObject.AddComponent <UILabel> ();
		actionLabel.fontSize = 30;
		actionLabel.alignment = NGUIText.Alignment.Left;
		actionLabel.text = labelName;
		actionLabel.width = actionSprite.width - 20;
		actionLabel.height = actionSprite.height - 20;
		actionLabel.trueTypeFont = font;
		actionLabel.depth = 2;
		actionLabel.color = Color.black;

	}


	void SetButtonComponent(){

		UIButton actionButton = gameObject.AddComponent<UIButton> ();
		NGUITools.AddWidgetCollider (gameObject);
		gameObject.AddComponent <UIDragScrollView>();

		actionButton.defaultColor = Color.white;
		actionButton.pressedSprite2D = diverCtrl.sprites ["difficulty-back-low"];
		actionButton.tweenTarget = gameObject;
		actionButton.hoverSprite = "Window";

	}



	public void SetButton(string labelName){

		label = labelName;

		SetLabel (labelName);
		SetButtonComponent();
	}

	public void OnClick(){
		diverCtrl.currentAction = label;
		diverCtrl.SetupCharacterAction ();
	}

	void Awake(){
		diverCtrl = GameObject.FindObjectOfType<DiverDifficultyController> ();

	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
