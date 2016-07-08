using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

/** Parallax Controller
 *  ===================
 *  The overall controller manipulates parallax backgrounds with
 *  this controller. The controller initializes with the existing
 *  parallax objects in the scene.
 */


public class MovableController : MonoBehaviour {

	private bool isMoving = false;
	private MovableBackground[] backgrounds;

	public void SetToScroll(bool isToMove){
		if(isMoving != isToMove){
			this.isMoving = isToMove;

			foreach (var bg in backgrounds) {
				bg.isMoving = isToMove;
			}
		}
	}

	public void SetScrollSpeed(float scrollRate){
		
		foreach (var bg in backgrounds) {
			bg.SetScrollSpeed (scrollRate);
		}			
	}

	public void SetLineScroll(bool isToMove){
		GameObject line = GameObject.Find ("TrackLine");
		line.GetComponent<MovableBackground> ().isMoving = isToMove;
		line.GetComponent<MeshRenderer>().enabled = isToMove;
	}

	public void UpdateSandScroll(){
//		if (sand.GetComponent <MovableBackground>().offset){
//			
//		}
	}

	GameObject sand;

	public void SetSandScroll(bool isToMove){
		GameObject line = GameObject.Find ("Sand");
		line.GetComponent<MovableBackground> ().isMoving = isToMove;
		line.GetComponent<MeshRenderer>().enabled = isToMove;
	}

	public void Setup(){
		backgrounds = GameObject.FindObjectsOfType<MovableBackground> ();
	}
	// Use this for initialization
	void Start () {
		sand = GameObject.Find ("Sand");
	}
	
	void Update () {
	}

}

