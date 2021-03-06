﻿using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class CollisionFacade {
	public CollisionLogic collisionLogic;
	public Dictionary< PairModel<string,string>,Action<GameObject, GameObject>>  collisionDictionary;
    public Dictionary<PairModel<string, string>, Action<GameObject, GameObject>> collisionExitDictionary;

    public CollisionFacade() {
		collisionLogic = new CollisionLogic();
		buildDictionray ();
	}

	private void buildDictionray(){
		collisionDictionary = new Dictionary<PairModel<string,string>,Action<GameObject, GameObject>> 	
		{ 
			{ PairModel.New<string,string>("EnemyWeapon","Player") , collisionLogic.EnemyCollidedWithPlayer},
			{ PairModel.New<string,string>("Enemy","Enemy") , doNothing },
            { PairModel.New<string,string>("Enemy","EnemySyncer") , collisionLogic.addEnemyNearby },
            { PairModel.New<string,string>("Enemy","Wall") , doNothing},
			{ PairModel.New<string,string>("Enemy","Untagged") , doNothing},
            { PairModel.New<string,string>("Enemy","Player") , doNothing },
            { PairModel.New<string,string>("Enemy","PlayerWeapon") , collisionLogic.playerCollideWithEnemy},
            { PairModel.New<string,string>("Enemy","Bullet") , collisionLogic.bulletColidedWithEnemy},
			{ PairModel.New<string,string>("Player","Untagged") , doNothing},
			{ PairModel.New<string,string>("Player","Enemy") , doNothing },
			{ PairModel.New<string,string>("Player","Wall") , collisionLogic.playerCollidedWithWall },

			{ PairModel.New<string,string>("Player","Boss") , doNothing },
			{ PairModel.New<string,string>("Boss","Untagged") , doNothing},
			{ PairModel.New<string,string>("Boss","Player") , doNothing },
			{ PairModel.New<string,string>("Boss","PlayerWeapon") , collisionLogic.playerCollideWithEnemy},
			{ PairModel.New<string,string>("Boss","Bullet") , collisionLogic.bulletColidedWithEnemy},
			{ PairModel.New<string,string>("Boss","Enemy") , doNothing },
			{ PairModel.New<string,string>("Boss","EnemySyncer") , collisionLogic.addEnemyNearby },
			{ PairModel.New<string,string>("Boss","Wall") , doNothing},

        };

        collisionExitDictionary = new Dictionary<PairModel<string, string>, Action<GameObject, GameObject>>
        {
            { PairModel.New<string,string>("Enemy","EnemySyncer") , collisionLogic.removeEnemyNearby },
        };

    }

	public void doNothing(GameObject mainCollider, GameObject collidedWith) {
		return;
	}
	public void Collision(GameObject mainCollider, GameObject collidedWith) {
		Debug.Log ("We got a collision between: " + mainCollider.tag + " and " + collidedWith.tag);
		var pair = PairModel.New<string,string> (mainCollider.tag, collidedWith.tag);
		if (collisionDictionary.ContainsKey (pair)) {
			collisionDictionary [pair].Invoke (mainCollider, collidedWith);
		}
	}

    public void ExitCollision(GameObject mainCollider, GameObject collidedWith)
    {
        Debug.Log("We exit a collision between: " + mainCollider.tag + " and " + collidedWith.tag);
        var pair = PairModel.New<string, string>(mainCollider.tag, collidedWith.tag);
        if (collisionExitDictionary.ContainsKey(pair))
        {
            collisionExitDictionary[pair].Invoke(mainCollider, collidedWith);
        }
    }
}
