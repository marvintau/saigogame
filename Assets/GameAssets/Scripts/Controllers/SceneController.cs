using UnityEngine;
using System.Collections;
using Prime31.TransitionKit;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour {

	public int theNextSceneBuildIndex = -1;

	private bool hasBeenSwitching = false;


	private FadeTransition fader;

	public void SetToSwitchScene(){
		if(!hasBeenSwitching){
			TransitionKit.instance.transitionWithDelegate( fader );	
			hasBeenSwitching = true;
		}
	}

	// Use this for initialization
	void Awake () {

		fader = new FadeTransition() {
			nextScene = theNextSceneBuildIndex,
			fadedDelay = 0.02f,
			fadeToColor = Color.black
		};

	}
	
	// Update is called once per frame
	void Update () {
	}
}
