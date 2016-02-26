using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor (typeof (Trape))] 
public class TrapeEditor : Editor {
	[MenuItem ("GameObject/Create Other/Trape")]
	static void Create(){
		
		GameObject gameObject = new GameObject("Trape");
		Trape s = gameObject.AddComponent<Trape>();
		MeshFilter meshFilter = gameObject.GetComponent<MeshFilter>();
		meshFilter.mesh = new Mesh();
		s.Rebuild();
	}
	
	public override void OnInspectorGUI ()
	{
		Trape obj = target as Trape;
		
		if (obj == null)
		{
			return;
		}
		
		base.DrawDefaultInspector();
		EditorGUILayout.BeginHorizontal ();
		
		// Rebuild mesh when user click the Rebuild button
		if (GUILayout.Button("Rebuild")){
			obj.Rebuild();
		}
		EditorGUILayout.EndHorizontal ();
	}
}