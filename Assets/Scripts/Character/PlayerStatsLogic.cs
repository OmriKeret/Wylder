using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerStatsLogic : MonoBehaviour {
	// Player basic stats.
    public int maxHP;
    public int HP = 3;
    public int Strength = 1;


    //GUI
    HPLogic healthLogic;
    StaminaBarLogic staminaBar;


	// Use this for initialization
    void OnEnable()
    {
		maxHP = HP;
		healthLogic = this.GetComponent<HPLogic>();
     //   staminaBar = this.GetComponent<StaminaBarLogic>();
  

	}
	void Start() 
	{
		maxHP = HP;
	//	staminaBar.setMaximumStamina(3);
	}

	public bool hit(int force) {
		HP = HP - force;
		Debug.Log("Current Health: " + HP);
		if (HP > 0) {
			healthLogic.reduceHpTo (HP);
			return true;
		} else {
			// Player is dead.
			healthLogic.reduceHpTo (HP);
			return false;
		}
	}
	

}
