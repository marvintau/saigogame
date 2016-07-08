using UnityEngine;
using System.Collections;

public class BootStrapController : MonoBehaviour {

	void Awake() {
		Object.DontDestroyOnLoad (GameObject.Find ("Sound"));
	}

	// Use this for initialization
	void Start () {

		gameObject.GetComponent<SceneController> ().SetToSwitchScene ();

		int width = Screen.currentResolution.width;
		int height = Screen.currentResolution.height;
		Screen.SetResolution (Mathf.CeilToInt (width * 250f / Screen.dpi), Mathf.CeilToInt (height * 250f / Screen.dpi), true);

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
