using UnityEngine;
using System.Collections.Generic;
using System;
using UnityStandardAssets._2D;

public class Zombie : Enemy {

    private static List<string> attacksNames;
    private static System.Random random = new System.Random();
	private CollisionFacade collisionFacade; // This is used for the collisions.

    public override void Awake()
    {
        base.Awake();
        attacksNames = new List<string> { "Attack1", "Attack2", "Attack3" };
        collisionFacade = new CollisionFacade();
    }

    public override void Start () {
        base.Start();
        Debug.Log(System.Environment.Version);

    }
	
	protected override void FixedUpdate () {
        base.FixedUpdate();        
    }

    // Commented this out because we use triggers.
    //    void OnCollisionEnter2D(Collision2D coll)
    //    {
    //		Debug.Log ("Enemy recived collision");
    //        if (coll.gameObject == playerObj)
    //        {
    //            
    //            Debug.Log("Boom!");
    //        }
    //    }

    public void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log("Enemy collision trigger detected");
        collisionFacade.Collision(this.gameObject, col.gameObject);
    }

    public void OnTriggerExit2D(Collider2D col)
    {
        Debug.Log("Enemy collision trigger detected");
        collisionFacade.ExitCollision(this.gameObject, col.gameObject);
    }


    protected override void Attack()
    {
		
        Debug.Log("Attacking");
        _motor.normalizedXMovement = 0;
        bool attacking = false;
        attacksNames.ForEach((name)=> 
        {
            if (animator.GetCurrentAnimatorStateInfo(0).IsName(name))
                attacking = true;
        });
		
		if (!attacking) {
			alreadyHitWithinCurrentAttack = false;
			animator.Play (attacksNames [random.Next (attacksNames.Count)]);
		}

    }

    protected override void Idle()
    {
        
    }

    public CollisionFacade GetCollisionFacade()
    {
        return collisionFacade;
    }

}
