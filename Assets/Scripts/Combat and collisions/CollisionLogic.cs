using UnityEngine;
using System.Collections;

public class CollisionLogic : MonoBehaviour  {


    //landing
    private GameObject landDust;

	void Start () {
	}


	public void EnemyCollidedWithPlayer(GameObject enemy, GameObject player) {
//     
		var playerStats = player.GetComponent<PlayerStatsLogic>();
		playerStats.hit (1);
	}
		
  
 
	public void playerCollidedWithWall(GameObject player, GameObject wall) 
	{

	}

	public void playerCollideWithEnemy(GameObject player, GameObject enemy) {
	}

}
