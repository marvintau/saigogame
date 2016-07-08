using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

public class Record {
	public string name;
	public double time;

	public Record(string name, double time){
		this.name = name;
		this.time = time;
	}
}

public class ResultSceneController : MonoBehaviour {

	PersistentData retainedInfo;
	List<Record> records = new List<Record>();

	string LoadFakeRecords(){
		records.Add (new Record ("F. Griffith-Joyner", 10.2));
		records.Add (new Record ("C. Jeter", 10.5));
		records.Add (new Record ("M. Jones", 11.0));
		records.Add (new Record ("S. Fraser-Pryce", 12.0));
		records.Add (new Record ("C. Arron", 15.0));
		records.Add (new Record ("You", Math.Round (retainedInfo.data.records [retainedInfo.data.records.Count-1], 2)));

		records.Sort( delegate (Record t1, Record t2) 
			{ return t1.time - t2.time > 0 ? 1 : ((t1.time == t2.time) ? 0 : -1); } 
		);

		for (int i = 0; i < records.Count; i++) {
			if (records[i].name == "You"){
				retainedInfo.data.balance += (6 - i) * 100;
			}
		}

		string str = "";
		foreach (Record rec in records){
			str += rec.name + " " + rec.time + "\n";
		}
		return str;
	}

	// Use this for initialization
	void Start () {
		retainedInfo = GameObject.Find ("Data").GetComponent<PersistentData> ();
		Data data = SaveLoadController.Load ();
		Debug.Log (data.selectedGear);
		GameObject.Find ("FinalScore").GetComponent<Text> ().text = LoadFakeRecords ();

	}

	void SaveAndJump(){
		retainedInfo.data.records = retainedInfo.data.records.Take (5).ToList();
		retainedInfo.data.records.Sort ();
		Debug.Log (retainedInfo.data.records);
		SaveLoadController.Save ();

		gameObject.GetComponent<SceneController> ().SetToSwitchScene ();
	}

	void JumpScene(){
		
		if(Input.touchCount == 1){
			Touch touch = Input.GetTouch (0);

			if(touch.phase == TouchPhase.Ended){
				SaveAndJump ();
			}
		}

		if (Input.GetKeyDown(KeyCode.D)){
			SaveAndJump ();
		}
	}

	// Update is called once per frame
	void Update () {
		JumpScene ();
	}
}
