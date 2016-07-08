using UnityEngine;
using System.Collections;

using System;
using System.Collections.Generic;
using System.Linq;
using Spine.Unity;
using Spine;


public class DiverDifficultyController : MonoBehaviour {

	private Switchable actions;

	GameObject grid;

	DiverPlayerController playerCtrler;
	PersistentData data;

	public Dictionary<string, Sprite> sprites;

	public string currentAction = "101A";


	bool Star3(string key){
		return actions.data.GetChild ("Actions").GetChild (key).GetChild ("Class").leaf == "3";
	}

	bool Star4(string key){
		return actions.data.GetChild ("Actions").GetChild (key).GetChild ("Class").leaf == "4";
	}

	bool Star5(string key){
		return actions.data.GetChild ("Actions").GetChild (key).GetChild ("Class").leaf == "5";
	}

	void CreateButton(string typeKey, string actionKey, Func<string, bool> IsApplicable, GameObject gridObject) {
		if (IsApplicable(actionKey)){

			GameObject actionButtonObject = NGUITools.AddChild (gridObject);
			actionButtonObject.name = typeKey + "-" + actionKey;
			actionButtonObject.AddComponent<ActionButtonController> ().SetButton (actionKey);
		}

	}

	void LoadData() {
		actions = new Switchable("DiverActions");
	}

	void Awake(){
		playerCtrler = gameObject.GetComponent<DiverPlayerController> ();
		data = GameObject.FindObjectOfType<PersistentData> ();
	}

//	void SetupCharacterGear(){
//		Switchable gears = new Switchable("DiverGears");
//		gears.SelectItem ("Gears", "SeparateSwimSuit");
//
//
//		foreach(KeyValuePair<string, TreeNode> gear in gears.GetSelected ().children){
//			character.skeleton.SetAttachment (gear.Key, gear.Value.leaf);
//		}
//
//	}

	public void SetupCharacterAction(){

		actions.SelectItem ("Actions", currentAction);
		TreeNode sequence = actions.GetSelected ().GetChild ("Sequence");

		data.chosenSequnece = sequence;
		playerCtrler.SetupCharacterAction (sequence);

	}

	public void UpdateDifficulties(int stars){
		while (grid.transform.childCount > 0){
			NGUITools.Destroy (grid.transform.GetChild (0).gameObject);
		}

		Func<string, bool> diffCheck;
		switch (stars)
		{
		case 3:
			diffCheck = Star3;
			data.chosenDifficulty = 3;
			break;
		case 4:
			diffCheck = Star4;
			data.chosenDifficulty = 4;
			break;
		default:
			diffCheck = Star5;
			data.chosenDifficulty = 5;
			break;
		}

		actions.CreateComponents (grid, diffCheck, CreateButton);
		grid.GetComponent<UIGrid> ().Reposition ();
		grid.GetComponent<UIGrid> ().keepWithinPanel = true;
	}

	void Start () {
		sprites = Resources.LoadAll<Sprite> ("Textures/Difficulty-Scene/DifficultySelect").ToDictionary (x=>x.name, x=>x);
		grid = GameObject.Find ("DiveSetsGrid");

		LoadData ();
		actions.CreateComponents (grid, Star3, CreateButton);
		grid.GetComponent<UIGrid> ().Reposition ();

		SetupCharacterAction ();

	}

	// Update is called once per frame
	void Update () {
	}
}
