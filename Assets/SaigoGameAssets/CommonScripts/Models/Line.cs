using UnityEngine;
using System.Collections;
using System;

public class Line : MonoBehaviour {
	public Color c1 = Color.yellow;
	public Color c2 = Color.red;

	private GameObject lineObject;
	private TrailRenderer trailRenderer;
	private int vertexPointer = 0;

	void Start()
	{
		lineObject = GameObject.Find("Track");
		trailRenderer = lineObject.GetComponent<TrailRenderer>();
	}

	public void CreateLine(){

		Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 5);
		lineObject.transform.position = Camera.main.ScreenToWorldPoint(mousePos);

	}

	public void CreateLineTouch(){
		Vector3 touchPos = new Vector3 (Input.touches [0].position.x, Input.touches [0].position.y, 5);
		lineObject.transform.position = Camera.main.ScreenToWorldPoint(touchPos);
	}

	public void DestroyLine(){
		trailRenderer.Clear ();
	}

	void Update() {

	}
}