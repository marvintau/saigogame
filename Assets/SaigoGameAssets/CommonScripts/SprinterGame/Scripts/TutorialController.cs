using UnityEngine;
using System.Collections;

public class TutorialController : MonoBehaviour {


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.touchCount == 1 || Input.GetKeyDown(KeyCode.D)){
			GameObject.FindObjectOfType<SprintGameController>().StartRoutine ();
			Destroy (gameObject);
		}
	}
}
