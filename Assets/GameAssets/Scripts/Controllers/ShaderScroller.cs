using UnityEngine;
using System.Collections;

public class ShaderScroller : MonoBehaviour {

	public float scrollSpeed;

	private Material m;


	// Use this for initialization
	void Start () {
		m = GetComponent<Renderer> ().material;
	}
	
	// Update is called once per frame
	void Update () {
		float x = Mathf.Repeat (Time.time * scrollSpeed, 1);
		m.SetVector ("_Scroll", new Vector4 (x, 0, 0, 0));
	}
}
