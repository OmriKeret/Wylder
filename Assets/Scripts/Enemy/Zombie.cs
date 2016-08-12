using UnityEngine;
using System.Collections;
using System;
using UnityStandardAssets._2D;

public class Zombie : Enemy {

    // Use this for initialization
    public override void Start () {
        base.Start();
    }
	
	protected override void Update () {
        base.Update();
    }

    protected override int Attack()
    {
        return 1;
    }

    protected override void Idle()
    {
        
    }


}
