using UnityEngine;
using System.Collections;

public class collisionTest : MonoBehaviour, ICharCollider {

	public eCharState state = eCharState.Attacking;
	public int health = 10;

	public CollisionFacade collisionFacade;

	// Use this for initialization
	void Start () {
		collisionFacade = new CollisionFacade();
	}

	public void OnCollisionEnter2D(Collision2D col)
	{
		Debug.Log ("Collision detected on enemy");
		collisionFacade.Collision(this.gameObject, col.gameObject);
	}

	public void OnTriggerEnter2D(Collider2D col) {
		Debug.Log ("Collision detected on enemy");
		collisionFacade.Collision(this.gameObject, col.gameObject);
	}

	#region ICharCollider implementation
	public eCharState GetState ()
	{
		return state;
	}

	public void ActiveHitAnimation ()
	{
		throw new System.NotImplementedException ();
	}

	public void ActiveDeathAnimation ()
	{
		throw new System.NotImplementedException ();
	}

	public void ActiveHitSound ()
	{
		throw new System.NotImplementedException ();
	}

	public void ActiveDeathSound ()
	{
		throw new System.NotImplementedException ();
	}

	public bool Hit (int dmg)
	{
		health = health - dmg;
		Debug.Log ("Test collider got hit, his HP now is " + health);
		return health < 1;
	}

	public void ActiveCounterAnimation ()
	{
		Debug.Log ("Enemy got countered!s");
	}

	public int getAttackStrength ()
	{
		return 1;
	}

	#endregion
}
