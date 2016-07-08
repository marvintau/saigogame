using UnityEngine;
using System.Collections;

public class CollisionHandler : MonoBehaviour {

	Rigidbody2D body;

	ParticleSystem splash;

	// Use this for initialization
	void Awake () {
		splash = GameObject.FindObjectOfType<ParticleSystem> ();

		body = GameObject.Find("Character").GetComponent <Rigidbody2D> ();
	
	}

	void StopEmission(){
		ParticleSystem.EmissionModule em = splash.emission;
		em.enabled = false;
	}

	void OnCollisionEnter2D(Collision2D collision) {
		ParticleSystem.EmissionModule em = splash.emission;
		em.enabled = true;

		em.type = ParticleSystemEmissionType.Time;

		em.SetBursts(
			new ParticleSystem.Burst[]{
				new ParticleSystem.Burst(0.0f, 30)
			});

		body.velocity = new  Vector2(0, collision.relativeVelocity.y);

		Invoke ("StopEmission", 0.5f);
	}

	void Update(){
	}
}
