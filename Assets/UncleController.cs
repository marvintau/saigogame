using UnityEngine;

using Spine.Unity;
using System.Collections;
using System.Collections.Generic;

public class UncleController : MonoBehaviour {

	public Status status;

	public bool shouldBeCounting = false;

	public float offset;

	private SkeletonAnimation skeleton;

	Rigidbody2D body;

	PersistentData retainedInfo;


	public void BeginScrollEvent ( Spine.AnimationState state, int trackIndex, Spine.Event e){
		if ( e.ToString() == "BEGIN_SCROLL" ) {
			this.status = Status.Running;
			if (!shouldBeCounting)
				shouldBeCounting = true;
		}
	}
		
	void Awake(){
		skeleton = gameObject.GetComponent<SkeletonAnimation>();
		body = gameObject.GetComponent<Rigidbody2D> ();
		body.constraints = RigidbodyConstraints2D.FreezeAll;
//		retainedInfo = GameObject.Find ("Data").GetComponent<PersistentData> ();

		this.status = Status.Stopped;
		skeleton.state.TimeScale = 0f;
		skeleton.state.SetAnimation (0, "starting-fixed", false);

//		TreeNode selectedGear = retainedInfo.totalGearList.GetChild ("Gears").GetChild (retainedInfo.data.selectedGear);
//		foreach (KeyValuePair<string, TreeNode> part in selectedGear.children) {
//			skeleton.skeleton.SetAttachment (part.Key, part.Value.leaf);
//		}

	}

	void Start () {

	}
		
	public void UncleMove(float delta, float lim){

		if (gameObject.transform.localPosition.x < lim){
			gameObject.transform.Translate (delta, 0, 0);
		}

		this.offset = gameObject.transform.localPosition.x;

	}

	public void Stop(){
		this.status = Status.Stopped;
	}

	public void StartRun(){
		this.status = Status.Running;
		skeleton.state.TimeScale = 2f;
		skeleton.state.SetAnimation (0, "starting-fixed", false);
		skeleton.state.AddAnimation (0, "run-loop-fixed", true, 0);
		skeleton.state.Event += BeginScrollEvent;
	}

	public void Pause(){
		skeleton.timeScale = 0;
	}

	public void StartJump(){
		skeleton.timeScale = 0.8f;
		skeleton.state.SetAnimation (0, "jump-fixed", false);
		body.constraints = RigidbodyConstraints2D.FreezeRotation;
		body.AddForce (new Vector2(2f, 9f), ForceMode2D.Impulse);
	}

	public void SetSpeed(float speed){
		skeleton.timeScale = speed;
	}

	// Update is called once per frame
	void Update () {
	}
}

