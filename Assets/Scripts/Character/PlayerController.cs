﻿using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public CollisionFacade collisionFacade;
	private PlatformerMotor2D motor;
	// Use this for initialization
	void Start () {
		collisionFacade = new CollisionFacade();
		motor = GetComponent<PlatformerMotor2D> ();
	}

	void Update() {
		
	}

	public void OnTriggerEnter2D(Collider2D col) {
		collisionFacade.Collision(this.gameObject, col.gameObject);
	}

	public void OnParticleCollision(GameObject col) {
		Debug.Log ("Player collided with particles");
		collisionFacade.Collision(this.gameObject, col.gameObject);
	}

	public void OnCollisionEnter2D(Collision2D col)
	{
		//collisionFacade.Collision(this.gameObject, col.gameObject);
	}


}
