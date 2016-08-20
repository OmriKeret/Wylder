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

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == playerObj)
        {
            playerObj.GetComponent<ICharCollider>().Hit(AttackForce);
            Debug.Log("Boom!");
        }
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
