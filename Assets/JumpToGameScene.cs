using UnityEngine;
using System.Collections;
using Prime31.TransitionKit;
using UnityEngine.SceneManagement;

public class JumpToGameScene : MonoBehaviour {

	public void Jump(){
		// Debug.Log ("Beep");
		gameObject.GetComponent<SceneController> ().SetToSwitchScene ();
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	}
}
