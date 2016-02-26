using UnityEngine;
using System.Collections;

//[ExecuteInEditMode]
public class ProgressBarController : MonoBehaviour {

	Material barShader;

	public float boostFactor = 1.0f;

	private float boostTime = 3.0f;
	private float remainingBoostTime = 0;
	private int boostDoses = 1;
	private bool isBoosting = false;
	private float maxBoostFactor = 1.5f;

	void SetBoostDoses(int doses){
		this.boostDoses = doses;
	}

	void SetMaxBoostFactor(float maxBoostFactor){
		this.maxBoostFactor = maxBoostFactor;
	}

	public void SetWidth( float newWidth) {
		this.barShader.SetVector ("_Width", new Vector4 (newWidth, 0, 0, 0));
	}

	public void SetBoost (bool tof){
		this.isBoosting = tof;
		this.remainingBoostTime = this.boostTime;
	}

	public void SetBoostTime (float boostTime){
		this.boostTime = boostTime;
	}

	public void SetProgress (float progress) {
		this.barShader.SetVector ("_Progress", new Vector4(progress * 0.56f, 0, 0, 0));
	}


	public void UpdateBoostFactor(float delta, bool increase){
		if (increase) {
			if (this.boostFactor < this.maxBoostFactor) {
				this.boostFactor += delta;
			} else{
				this.boostFactor = this.maxBoostFactor;
			}
		} else{
			if (this.boostFactor > 1.0f) {
				this.boostFactor -= delta;
			} else
				this.boostFactor = 1.0f;
		}
	}

	public void UpdateBoostIndicator(){
		if (this.isBoosting && this.boostDoses > 0) {
			this.UpdateBoostFactor (0.05f, true);
		} else {
			this.UpdateBoostFactor (0.05f, false);
		}
		this.SetWidth (this.boostFactor);

	}

	public void UpdateProgres(float speedFactor){
		this.SetProgress ( (1.5f - speedFactor * this.boostFactor) / 1.5f);
	}

	// Use this for initialization
	void Start () {
		this.barShader = gameObject.GetComponent<Renderer> ().sharedMaterial;
	}
		
	// Update is called once per frame
	void Update () {
		if(this.remainingBoostTime > 0){
			this.remainingBoostTime -= Time.deltaTime;
		} else {
			this.remainingBoostTime = 0;
			this.isBoosting = false;
		}


		this.UpdateBoostIndicator ();
	}
}
