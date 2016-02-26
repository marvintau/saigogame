using UnityEngine;
using UnityEngine.UI;
using System.Collections;

using Prime31.TransitionKit;
using UnityEngine.SceneManagement;

public class OverallController : MonoBehaviour {

	private float milage = 0;
	private float maxMilage = 1300;

	private float time = 0;

	public float speedFactor = 1.0f;

	// Internal controllers (instiantiated from code and only used in this class)
	private StepController stepCtrl;
	private InputController inputCtrl;

	// External controllers (existing controller
	// belonging to other GameObjects)
	private AtheleteController atheCtrl;
	private MovableController moveCtrl;
	public  ProgressBarController progCtrl;
	private Text speedCtrl;
	private Text timerCtrl;
	private Text countDownCtrl;

	private int countDown = 7;
	// Use this for initialization
	void Start () {

		stepCtrl = new StepController ();
		inputCtrl = new InputController(stepCtrl);

		this.atheCtrl = GameObject.Find("Athelete").GetComponent<AtheleteController> ();
		this.moveCtrl = GameObject.Find("MovableController").GetComponent<MovableController> ();
		this.moveCtrl.Setup ();

		this.progCtrl = GameObject.Find("ProgressBarController").GetComponent<ProgressBarController> ();
		this.speedCtrl = GameObject.Find("SpeedIndicator").GetComponent<Text> ();
		this.timerCtrl = GameObject.Find ("Timer").GetComponent<Text> ();
		this.countDownCtrl = GameObject.Find ("CountDown").GetComponent<Text> ();

		InvokeRepeating ("UpdateCountDown", 0.0f, 0.5f);
	}

	public void UpdateCountDown(){

		if (--this.countDown == 0) {
			this.countDownCtrl.text = "Bang!";
		} else if (this.countDown == 1){
			this.countDownCtrl.text = (this.countDown/2+1).ToString ();
			this.atheCtrl.StartRun ();
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

	public void UpdateControllers(Status status){

		if (this.atheCtrl.status == Status.Stopped){
			this.UpdateSpeedFactor (0.05f, 0.00f, false);
		} else if (this.speedFactor < 1.0f){
			this.UpdateSpeedFactor (0.06f, 1.0f * this.progCtrl.boostFactor, true);
		} else {
			this.UpdateSpeedFactor (0.005f, 1.0f * this.progCtrl.boostFactor , false);
			this.atheCtrl.SetSpeed (this.speedFactor);
		}

		this.moveCtrl.SetScrollSpeed (this.speedFactor * this.progCtrl.boostFactor);

		this.speedCtrl.text = (this.speedFactor * this.progCtrl.boostFactor * 5.1).ToString ("0.00") + " m/s";

//		this.progCtrl.SetProgress ( (1.5f - this.speedFactor * this.progCtrl.boostFactor) / 1.5f);
		this.progCtrl.UpdateProgres (this.speedFactor);

		if(this.atheCtrl.status == Status.Running){
			this.moveCtrl.SetToScroll (true);
		}

//		if(this.time > 0.7f){
//			this.moveCtrl.SetLineScroll (false);
//		}

		if(this.atheCtrl.status == Status.Stopped){
			this.moveCtrl.SetToScroll (false);
		}

//		if(this.maxMilage - this.milage <= 100){
//			this.moveCtrl.SetLineScroll (true);
//		}

//		if(this.maxMilage - this.milage <= 10){
//			this.moveCtrl.SetLineScroll (false);
//		}


		if(this.milage >= this.maxMilage){
			this.moveCtrl.SetToScroll (false);
			this.atheCtrl.AtheleteMove (this.speedFactor * 0.2f, 30.0f);

			this.atheCtrl.shouldBeCounting = false;
			GameObject.Find ("Retained Data").GetComponent<RetainedInformation> ().recordTime = this.time;

			if (this.atheCtrl.offset >= 30.0f){
//				var fader = new FadeTransition()
//				{
//					nextScene = 3,
//					fadedDelay = 0.2f,
//					fadeToColor = Color.black
//				};
//				TransitionKit.instance.transitionWithDelegate( fader );
			}
		}

		if(this.atheCtrl.shouldBeCounting){
			this.time += Time.deltaTime;
			this.timerCtrl.text = (this.time.ToString ("0.00") + "s");
		}
	}

	
	// Update is called once per frame
	void Update () {
		this.milage += this.speedFactor;

		this.UpdateControllers (this.atheCtrl.status);

		this.stepCtrl.UpdateStepDelay ();
		this.inputCtrl.CheckTap (this, this.atheCtrl);
		this.stepCtrl.ResetStep ();

	}
}

public class StepController{

	private int currStep = 0;
	private int resetStepDelay = 0;

	public void NextStep(int newStep, OverallController ctrl, AtheleteController athe){

		this.resetStepDelay = 0;

		if (newStep == currStep){
			athe.Stagger();
			this.currStep = 0;
		} else {
			ctrl.UpdateSpeedFactor (0.1f, 2.0f, true);
			athe.SetSpeed (ctrl.speedFactor);
			this.currStep = newStep;
		}
	}

	public void UpdateStepDelay(){
		this.resetStepDelay++;
	}

	public void ResetStep(){
		if (this.resetStepDelay > 20)
			this.currStep = 0;
	}
}

public class InputController{

	public StepController stepCtrl;

	// Should place all affected controller here.
	public InputController(StepController stepCtrl){
		this.stepCtrl = stepCtrl;
	}

	public void CheckTap(OverallController ctrl, AtheleteController athe){
		if(Input.touchCount == 1 && athe.status != Status.Staggering && athe.status != Status.Stopped){
			Touch touch = Input.GetTouch (0);

			if(touch.phase == TouchPhase.Began){
				if((touch.position.x - Screen.width * 0.75f) > 0){
					stepCtrl.NextStep (1, ctrl, athe);	
				} else if ((touch.position.x - Screen.width * 0.25) < 0){
					stepCtrl.NextStep (2, ctrl, athe);
				} else {
					ctrl.progCtrl.SetBoost (true);
				}
			}
		}

		if (Input.GetKeyDown (KeyCode.LeftArrow) && athe.status != Status.Staggering) {
			stepCtrl.NextStep (1, ctrl, athe);
		} else if (Input.GetKeyDown (KeyCode.RightArrow) && athe.status != Status.Staggering){
			stepCtrl.NextStep (2, ctrl, athe);
		}

		if (Input.GetKeyDown(KeyCode.D)){
			ctrl.progCtrl.SetBoost (true);
		}
	}
}
