using UnityEngine;
using UnityEngine.UI;
using System.Collections;

using UnityEngine.SceneManagement;

public class LongJumpGameController : MonoBehaviour {

	static float MAX_ATTRIBUTE = 35;
	float str;
	float agi;
	float lck;

	private float drillDistance = 0;

	private float time = 0;

	public float MaxDrillDistance = 500.0f;

	public float speedFactor = 0.0f;
	public float DPIratio = 1f;

	// Internal controllers (instiantiated from code and only used in this class)
	private UncleStepController stepCtrl;
	private UncleInputController inputCtrl;

	// External controllers (existing controller
	// belonging to other GameObjects)
	private UncleController unclCtrl;
	private MovableController moveCtrl;
	public  ProgressBarController indiCtrl;

	// Use this for initialization
	private bool tutDone = false;
	public bool beginJump = false;

	void Start(){
	}

	void SetupSpeed(){
//		PersistentData savedData = GameObject.FindObjectOfType <PersistentData> ();

//		TreeNode gearAttributes = savedData.totalGearAttributes;
//		TreeNode attributes = gearAttributes.GetChild("Gears").GetChild (savedData.data.selectedGear);

//		str = float.Parse (attributes.children ["str"].leaf )/ MAX_ATTRIBUTE;
//		agi = float.Parse (attributes.children ["agi"].leaf ) / MAX_ATTRIBUTE;
//		lck = float.Parse (attributes.children ["lck"].leaf )/ MAX_ATTRIBUTE;

//		indiCtrl.boostTime += str;

	}

	public void StartRoutine () {


		stepCtrl = new UncleStepController ();
		inputCtrl = new UncleInputController(stepCtrl);

		this.unclCtrl = GameObject.Find("Character").GetComponent<UncleController> ();
		this.unclCtrl.Stop ();

		this.moveCtrl = GameObject.Find("MovableController").GetComponent<MovableController> ();
		this.moveCtrl.Setup ();

		this.indiCtrl = GameObject.Find("Indicator").GetComponent<ProgressBarController> ();

		tutDone = true;
		Invoke ("StartRun", 0.0f);
		Invoke ("SetLineDisappear", 0.8f);
	}
		
	public void StartRun(){

		this.unclCtrl.StartRun ();
		SetupSpeed ();

	}

	public void SetLineDisappear(){
			this.moveCtrl.SetLineScroll (false);
	}

	public void UpdateSpeedFactor(float delta, float limit, bool increase){
		if (increase){
			if (this.speedFactor + delta < limit)
				this.speedFactor += delta;
			else
				this.speedFactor = limit;
		} else {
			if (this.speedFactor - delta > limit)
				this.speedFactor -= delta;	
			else
				this.speedFactor = limit;
		}
	}


	public void UpdateScroll(){
		this.moveCtrl.SetScrollSpeed (this.speedFactor * this.DPIratio *  this.indiCtrl.boostFactor);

		if(this.unclCtrl.status == Status.Running){
			this.moveCtrl.SetSandScroll (false);
			this.moveCtrl.SetToScroll (true);
		}
			
		if(this.drillDistance > MaxDrillDistance){
			this.moveCtrl.SetLineScroll (true);
			this.moveCtrl.SetSandScroll (true);

			if(!beginJump){
				beginJump = true;
			}
		}

		if(beginJump){
			moveCtrl.SetLineScroll (false);
			moveCtrl.SetSandScroll (false);
			moveCtrl.SetToScroll (false);
			unclCtrl.Pause();
		}
	}

	public void RecordTime(){		
		if(this.unclCtrl.shouldBeCounting){
			// Guarantee that this is executed only once.
//			GameObject.Find ("Data").GetComponent<PersistentData> ().data.records.Add (this.time);
		}
		this.unclCtrl.shouldBeCounting = false;
	}

	public void SwitchScene(){
		gameObject.GetComponent<SceneController> ().SetToSwitchScene ();
	}

	public void StartJump(){
		moveCtrl.SetSandScroll (true);
		moveCtrl.SetToScroll (true);
		unclCtrl.StartJump ();
	}

	public void CheckFinish(){
		
	}

	public void UpdateControllers(Status status){

		if (this.unclCtrl.status == Status.Stopped){
			this.UpdateSpeedFactor (0.05f, 0.00f, false);
		} else if (this.speedFactor < 1.0f){
			this.UpdateSpeedFactor (0.06f, 1.0f * this.indiCtrl.boostFactor * (1 + agi), true);
		} else {
			this.UpdateSpeedFactor (0.01f, 1.0f * this.indiCtrl.boostFactor * (1 + agi) , false);
			this.unclCtrl.SetSpeed (this.speedFactor);
		}

		this.indiCtrl.UpdateProgress (this.speedFactor);
		UpdateScroll ();

		CheckFinish ();
	}


	// Update is called once per frame
	void Update () {
		if(tutDone){
			this.drillDistance += this.speedFactor/2;

			this.UpdateControllers (this.unclCtrl.status);

			this.stepCtrl.UpdateStepDelay ();
			this.inputCtrl.CheckTap (this, this.unclCtrl);
			this.stepCtrl.ResetStep ();	
			Debug.Log (drillDistance);
		}
	}
}
