using UnityEngine;
using UnityEditor;
using System;
using System.Collections;

[CustomEditor (typeof (MovableController))]

public class CreateMovableController : Editor {

	[MenuItem ("GameObject/Create Other/MovableController")]

	static void Create(){

		// 1. Create a background controller
		// -------------------------------
		// Notice that a background controller is only responsible
		// for modifying background offset during runtime. All
		// creation work is done within this function.
		GameObject gameObject = new GameObject("MovableController");
		gameObject.AddComponent<MovableController>();

		// 2. Create background objects
		// -------------------------------
		// Read the configuration table, which contains the name,
		// offset, scroll speed and other possible paramters, and
		// generate the corresponding objects.


	}


	void CreateTextureAsset(string path, string name){

		TextureImporter textureImporter = AssetImporter.GetAtPath (path + "Textures/" + name + ".png") as TextureImporter;
		Debug.Log (textureImporter);
		textureImporter.textureType = TextureImporterType.Image;
		textureImporter.alphaIsTransparency = true;
		textureImporter.wrapMode = TextureWrapMode.Repeat;
		textureImporter.textureFormat = TextureImporterFormat.AutomaticCompressed;

		AssetDatabase.ImportAsset (path + "Textures/" + name + ".png");
	}

	void CreateMaterialAsset(string path, string name){
		Material material = new Material (Shader.Find ("NonAffineCorrected"));
		material.mainTexture = AssetDatabase.LoadAssetAtPath<Texture> (path + "Textures/" + name + ".png");
		AssetDatabase.CreateAsset (material, path + "Materials/" + name + ".mat");
	}

	void CreateAsset(string path, string name){
		CreateTextureAsset (path, name);
		CreateMaterialAsset (path, name);
	}

	void CreateCamera(){
		if(GameObject.FindObjectOfType<Camera>() != null){
			Debug.Log ("Camera existing");
			return;
		} else {
			GameObject camera = new GameObject ();
			camera.name = "Main Camera";
			camera.tag = "MainCamera";
	
			Camera c = camera.AddComponent<Camera> ();
			c.orthographic = true;
		}
	}

	void CreateObject(string path, string[] fields){
		// Check if the object has been created
		GameObject go;
		if((go = GameObject.Find(fields[0]))!= null){
			DestroyImmediate (go);
		}
			
		go = new GameObject ();
		MovableBackground mb = go.AddComponent<MovableBackground> ();
		mb.Setup (fields);

		Renderer r = mb.GetComponent<Renderer> ();
		r.material = AssetDatabase.LoadAssetAtPath<Material> (path + "Materials/" + fields [0] + ".mat");
	}


	// Reusable in future.

	public void CreateAssets(){

		string path = "Assets/GameAssets/Resources/";

		Indentum.ReadLines ( path + "backgrounds.txt", delegate(string line) {
			if(line.Length != 0 && line[0] != '#'){
				string[] fields = System.Text.RegularExpressions.Regex.Split (line, @"\s+");

				CreateAsset(path, fields[0]);
			}
		});
			
	}

	public void CreateObjects(){
		string path = "Assets/GameAssets/Resources/";

		CreateCamera ();

		Indentum.ReadLines (path + "backgrounds.txt", delegate(string line) {
			if (line.Length != 0 && line [0] != '#') {
				string[] fields = System.Text.RegularExpressions.Regex.Split (line, @"\s+");

				CreateObject (path, fields);
			}
		});
	}

	// Main entry of the inspector setup routine.
	override public void OnInspectorGUI(){
//		MovableController paraCtrl = target as MovableController;

		// Entering changing check area
		EditorGUI.BeginChangeCheck ();

		// For controllers
		EditorGUILayout.BeginHorizontal ();

		if (GUILayout.Button("Create Assets")){
			CreateAssets ();
		}

		if (GUILayout.Button("Create Objects")){
			CreateObjects ();
		}

		EditorGUILayout.EndHorizontal ();

		if (EditorGUI.EndChangeCheck ()) {
			serializedObject.ApplyModifiedProperties ();
		}
	}
}