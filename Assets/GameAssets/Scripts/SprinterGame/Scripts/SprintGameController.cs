using UnityEngine;
using UnityEngine.UI;
using System.Collections;

using UnityEngine.SceneManagement;

public class SprintGameController : MonoBehaviour {

	static float MAX_ATTRIBUTE = 35;
	float str;
	float agi;
	float lck;

	private float milage = 0;
	private float maxMilage = 50;

	private float time = 0;

	public float speedFactor = 0.0f;
	public float DPIratio = 1f;

	// Internal controllers (instiantiated from code and only used in this class)
	private StepController stepCtrl;
	private InputController inputCtrl;

	// External controllers (existing controller
	// belonging to other GameObjects)
	private AtheleteController atheCtrl;
	private MovableController moveCtrl;
	public  ProgressBarController indiCtrl;

	private UILabel timerCtrl;
	private UILabel countDownCtrl;

	private int countDown = 7;
	// Use this for initialization
	private bool tutDone = false;

	void Start(){
	}

	void SetupSpeed(){
		PersistentData savedData = GameObject.FindObjectOfType <PersistentData> ();

		TreeNode gearAttributes = savedData.totalGearAttributes;
		TreeNode attributes = gearAttributes.GetChild("Gears").GetChild (savedData.data.selectedGear);

		str = float.Parse (attributes.children ["str"].leaf )/ MAX_ATTRIBUTE;
		agi = float.Parse (attributes.children ["agi"].leaf ) / MAX_ATTRIBUTE;
		lck = float.Parse (attributes.children ["lck"].leaf )/ MAX_ATTRIBUTE;

		indiCtrl.boostTime += str;

	}

	public void StartRoutine () {


		stepCtrl = new StepController ();
		stepCtrl.staggerProb = lck;

		inputCtrl = new InputController(stepCtrl);

		this.atheCtrl = GameObject.Find("Character").GetComponent<AtheleteController> ();
		this.atheCtrl.Stop ();

		this.moveCtrl = GameObject.Find("MovableController").GetComponent<MovableController> ();
		this.moveCtrl.Setup ();

		this.indiCtrl = GameObject.Find("Indicator").GetComponent<ProgressBarController> ();
		this.timerCtrl = GameObject.Find ("Timer").GetComponent<UILabel> ();
		this.countDownCtrl = GameObject.Find ("CountDown").GetComponent<UILabel> ();

		tutDone = true;
		InvokeRepeating ("UpdateCountDown", 0.0f, 0.5f);
		PlaySound ("countdown");
	}

	public void PlaySound(string name){
		AudioSource au = GameObject.Find("Sound").GetComponent<AudioSource> ();
		au.clip = Resources.Load ("sfx/" + name) as AudioClip;
		au.Play ();
	}

	public void UpdateCountDown(){

		if (--this.countDown == 0) {
			this.countDownCtrl.text = "Bang!";
		} else if (this.countDown == 1){
			this.countDownCtrl.text = (this.countDown/2+1).ToString ();
			this.atheCtrl.StartRun ();
			SetupSpeed ();

		}else if (this.countDown == -1){
			this.countDownCtrl.text = "";
			CancelInvoke ("UpdateCountDown");
		} else{
			this.countDownCtrl.text = (this.countDown/2+1).ToString ();	
		}

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

		if(this.atheCtrl.status == Status.Running){
			this.moveCtrl.SetToScroll (true);
		}

		if(this.time > 0.8f){
			this.moveCtrl.SetLineScroll (false);
		}

		if(this.atheCtrl.status == Status.Stopped){
			this.moveCtrl.SetToScroll (false);
		}

		if(this.maxMilage - this.milage <= 15){
			this.moveCtrl.SetLineScroll (true);
		}
	}

	public void RecordTime(){		
		if(this.atheCtrl.shouldBeCounting){
			// Guarantee that this is executed only once.
			GameObject.Find ("Data").GetComponent<PersistentData> ().data.records.Add (this.time);
			PlaySound ("applau");
		}
		this.atheCtrl.shouldBeCounting = false;
	}

	public void TimerControl(){
		if(this.atheCtrl.shouldBeCounting){
			this.time += Time.deltaTime;
			this.timerCtrl.text = (this.time.ToString ("0.00") + "s");
		}
	}

	public void CheckFinish(){
		if(this.milage > this.maxMilage){
			this.moveCtrl.SetToScroll (false);
			this.atheCtrl.AtheleteMove (this.speedFactor * 0.08f, 30.0f);
			RecordTime ();

			if (this.atheCtrl.offset >= 10.0f){
				gameObject.GetComponent<SceneController> ().SetToSwitchScene ();
			}
		}

		TimerControl();
	}

	public void UpdateControllers(Status status){

		if (this.atheCtrl.status == Status.Stopped){
			this.UpdateSpeedFactor (0.05f, 0.00f, false);
		} else if (this.speedFactor < 1.0f){
			this.UpdateSpeedFactor (0.06f, 1.0f * this.indiCtrl.boostFactor * (1 + agi), true);
		} else {
			this.UpdateSpeedFactor (0.005f, 1.0f * this.indiCtrl.boostFactor * (1 + agi) , false);
			this.atheCtrl.SetSpeed (this.speedFactor);
		}

		this.indiCtrl.UpdateProgress (this.speedFactor);
		Debug.Log (indiCtrl.isBoosting);
		UpdateScroll ();

		CheckFinish ();
	}

	
	// Update is called once per frame
	void Update () {
		if(tutDone){
			this.milage += this.speedFactor/2;

			this.UpdateControllers (this.atheCtrl.status);

			this.stepCtrl.UpdateStepDelay ();
			this.inputCtrl.CheckTap (this, this.atheCtrl);
			this.stepCtrl.ResetStep ();	
		}

	}
}