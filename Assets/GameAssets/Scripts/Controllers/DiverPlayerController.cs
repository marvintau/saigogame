using UnityEngine;
using System.Collections;
using System.Linq;

using Spine.Unity;
using Spine;
using UnityEngine.Networking.NetworkSystem;

public class DiverPlayerController : MonoBehaviour {

//	[HideInInspector]
	public SkeletonAnimation character;

	Spine.AnimationState charState;
	TreeNode sequence;

	// For rotating
	bool rotationMark = false;
	bool finishingAnimationAdded = false;

	// For falling
	Rigidbody2D body;
	Vector2 transientSpeed;

	// For animation playing
	public bool replay = true;
	public bool fadeOut = true;
	bool replayMark = false;
	bool fadeOutMark = false;
	bool forceMark = false;

	float currRotateAngle = 0f;
	float totalRotateAngle = 0f;
	float rotateDelta = 0f;

	Vector3 charPos;

	Vector3 z = new Vector3 (0, 0, 1);
	Vector3 originalPosition;


	void UpdateTransform(){
		if(rotationMark){

			if ((Mathf.Abs (currRotateAngle) > Mathf.Abs (totalRotateAngle) || Mathf.Abs(totalRotateAngle) == 0) && !finishingAnimationAdded) {
				finishingAnimationAdded = true;
				charState.AddAnimation (0, sequence.GetChild ("Finish").leaf, false, 0);
			}

			if (Mathf.Abs (currRotateAngle) > Mathf.Abs (totalRotateAngle)){
				rotationMark = false;
				return;
			}

			// 1. Accumulate the increment
			currRotateAngle += rotateDelta;

			// 4. Do the transform
			charPos = character.skeleton.RootBone.GetWorldPosition (gameObject.transform);
			gameObject.transform.RotateAround (charPos, z, rotateDelta);

		}
	}

	void UpdateFinishing(){
		if (fadeOut && fadeOutMark){
			character.skeleton.a -= 0.05f;
		}

		if (character.skeleton.a <= 0){
			if(replay){
				replayMark = true;	
			}

			fadeOutMark = false;
		}

	}

	void AddTransform(){

		currRotateAngle = 0f;
		totalRotateAngle = 0f;
		rotateDelta = 0f;
		rotationMark = false;


		float loops = float.Parse (sequence.GetChild ("Rot").leaf);

		totalRotateAngle = loops * 360;

		rotateDelta = Mathf.Sign (totalRotateAngle) * 7f * Mathf.Abs (loops);

//		body.gravityScale = 0.05f + 0.005f * Mathf.Abs (loops);

		charState.Start += delegate { character.skeleton.SetToSetupPose(); };

		charState.Event += delegate(Spine.AnimationState state, int trackIndex, Spine.Event e) {
			if (e.ToString () == "BeginRotate") {
				rotationMark = true;
			}
		};

		charState.Event += delegate(Spine.AnimationState state, int trackIndex, Spine.Event e) {
			if (e.ToString () == "Departing"){
				body.constraints = RigidbodyConstraints2D.FreezeRotation;
				if (!forceMark){
					body.AddForce (new Vector2(0, 2f), ForceMode2D.Impulse);
					forceMark = true;
				}
			}
		};

		charState.Complete += delegate(Spine.AnimationState state, int trackIndex, int loopCount) {
			if (state.GetCurrent (0).animation.name.Split ('-').Last () == "Open"){
				body.constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionX;

				body.gravityScale = 0.2f;
				fadeOutMark = true;
				forceMark = false;
			}
		};

	}

	void Inverse(){
		bool isInverse = sequence.GetChild ("Inverse").leaf == "True";

		Vector3 newScale = gameObject.transform.localScale;
		newScale.x = isInverse ? -Mathf.Abs (newScale.x) : Mathf.Abs (newScale.x);
		gameObject.transform.localScale = newScale;
	}

	void AddStartingAnimation(){
		charState.TimeScale = 0.5f;
		charState.SetAnimation (0, sequence.GetChild ("Set").leaf, false);
	}

	void AddJumpAnimation(){
		charState.TimeScale = 1f;
		charState.AddAnimation (0, sequence.GetChild ("Begin").leaf, false, 0);
	}

	void AddResumeAnimation(){
		charState.TimeScale = 0f;
		charState.AddAnimation (0, sequence.GetChild ("Begin").leaf, false, 0);
	}

	void ResetTransform(){

		gameObject.transform.position = originalPosition;
		gameObject.transform.rotation = Quaternion.identity;

		fadeOutMark = false;
		character.skeleton.a = 1f;

		body.gravityScale = 0.05f + 0.005f * Mathf.Abs (totalRotateAngle / 360);
		body.constraints = RigidbodyConstraints2D.FreezeAll;

		currRotateAngle = 0f;
		finishingAnimationAdded = false;
	}

	void ReplayAnimation(){
		ResetTransform ();

		Inverse ();
		AddStartingAnimation ();
		AddJumpAnimation ();
	}

	public void SetupCharacterAction(TreeNode sequence){

		this.sequence = sequence;
		AddTransform ();

		ReplayAnimation ();
	}

	public void Pause(){
		character.timeScale = 0;
		transientSpeed = body.velocity;
		body.constraints = RigidbodyConstraints2D.FreezeAll;

		if(rotationMark){
			rotationMark = false;
		}
	}

	public void Resume(){
		character.timeScale = 1;
		body.velocity = transientSpeed;
		body.constraints = RigidbodyConstraints2D.FreezeRotation;

		rotationMark = true;
	}

	// Use this for initialization
	void Awake () {
		originalPosition = transform.position;
		character = gameObject.GetComponent <SkeletonAnimation> ();

		body = gameObject.GetComponent<Rigidbody2D> ();

		charState = character.state;
	}
	
	// Update is called once per frame
	void Update () {
		UpdateTransform ();
		UpdateFinishing ();

		if (replay && replayMark){
			replayMark = false;
			ReplayAnimation ();
		}
	}
}
