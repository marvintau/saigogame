using UnityEngine;
using Spine;
using System.Collections;

[RequireComponent(typeof(SkeletonAnimation))]

public class SprinterController : MonoBehaviour {

	SkeletonAnimation skeletonAnimation;
	
	void Start () {
		skeletonAnimation = GetComponent<SkeletonAnimation>();

		skeletonAnimation.state.Event += HandleEvent;

		skeletonAnimation.state.SetAnimation(0, "get set", false);
		skeletonAnimation.state.AddAnimation(0, "block start", false, 0);
		skeletonAnimation.state.AddAnimation(0, "starting run", false, 0);
		skeletonAnimation.state.AddAnimation(0, "run", true, 0);
	}

	void HandleEvent (Spine.AnimationState state, int trackIndex, Spine.Event e) {
//		Debug.Log (trackIndex + " " + state.GetCurrent (trackIndex) + " " + e + " " + e.Int);
	}

}
