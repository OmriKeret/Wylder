using UnityEngine;
using System.Collections.Generic;
using System;
using UnityStandardAssets._2D;

public class Zombie : Enemy {

    private static List<string> attacksNames;
    private static System.Random random = new System.Random();

    public override void Awake()
    {
        base.Awake();
        attacksNames = new List<string> { "Attack1", "Attack2", "Attack3" };
    }

    public override void Start () {
        base.Start();
    }
	
	protected override void FixedUpdate () {
        base.FixedUpdate();
        
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
		Debug.Log ("Enemy recived collision");
        if (coll.gameObject == playerObj)
        {
            
            Debug.Log("Boom!");
        }
    }

	public void OnTriggerEnter2D(Collider2D col) {
		Debug.Log ("Enemy collision trigger detected");
		//		collisionFacade.Collision(this.gameObject, col.gameObject);
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
        if (!attacking)
            animator.Play(attacksNames[random.Next(attacksNames.Count)]);

    }

    protected override void Idle()
    {
        
    }


}
