using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;

using Spine.Unity;
using System.Diagnostics;

public class SprintGearsController : MonoBehaviour {

	static float MAX_ATTRIBUTE = 35;

	public GameObject grid;

	PersistentData savedData;

	[HideInInspector]
	public Switchable gearParts;
	public Switchable gearAttributes;

	List<Sprite> icons;

	UISlider str;
	UISlider agi;
	UISlider lck;
	UILabel money;
	UILabel records;
	SkeletonAnimation character;

	NewItemPanelController newItemPanel;

	public bool IsUnlocked(string gear){
		return savedData.IsUnlocked (gear);
	}

	public bool IsOkayToPurchase(){
		return savedData.IsOkayToPurchase ();
	}

	public void AddUnlockedItem(string gear){
		savedData.AddUnlocked (gear);
	}

	public void Select(string gear){
		savedData.Select (gear);
	}
		

	void CreateButton(string typeKey, string gearKey, Func<string, bool> IsApplicable, GameObject gridObject) {

		if (IsApplicable(gearKey)){

			Sprite sprite = icons.Find (x => x.name == gearKey);

			GameObject gearButtonObject = NGUITools.AddChild (gridObject);
			gearButtonObject.name = typeKey + "-" + gearKey;
			gearButtonObject.AddComponent<GearButtonController> ().SetButton (sprite);
		}

	}
		
	public void AppendButton(string typeKey, string gearKey, Func<string, bool> IsApplicable, GameObject parentUIObject){
		AddUnlockedItem (gearKey);

		CreateButton (typeKey, gearKey, IsApplicable, parentUIObject);

		GameObject.Find ("NewItemIcon").GetComponent <UI2DSprite> ().sprite2D = icons.Find (x => x.name == gearKey);
		newItemPanel.Show (true);
	}


	void LoadData(){
		
		gearParts = new Switchable ("SprintGears");
		gearAttributes = new Switchable ("SprintGearAttributes");

		savedData.totalGearList = gearParts.data;
		savedData.totalGearAttributes = gearAttributes.data;

		icons = Resources.LoadAll<Sprite> ("Textures/Gear-Scene/gear-icons").ToList();
	}

	void LoadPersistentData(){
		Data data = SaveLoadController.Load ();
		if (data != null){
			savedData.data = data;
		}
	}

	/**
	 * Initialize Gears defines the default gears, if the saved files are missing
	 * or broken for any reasons. Thus this should be called before defining any
	 * buttons.
	 */

	void InitializeGearData(){
		if (savedData.data.selectedGear == "") {
			savedData.data.selectedGear = "casual";
		}

		gearParts.SelectItem ("Gears", savedData.data.selectedGear);
		gearAttributes.SelectItem ("Gears", savedData.data.selectedGear);


		if (savedData.data.unlockedGears == null || savedData.data.unlockedGears.Count == 0){
			savedData.data.unlockedGears = new List<string>(){"casual"};
		}

	}

	void InitUIComponents(){
		str = GameObject.Find ("StrengthIndicator").GetComponent<UISlider> ();
		agi = GameObject.Find ("AgilityIndicator").GetComponent<UISlider> ();
		lck = GameObject.Find ("LuckIndicator").GetComponent<UISlider> ();

		money = GameObject.Find ("Money").GetComponent<UILabel> ();
		records = GameObject.Find ("StatusContent").GetComponent<UILabel> ();
		records.text = string.Join ("\n", savedData.data.records.Select(e => e+"s").ToArray ());

		character = GameObject.Find ("Character").GetComponent<SkeletonAnimation>();

		GameObject.Find ("Box").GetComponent <GiftBoxController> ().SetButton ();
		GameObject.Find("MoreMoney").GetComponent<UILabel>().text = "$100";
	}

	public void UpdateUIComponents(){

		// Setup gear attribtues

//		TreeNode selectedGearParts = gearParts.GetChild("Gears").GetChild(selectedGear);
//		TreeNode attributes = gearAttributes.GetChild("Gears").GetChild (selectedGear);

		TreeNode selectedGearParts = gearParts.GetSelected ();
		TreeNode attributes = gearAttributes.GetSelected ();

		str.value = int.Parse (attributes.children ["str"].leaf )/ MAX_ATTRIBUTE;
		agi.value = int.Parse (attributes.children ["agi"].leaf ) / MAX_ATTRIBUTE;
		lck.value = int.Parse (attributes.children ["lck"].leaf )/ MAX_ATTRIBUTE;

		// update balance

		money.text = savedData.data.balance.ToString ();

		// setup characters

		foreach(KeyValuePair<string, TreeNode> part in selectedGearParts.children){
			character.skeleton.SetAttachment (part.Key, part.Value.leaf);
		}

		// update box button

		GameObject.Find ("Box").GetComponent <GiftBoxController> ().SetButton ();

		GameObject.Find("MoreMoney").GetComponent<UILabel>().text = savedData.data.unlockedGears.Count * 200 + "";
		// update new item panel

	
	}
		
	// Use this for initialization
	void Start () {
		grid = GameObject.Find("GearGrid");

		savedData = GameObject.Find ("Data").GetComponent<PersistentData> ();

		newItemPanel = GameObject.Find ("NewItemPanel").GetComponent <NewItemPanelController> ();

		LoadData();
		LoadPersistentData ();
		InitializeGearData ();

		InitUIComponents ();
		gearParts.CreateComponents (grid, IsUnlocked, CreateButton);
		UpdateUIComponents ();

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
