using UnityEngine;
using System.Collections;

//[ExecuteInEditMode]
using UnityEngine.UI;


public class ProgressBarController : MonoBehaviour {

	// We are not going to use shader anymore.
	//	Material barShader;
	UIProgressBar indicator;
	UIWidget backWidget, bodyWidget, maskWidget;

	float width;

	[HideInInspector]
	public float boostFactor = 1.0f;
	public float boostTime = 2.0f;
	public int boostDoses = 1;


	private float remainingBoostTime = 0;
	public bool isBoosting = false;
	private float maxBoostFactor = 1.5f;

	void SetBoostDoses(int doses){
		this.boostDoses = doses;
	}

	void SetMaxBoostFactor(float maxBoostFactor){
		this.maxBoostFactor = maxBoostFactor;
	}

	public void SetWidth( float newWidth) {
//		this.barShader.SetVector ("_Width", new Vector4 (newWidth, 0, 0, 0));
		backWidget.width = (int)(width * newWidth);
		bodyWidget.width = (int)(width * newWidth);
		maskWidget.width = (int)(width * newWidth);

	}

	public void SetBoost (bool tof){
		this.isBoosting = tof;
		this.remainingBoostTime = this.boostTime;
	}

	public void SetBoostTime (float boostTime){
		this.boostTime = boostTime;
	}

	public void SetProgress (float progress) {
//		this.barShader.SetVector ("_Progress", new Vector4(progress * 0.56f, 0, 0, 0));
		indicator.value = progress *.56f;
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

	public void UpdateProgress(float speedFactor){
		this.SetProgress ( speedFactor / this.boostFactor);
	}

	// Use this for initialization
	void Start () {
//		this.barShader = gameObject.GetComponent<Renderer> ().sharedMaterial;
		indicator = gameObject.GetComponent <UIProgressBar>();


		backWidget = indicator.backgroundWidget;
		bodyWidget = indicator.foregroundWidget;
		maskWidget = GameObject.Find ("Indicator-mask").GetComponent <UI2DSprite> ();

		width = backWidget.width;
	}
		
	// Update is called once per frame
	void Update () {
		if(this.remainingBoostTime > 0){
			this.remainingBoostTime -= Time.deltaTime;
		} else {
			this.remainingBoostTime = 0;
			if (this.isBoosting){
				this.boostDoses -= 1;
			}
			this.isBoosting = false;
		}


		this.UpdateBoostIndicator ();
	}
}
