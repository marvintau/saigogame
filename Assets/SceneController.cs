using UnityEngine;
using System.Collections;
using Prime31.TransitionKit;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour {

	public string theAssetBundleName;
	public string theNextSceneName;
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
	void Start () {

		fader = new FadeTransition() {
			assetBundleName = theAssetBundleName,
			nextSceneName = theNextSceneName,
			nextSceneBuildIndex = theNextSceneBuildIndex,
			fadedDelay = 0.02f,
			fadeToColor = Color.black
		};

	}
	
	// Update is called once per frame
	void Update () {
	}
}
