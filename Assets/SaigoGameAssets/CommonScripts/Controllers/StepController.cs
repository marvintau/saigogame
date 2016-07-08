using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StepController{

	public float staggerProb;

	private int currStep = 0;
	private int resetStepDelay = 0;

	public void NextStep(int newStep, SprintGameController ctrl, AtheleteController athe){

		this.resetStepDelay = 0;

		if (newStep == currStep){
			if (Random.value > staggerProb){
				athe.Stagger();	
			}
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

public class UncleStepController{
	private int currStep = 0;
	private int resetStepDelay = 0;

	public void NextStep(int newStep, LongJumpGameController ctrl, UncleController uncl){

		this.resetStepDelay = 0;

		if (newStep == currStep){
			this.currStep = 0;
		} else {
			ctrl.UpdateSpeedFactor (0.1f, 2.0f, true);
			uncl.SetSpeed (ctrl.speedFactor);
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

	public void CheckTap(SprintGameController ctrl, AtheleteController athe){
		if(Input.touchCount >0 && athe.status != Status.Staggering && athe.status != Status.Stopped){
			Touch touch = Input.GetTouch (0);

			if(touch.phase == TouchPhase.Began){
				if((touch.position.x - Screen.width * 0.75f) > 0){
					stepCtrl.NextStep (1, ctrl, athe);	
				} else if ((touch.position.x - Screen.width * 0.25) < 0){
					stepCtrl.NextStep (2, ctrl, athe);
				} else {
					ctrl.indiCtrl.SetBoost (true);
					//					ctrl.indiCtrl.boostDoses -= 1;
				}
			}
		}

	}
}

public class UncleInputController{
	public UncleStepController stepCtrl;

	public UncleInputController(UncleStepController stepCtrl){
		this.stepCtrl = stepCtrl;
	}

	public void CheckTap(LongJumpGameController ctrl, UncleController uncl){

		if(ctrl.beginJump && Input.GetKeyDown(KeyCode.D)){
			ctrl.StartJump ();
			ctrl.beginJump = false;
		}

		if (Input.GetKey(KeyCode.LeftArrow)){
			stepCtrl.NextStep (1, ctrl, uncl);
		} else if (Input.GetKey(KeyCode.RightArrow)){
			stepCtrl.NextStep (2, ctrl, uncl);
		}

		if(Input.touchCount >0 && uncl.status != Status.Stopped){
			Touch touch = Input.GetTouch (0);

			if(touch.phase == TouchPhase.Began){
				if((touch.position.x - Screen.width * 0.75f) > 0){
					stepCtrl.NextStep (1, ctrl, uncl);
				} else if ((touch.position.x - Screen.width * 0.25) < 0){
					stepCtrl.NextStep (2, ctrl, uncl);
				} else {
					ctrl.indiCtrl.SetBoost (true);
				}
			}
		}

	}

}