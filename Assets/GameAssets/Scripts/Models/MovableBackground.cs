using UnityEngine;
using System.Collections;


public class MovableBackground : MonoBehaviour{

	public float  depth;
	public float  slope;

	private Material m;
	public float  initialOffset;
	public float  initialScrollSpeed;
	private float scrollSpeed;
	public float offset;

	public bool isMoving;

	void SetupFromString(string[] fields){
		this.name = fields [0];
		this.depth = float.Parse (fields [1]);
		this.slope = float.Parse (fields [2]);

		this.initialOffset = float.Parse (fields [3]);
		this.initialScrollSpeed = float.Parse (fields [4]);

	}
		
	void SetupDimension(){
		float quadHeight = Camera.main.orthographicSize * 2.0f;
		float quadWidth = quadHeight * Screen.height / Screen.width;
		gameObject.transform.localScale = new Vector3(quadWidth*1.7f, quadHeight, 1f);
		gameObject.transform.position = new Vector3(0, 0, this.depth);
//		gameObject.hideFlags = HideFlags.HideInHierarchy;
	}

	void SetupMesh(){
		Trape s = gameObject.AddComponent<Trape>();
		s.upper = 1.0f - this.slope;
		s.lower = 1.0f + this.slope;
		s.Rebuild ();
	}
		
	public void Setup(string[] fields){
		SetupFromString (fields);
		SetupDimension ();
		SetupMesh ();
	}

	void Start () {
		m = GetComponent<Renderer> ().material;
		m.SetTextureOffset ("_MainTex", new Vector2 (initialOffset, 0));
		m.SetVector ("_Offset", new Vector4 (initialOffset, 0));
		this.offset = this.initialOffset;
	}

	public void SetScrollSpeed(float scrollRateFactor){
		this.scrollSpeed = this.initialScrollSpeed * scrollRateFactor;
	}

	public void ResetOffset () {
		this.offset = this.initialOffset;
		m.SetVector ("_Offet", new Vector4 (initialOffset, 0));
	}

	void Scroll(){
		float x = Mathf.Repeat (offset += Time.fixedDeltaTime * this.scrollSpeed, 1);
		m.SetTextureOffset("_MainTex", new Vector2 (x, 0));
		m.SetVector ("_Offset", new Vector4 (x, 0));
	}

	void Update(){
		if(isMoving)
			Scroll ();

	}
}
