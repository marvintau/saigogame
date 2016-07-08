using UnityEngine;
using System.Collections;

public class JumpCollideHandler : MonoBehaviour {

	// Use this for initialization
	void Awake () {

	}

	public void SwitchScene(){
		GameObject.FindObjectOfType<SceneController> ().SetToSwitchScene ();
	}

	void OnCollisionEnter2D(Collision2D collision) {
		GameObject.FindObjectOfType<UncleController> ().status = Status.Stopped;
		Invoke ("SwitchScene", 1f);
	}

}
