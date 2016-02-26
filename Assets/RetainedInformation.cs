using UnityEngine;
using System.Collections;

public class RetainedInformation : MonoBehaviour {

	// Store the latest game result
	public float recordTime = 0;

	// Store the selected Gears. Temporarily hard coded.
	public

	void Awake(){
		Object.DontDestroyOnLoad (gameObject);
	}

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
