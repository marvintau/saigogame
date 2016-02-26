using UnityEngine;


using System.Collections;
using System.Collections.Generic;
using Spine;
using System.Linq;

// For general function pointer

public enum Status {Running, Staggering, Stopped};

public class AtheleteController : MonoBehaviour {

	public Status status;

	public bool shouldBeCounting = false;

	private SkeletonAnimation skeleton;
	private Slot slot;

	public float offset;

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

	}

	void Start () {
		this.status = Status.Stopped;
		skeleton.state.SetAnimation (0, "GET_SET", false);
	}

	public void StartRun(){
		this.status = Status.Stopped;
		skeleton.state.SetAnimation (0, "GET_SET", false);
		skeleton.state.AddAnimation (0, "READY", false, 0);
		skeleton.state.AddAnimation (0, "RUN", true, 0);
		skeleton.state.Event += BeginScrollEvent;
	}

	public void Stagger(){
		this.status = Status.Staggering;
		skeleton.state.SetAnimation (0, "STAGGER", false);
		skeleton.state.Event += StopScrollEvent;
		skeleton.state.AddAnimation (0, "GET_UP", false, 0);
		skeleton.state.AddAnimation (0, "RUN", true, 0);
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
	
