
/*
 *  Trapezoid Mesh
 *  ==============
 *  A Trapezoid mesh defined by height and length of upper and lower
 *  edges. Based on Jessy's work.
 * 
 *  http://forum.unity3d.com/threads/correcting-affine-texture-mapping-for-trapezoids.151283/#post-1036716
 * 
 */

using UnityEngine;
using System.Collections;

[RequireComponent (typeof (MeshFilter))]
[RequireComponent (typeof (MeshRenderer))]
public class Trape : MonoBehaviour {

	public float height = 1f;
	public float upper  = 0.8f;
	public float lower  = 1.2f;
	
	public void Rebuild(){
		MeshFilter meshFilter = GetComponent<MeshFilter>();
		if (meshFilter==null){
			Debug.LogError("MeshFilter not found!");
			return;
		}

		float halfHeight = height / 2;
		float halfUpper  = upper  / 2;
		float halfLower  = lower  / 2;

		Mesh mesh = meshFilter.sharedMesh;

		if (mesh == null){
			meshFilter.mesh = new Mesh();
			mesh = meshFilter.sharedMesh;
		}
		mesh.Clear();

		// basically just assigns a corner of the texture to each vertex
		mesh.vertices = new Vector3[4]{
			new Vector3(-halfLower, -halfHeight,  0f),
			new Vector3(-halfUpper,  halfHeight,  0f),
			new Vector3( halfUpper,  halfHeight,  0f),
			new Vector3( halfLower, -halfHeight,  0f)
		};

		mesh.triangles = new int[]{
			0,1,2,
			0,2,3
		};
		
		var shiftedPositions = new Vector2[] {
			Vector2.zero,
			new Vector2(0, mesh.vertices[1].y - mesh.vertices[0].y),
			new Vector2(mesh.vertices[2].x - mesh.vertices[1].x, mesh.vertices[2].y - mesh.vertices[3].y),
			new Vector2(mesh.vertices[3].x - mesh.vertices[0].x, 0)
		};
		mesh.uv = shiftedPositions;

		var widths_heights = new Vector2[4];
		widths_heights[0].x = widths_heights[3].x = shiftedPositions[3].x;
		widths_heights[1].x = widths_heights[2].x = shiftedPositions[2].x;
		widths_heights[0].y = widths_heights[1].y = shiftedPositions[1].y;
		widths_heights[2].y = widths_heights[3].y = shiftedPositions[2].y;
		mesh.uv2 = widths_heights;

	}
	
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	}
}
