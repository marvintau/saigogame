using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GearsLoader : MonoBehaviour {

	GameObject grid;

	void CreateSpriteButton(string type, string gear, TreeNode parts, UIAtlas atlas, string spriteName) {
		
		GameObject gearButtonObject = NGUITools.AddChild(grid);
		gearButtonObject.name = type + "-" + gear;

		UISprite gearSprite = gearButtonObject.AddComponent<UISprite> ();
		gearSprite.atlas = atlas;
		gearSprite.spriteName = spriteName;
		gearSprite.type = UIBasicSprite.Type.Sliced;
		gearSprite.localSize.Set (100, 300);

		UIButton gearButton = gearButtonObject.AddComponent<UIButton> ();
		gearButton.onClick.Add (delegate() {
			Debug.Log ("asd");
		});
	}

	void LoadGears () {

		grid = GameObject.Find("Grid");

		string atlasPath = "UI/Wooden Atlas";
		                   
		GameObject atlasObject = (GameObject) Resources.Load (atlasPath);
		UIAtlas textureAtlas = atlasObject.GetComponent<UIAtlas> ();
		Debug.Log (textureAtlas);

		string basePath = "Assets/GameAssets/Resources/";

		TreeNode gears = Indentum.ParseTree(basePath + "gears.txt");
		Debug.Log(gears.GetChild("Topwear").key);

		foreach (KeyValuePair<string, TreeNode> type in gears.children) {
			string typeName = type.Key;
			foreach (KeyValuePair<string, TreeNode> gear in type.Value.children) {
				string gearName = gear.Key;

				CreateSpriteButton (typeName, gearName, gear.Value, textureAtlas, "Window");
			}
		}

		grid.GetComponent<UIGrid> ().Reposition ();
	}

	// Use this for initialization
	void Start () {
		float time = GameObject.Find ("RetainedData").GetComponent<RetainedInformation> ().recordTime;

		LoadGears();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
