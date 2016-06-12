using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public CollisionFacade collisionFacade;

	// Use this for initialization
	void Start () {
		collisionFacade = new CollisionFacade();
	}
	
	public void OnTriggerEnter2D(Collider2D col) {
		Debug.Log ("Collision detected");
			collisionFacade.Collision(this.gameObject, col.gameObject);
	}
}
