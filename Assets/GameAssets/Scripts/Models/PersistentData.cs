using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Data {
	
	public List<float> records = new List<float>();


	// Store the selected Gears.
	public string selectedGear ="";

	public List<string> unlockedGears;

	public int balance = 0;

	public List<float> divingRecords = new List<float> ();
	public string diverSelectedGear = "";

}


public class PersistentData : MonoBehaviour {

	[HideInInspector]
	public TreeNode totalGearList;
	public TreeNode totalGearAttributes;

	public TreeNode chosenSequnece;
	public int chosenDifficulty = 3;

	public float latestDivingScore = 0;


	public bool IsUnlocked (string gearName){
		return data.unlockedGears.Contains (gearName);
	}

	public bool IsOkayToPurchase(){
		return (data.balance > data.unlockedGears.Count * 200 && data.unlockedGears.Count < 7);
	}

	public void AddUnlocked (string gearName){
		data.balance -= data.unlockedGears.Count * 200;
		data.unlockedGears.Add (gearName);
	}

	public void Select(string gearName){
		data.selectedGear = gearName;
	}

	// Store the latest game result
	public Data data = new Data();

	void Awake(){
		Object.DontDestroyOnLoad (gameObject);
	}

	// Use this for initialization
	void Start () {
		Debug.Log (Application.persistentDataPath);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
