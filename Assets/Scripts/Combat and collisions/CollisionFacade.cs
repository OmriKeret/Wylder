using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class CollisionFacade {
	public CollisionLogic collisionLogic;
	public Dictionary< PairModel<string,string>,Action<GameObject, GameObject>>  collisionDictionary;

	public CollisionFacade() {
		collisionLogic = GameObject.Find("Logic").GetComponent<CollisionLogic>();
		buildDictionray ();
	}

	private void buildDictionray(){
		collisionDictionary = new Dictionary<PairModel<string,string>,Action<GameObject, GameObject>> 	
		{ 
			{ PairModel.New<string,string>("Enemy","Player") , collisionLogic.EnemyCollidedWithPlayer},
			{ PairModel.New<string,string>("Enemy","Enemy") , doNothing },
			{ PairModel.New<string,string>("Enemy","Wall") , doNothing},
			{ PairModel.New<string,string>("Player","Wall") , collisionLogic.playerCollidedWithWall },
			{ PairModel.New<string,string>("Player","Enemy") , doNothing },
			{ PairModel.New<string,string>("PlayerWeapon","Enemy") , collisionLogic.playerCollideWithEnemy }

		};


	}

	public void doNothing(GameObject mainCollider, GameObject collidedWith) {
		return;
	}
	public void Collision(GameObject mainCollider, GameObject collidedWith) {
		var pair = PairModel.New<string,string> (mainCollider.tag, collidedWith.tag);
		if (collisionDictionary.ContainsKey (pair)) {
			collisionDictionary [pair].Invoke (mainCollider, collidedWith);
		}
	}
}
