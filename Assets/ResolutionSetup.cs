using UnityEngine;
using System.Collections;

public class ResolutionSetup : MonoBehaviour {

	public float ratio = 1.0f;
	public float standardDPI = 250f;

	// Use this for initialization
	void Start () {
		Screen.SetResolution (
			(int)(Screen.currentResolution.width * (standardDPI / Screen.dpi)),
			(int)(Screen.currentResolution.height * (standardDPI / Screen.dpi)),
			true
		);

		// All the method will be executed only one time, but the data
		// stored at "Retained Data" will be saved during transiting
		// between scenes.
		gameObject.GetComponent<SceneController> ().SetToSwitchScene ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
