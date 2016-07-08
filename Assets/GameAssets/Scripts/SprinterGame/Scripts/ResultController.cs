using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


public enum GameType
{
	Diving, Sprint
}

public class ResultController : MonoBehaviour {

	PersistentData retainedInfo;
	List<Record> records = new List<Record>();

	GameObject cupSprite;

	Sprite gold, silver, bronze;

	public GameType gameType;

	void SetSprintResult(){
		records.Add (new Record ("F. G-Joyner  ", 10.2));
		records.Add (new Record ("C. Jeter     ", 10.5));
		records.Add (new Record ("M. Jones     ", 11.0));
		records.Add (new Record ("S. F-Pryce   ", 12.0));
		records.Add (new Record ("C. Arron     ", 15.0));
	}

	void SetDivingResult(){
		records.Add (new Record ("R. Chen      ", 86.4));
		records.Add (new Record ("B. Broben    ", 75.5));
		records.Add (new Record ("P. Rinong    ", 70.5));
		records.Add (new Record ("M. Wu        ", 62.2));
		records.Add (new Record ("Y. Koltunova ", 56.1));
	}

	public void SetResult(){

		switch(gameType){
		case GameType.Diving:
			SetDivingResult ();
			break;
		case GameType.Sprint:
			SetSprintResult ();
			break;
		}

		records.Add (new Record ("You          ", Math.Round (retainedInfo.data.records [retainedInfo.data.records.Count-1], 2)));

		records.Sort( delegate (Record t1, Record t2) 
			{ return t1.time - t2.time > 0 ? 1 : ((t1.time == t2.time) ? 0 : -1); } 
		);

		for (int i = 0; i < records.Count; i++) {
			if (records[i].name == "You          "){
				retainedInfo.data.balance += (6 - i) * 100;
				Debug.Log (i);
				switch(i){
				case 0:
					Debug.Log (cupSprite.GetComponent<Image> ());
					cupSprite.GetComponent<Image> ().sprite = gold;
					break;
				case 1:
					cupSprite.GetComponent<Image> ().sprite = silver;
					break;
				case 2:
					cupSprite.GetComponent<Image> ().sprite = bronze;
					break;
				default:
					cupSprite.GetComponent<Image> ().sprite = null;
					cupSprite.GetComponent<Image> ().color = new Color (0, 0, 0, 0);
					break;
				}
			}

			GameObject.Find ("Result" + i).GetComponent<Text> ().text = records [i].name + records [i].time + "s";
		}

	}

	void SetCups(){
		List<Sprite> cups = Resources.LoadAll<Sprite> ("Textures/Result-Scene/result-atlas").ToList();

		for(int i = 0; i < cups.Count; i++){
			switch(cups[i].name){
			case "cup-bronze":
				bronze = cups [i];
				break;
			case "cup-silver":
				silver = cups [i];
				break;
			case "cup-gold":
				gold = cups [i];
				break;
			default:
				break;
			}
		}
	}
		
	// Use this for initialization
	void Start () {
		retainedInfo = GameObject.FindObjectOfType<PersistentData> ();

		cupSprite = GameObject.Find ("CupSprite");
		SetCups ();
		SetResult ();
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

	void SaveAndJump(){
		retainedInfo.data.records = retainedInfo.data.records.Take (5).ToList();
		retainedInfo.data.records.Sort ();
		Debug.Log (retainedInfo.data.records);
		SaveLoadController.Save ();

		gameObject.GetComponent<SceneController> ().SetToSwitchScene ();
	}

	void Update(){
		if(Input.touchCount == 1 || Input.GetKeyDown(KeyCode.D)){
//			gameObject.GetComponent<SceneController> ().SetToSwitchScene ();
			JumpScene ();
		}
	}
}
