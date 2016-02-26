using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class RetainedInformationHandler : MonoBehaviour {


	// Use this for initialization
	void Start () {
		float time = GameObject.Find ("RetainedInfo").GetComponent<RetainedInformation> ().recordTime;

		GameObject.Find("FinalScore").GetComponent<Text>().text = "Your final record is:\n" + time.ToString ("0.000") + " secs";

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
