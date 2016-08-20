using UnityEngine;
using System.Collections;

public class CollisionLogic  {


    //landing
    private GameObject landDust;

	void Start () {
	}


	/**
	 * Invoked when enemy colides with player, enemy attack player logic.
	 * */
	public void EnemyCollidedWithPlayer(GameObject enemy, GameObject player) {
    
		var playerCollider = player.GetComponentInParent <ICharCollider>();
		var enemyCollider = enemy.GetComponent<ICharCollider>();
		var playerState = playerCollider.GetState();
		var enemyState =  eCharState.Attacking;//enemyCollider.GetState(); Stub.
		var enemyAttacking = (enemyState == eCharState.Attacking);

		if (isAttackable (playerState) && enemyAttacking) {
			
			// Enemy attacked player, and player can be attacked.
			playerCollider.Hit (1) ;//enemyCollider.getAttackStrength ()); Stub.

		} else if (playerState == eCharState.Countering && enemyAttacking) {
			// Enemy got countered.
			//enemyCollider.ActiveCounterAnimation();
			playerCollider.ActiveCounterAnimation(enemy.gameObject.tag);
		}


	}


	/**
	 * Invoked when enemy colides with player, player attack enemy logic.
	 * */
	public void playerCollideWithEnemy(GameObject player, GameObject enemy) {
		var playerCollider = player.GetComponentInParent <ICharCollider>();
		var enemyCollider = enemy.GetComponent<ICharCollider>();
		var playerState = playerCollider.GetState();
		var enemyState = enemyCollider.GetState();
		var playerAttacking = (playerState == eCharState.Attacking);

		if (isAttackable (enemyState) && playerAttacking) {
			// Enemy attacked player, and player can be attacked.
			enemyCollider.Hit (playerCollider.getAttackStrength ());
		}

	}




	public void playerCollidedWithWall(GameObject player, GameObject wall) 
	{

	}

	private bool isAttackable(eCharState state) {
		return state != eCharState.KillingMove && state != eCharState.dead && state != eCharState.Countering;
	}


}
