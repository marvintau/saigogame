using UnityEngine;
using System.Collections;
using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;
using System;

public class ConnectorController : MonoBehaviour {

	PersistentData data;

	Dictionary<string, Sprite> parentSprites;
	List<Sprite> sprites;
	List<Sprite> selected;

	GameObject[] fores;
	GameObject[] conns;

	UIProgressBar timeIndicator;

	List<String> connectedIcon;

	Line lineCtrl;

	bool isConnecting = false;
	bool isNewConnection = true;

	public bool isConnectingDone = false;
	public bool isConnectingFailed = false;

	public float time = 10f;

	void Awake(){

		data = GameObject.FindObjectOfType<PersistentData> ();

		timeIndicator = GameObject.FindObjectOfType<UIProgressBar> ();

		parentSprites = Resources.LoadAll<Sprite> ("Textures/Diving-Game-Scene/diving-play").ToDictionary (x=>x.name, x=>x);
		sprites = Resources.LoadAll<Sprite> ("Textures/Diving-Game-Scene/diving-play-icons").ToList ();
		lineCtrl = GameObject.FindObjectOfType <Line> ();
		connectedIcon = new List<string> ();
	}

	public void SetDifficulty(int difficulty){
		System.Random rnd = new System.Random();
		selected = sprites.OrderBy (x => rnd.Next ()).Take (difficulty).ToList ();

		fores = GameObject.FindGameObjectsWithTag ("ForeIcon").OrderBy (x => x.name.Split ('-') [1]).ToArray ();
		conns = GameObject.FindGameObjectsWithTag ("ConnIcon");

		for(int i = 0; i < 5; i++){
			if(i>=difficulty){
				fores[i].GetComponent<SpriteRenderer> ().color = new Color (1f, 1f, 1f, 0f);
				fores[i].transform.parent.gameObject.GetComponent <SpriteRenderer>().color = new Color (1f, 1f, 1f, 0f);

				conns[i].GetComponent<SpriteRenderer> ().color = new Color (1f, 1f, 1f, 0f);
				conns[i].transform.parent.gameObject.GetComponent <SpriteRenderer>().color = new Color (1f, 1f, 1f, 0f);
			} else {
				fores [i].GetComponent<SpriteRenderer> ().sprite = selected [i];
				fores [i].name = selected[i].name;

				conns [i].GetComponent<SpriteRenderer> ().sprite = selected [i];
				conns [i].name = selected [i].name;
			}
		}

		conns = conns.OrderBy (x => rnd.Next ()).ToArray ();

		for (int i = 0; i < 5; i++) {
			if(i < difficulty)
				conns[i].transform.parent.position = new Vector3(3f * Mathf.Sin (2*Mathf.PI*i/difficulty - 0.1f), 3f * Mathf.Cos (2*Mathf.PI*i/difficulty - 0.1f), -10);
		}

	}

	public void BeginConnect(){
		isConnecting = true;

		timeIndicator.alpha = 1f;
		gameObject.GetComponentsInChildren<SpriteRenderer> ().ToList ().ForEach (elem => elem.color = new Color(1f, 1f, 1f, 1f));
		SetDifficulty (data.chosenDifficulty);
	}


	public void EndConnect(){
		isConnecting = false;

		timeIndicator.alpha = 0f;
		gameObject.GetComponentsInChildren<SpriteRenderer> ().ToList ().ForEach (elem => elem.color = new Color(1f, 1f, 1f, 0f));
	}

	// Use this for initialization
	void Start () {
		timeIndicator.alpha = 0f;
		gameObject.GetComponentsInChildren<SpriteRenderer> ().ToList ().ForEach (elem => elem.color = new Color(1f, 1f, 1f, 0f));

	}

	void Reset(){
		isNewConnection = false;

		lineCtrl.DestroyLine ();

		connectedIcon.Clear ();

		foreach (var elem in conns) {
			elem.transform.parent.gameObject.GetComponent<SpriteRenderer> ().sprite = parentSprites["normal"];
		}
	}

	void UpdateLineDetection(bool isMouse){

		lineCtrl.CreateLine ();

		RaycastHit hitInfo = new RaycastHit ();
		bool hit = Physics.Raycast (Camera.main.ScreenPointToRay (Input.mousePosition), out hitInfo);
		if (hit && isNewConnection) {
			if (hitInfo.transform.gameObject.tag == "ConnIcon") {
				Debug.Log ("hitObject.name");
				GameObject hitObject = hitInfo.transform.gameObject;

				if (connectedIcon.Count == 0 || connectedIcon.Last () != hitObject.name) {
					connectedIcon.Add (hitObject.name);
					Debug.Log (hitObject.name);

					if (connectedIcon.Last () == selected [connectedIcon.Count - 1].name) {
						hitObject.transform.parent.gameObject.GetComponent  <SpriteRenderer> ().sprite = parentSprites ["correct"];

						if (connectedIcon.Count == selected.Count) {
							isConnecting = false;
							isConnectingDone = true;
							lineCtrl.DestroyLine ();
						}

					} else {
						hitObject.transform.parent.gameObject.GetComponent  <SpriteRenderer> ().sprite = parentSprites ["false"];
						lineCtrl.DestroyLine ();

						Invoke ("Reset", 0.3f);
					}
				}

			}
		}
	}

	void NewLineDetection(){
		isNewConnection = true;

	}

	// Update is called once per frame
	void Update (){
		if(isConnecting){

			time -= Time.deltaTime;

			timeIndicator.value = time / 10f;

			if (Input.GetMouseButton (0)) {
				UpdateLineDetection (true);

			} 

			if (Input.GetMouseButtonUp (0)) {
				NewLineDetection ();
			}

			if(Input.touchCount > 0){
				UpdateLineDetection (false);
			} else {
				NewLineDetection ();
			}

			if (time < 0 && !isConnectingDone){
				lineCtrl.DestroyLine ();
				isConnecting = false;
				isConnectingFailed = true;
			}
		} else {
			data.latestDivingScore = time * 3 * data.chosenDifficulty;
		}
	}
		
}
