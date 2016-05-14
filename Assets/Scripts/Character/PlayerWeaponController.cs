using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public CollisionFacade collisionFacade;

	// Use this for initialization
	void Start () {
		collisionFacade = new CollisionFacade();
	}
	
	public void OnCollisionEnter2D(Collision2D col)
	{
		collisionFacade.Collision(this.gameObject, col.gameObject);
	}
}
