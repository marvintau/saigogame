using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class Debugger : MonoBehaviour {

	// Use this for initialization
	void Start () {
		gameObject.GetComponent<Text>().text = Screen.dpi.ToString() + " DPI";
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
