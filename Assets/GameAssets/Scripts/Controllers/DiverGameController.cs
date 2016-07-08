using UnityEngine;
using System.Collections;
using System.Runtime.CompilerServices;
using System.Linq;
using UnityEngine.SceneManagement;

public class DiverGameController : MonoBehaviour {

	Rigidbody2D bgBody, platformBody, charBody, waterFrontBody;

	Vector2 bgTransientSpeed, platformTransientSpeed;

	DiverPlayerController playerCtrl;

	ConnectorController connCtrl;

	SceneController sceneCtrl;

	PersistentData data;

	int countDown = 5;


	void Awake(){
		sceneCtrl = GameObject.FindObjectOfType<SceneController> ();
	
		connCtrl = GameObject.FindObjectOfType<ConnectorController> ();

		bgBody = GameObject.Find ("Background").GetComponent<Rigidbody2D> ();
		platformBody = GameObject.Find ("Platform").GetComponent<Rigidbody2D> ();
		charBody = GameObject.Find ("Character").GetComponent<Rigidbody2D> ();

		bgBody.constraints = RigidbodyConstraints2D.FreezeAll;
		platformBody.constraints = RigidbodyConstraints2D.FreezeAll;
		charBody.constraints = RigidbodyConstraints2D.FreezeAll;

		playerCtrl = GameObject.FindObjectOfType <DiverPlayerController> ();
		playerCtrl.character.state.Event += delegate(Spine.AnimationState state, int trackIndex, Spine.Event e) {
			
			if (e.ToString () == "Departing"){
				bgBody.constraints = RigidbodyConstraints2D.FreezeRotation;
				platformBody.constraints = RigidbodyConstraints2D.FreezeRotation;

				bgBody.AddForce (new Vector2(-50f, -2f), ForceMode2D.Force);
				platformBody.AddForce (new Vector2(-5f, -3f), ForceMode2D.Impulse);
				charBody.AddForce (new Vector2(0, 5f), ForceMode2D.Impulse);
			}
		};
			
		data = GameObject.FindObjectOfType <PersistentData> ();
	}

	void NextScene(){
		sceneCtrl.SetToSwitchScene ();
	}

	// Use this for initialization
	void Start () {
		playerCtrl.character.timeScale = 0;
		playerCtrl.SetupCharacterAction (data.chosenSequnece);

		InvokeRepeating ("UpdateCountDown", 0.0f, 0.5f);
	}

	void Pause () {
		bgTransientSpeed = bgBody.velocity;
		platformTransientSpeed = platformBody.velocity;

		bgBody.constraints = RigidbodyConstraints2D.FreezeAll;
		platformBody.constraints = RigidbodyConstraints2D.FreezeAll;

		playerCtrl.Pause ();
//		connCtrl.BeginConnect ();
	}

	void Resume(){
		bgBody.constraints = RigidbodyConstraints2D.FreezeRotation;
		platformBody.constraints = RigidbodyConstraints2D.FreezeRotation;

		bgBody.velocity = bgTransientSpeed;
		platformBody.velocity = platformTransientSpeed;

		playerCtrl.Resume ();
//		connCtrl.EndConnect ();
	}

	void ConnectBegin(){
		connCtrl.BeginConnect ();
		Pause ();
	}

	void GamePlayBegin(){
		playerCtrl.character.timeScale = 0.5f;

		Invoke ("ConnectBegin", 1.5f);
	}
		
	void UpdateCountDown(){
		if (--this.countDown == 0) {
			GamePlayBegin ();
		}
	}

	// Update is called once per frame
	void Update () {
		if(connCtrl.isConnectingDone || connCtrl.isConnectingFailed){
			connCtrl.isConnectingDone = false;
			connCtrl.isConnectingFailed = false;
			connCtrl.EndConnect ();
			Resume ();

			Invoke ("NextScene", 1.0f);
		}
	}
}
