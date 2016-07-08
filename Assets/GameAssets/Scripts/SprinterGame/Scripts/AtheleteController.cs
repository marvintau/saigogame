using UnityEngine;

using Spine.Unity;
using System.Collections;
using System.Collections.Generic;

public enum Status {Running, Staggering, Stopped};

public class AtheleteController : MonoBehaviour {

	public Status status;

	public bool shouldBeCounting = false;

	public float offset;

	private SkeletonAnimation skeleton;

	PersistentData retainedInfo;


	public void BeginScrollEvent ( Spine.AnimationState state, int trackIndex, Spine.Event e){
		if ( e.ToString() == "BEGIN_SCROLL" ) {
			this.status = Status.Running;
			if (!shouldBeCounting)
				shouldBeCounting = true;
		}
	}

	public void StopScrollEvent ( Spine.AnimationState state, int trackIndex, Spine.Event e){
		if (e.ToString() == "STOP_SCROLL") {
			this.status = Status.Stopped;
		}
	}

	void Awake(){
		skeleton = gameObject.GetComponent<SkeletonAnimation>();

		retainedInfo = GameObject.Find ("Data").GetComponent<PersistentData> ();

		this.status = Status.Stopped;
		skeleton.state.SetAnimation (0, "GetSet", false);

		TreeNode selectedGear = retainedInfo.totalGearList.GetChild ("Gears").GetChild (retainedInfo.data.selectedGear);
		foreach (KeyValuePair<string, TreeNode> part in selectedGear.children) {
			skeleton.skeleton.SetAttachment (part.Key, part.Value.leaf);
		}

	}

	void Start () {
		
	}

	public void Stop(){
		this.status = Status.Stopped;
	}

	public void StartRun(){
		this.status = Status.Stopped;
		skeleton.state.SetAnimation (0, "GetSet", false);
		skeleton.state.AddAnimation (0, "BlockStartFixed", false, 0);
		skeleton.state.AddAnimation (0, "TransitionFixed", false, 0);
		skeleton.state.AddAnimation (0, "Running", true, 0);
		skeleton.state.Event += BeginScrollEvent;
	}

	public void Stagger(){
		this.status = Status.Staggering;
		skeleton.state.SetAnimation (0, "Staggered", false);
		skeleton.state.Event += StopScrollEvent;
		skeleton.state.AddAnimation (0, "GetUpAfterStaggered", false, 0);
		skeleton.state.AddAnimation (0, "BlockStartFixed", false, 0);
		skeleton.state.AddAnimation (0, "TransitionFixed", false, 0);
		skeleton.state.AddAnimation (0, "Running", true, 0);
		skeleton.state.Event += BeginScrollEvent;
	}

	public void SetSpeed(float speed){
		skeleton.timeScale = speed;
	}
		

	public void AtheleteMove(float delta, float lim){

		if (gameObject.transform.localPosition.x < lim){
			gameObject.transform.Translate (delta, 0, 0);
		}

		this.offset = gameObject.transform.localPosition.x;

	}

	// Update is called once per frame
	void Update () {
	}
}
	
