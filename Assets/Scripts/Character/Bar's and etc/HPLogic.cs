using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using EZCameraShake;

public class HPLogic : MonoBehaviour {

    private int MAX_HP;
    private int currentHp;

    //GUI fill logic
    public float changeColorDuration = 0.2f;


    Animator anim;

    //logic connections
    private PlayerStatsLogic statsLogic;

    void Awake()
    {
        statsLogic = this.gameObject.GetComponent<PlayerStatsLogic>();
       
    }

    private void updateRatio()
    {      

    }

	public void reduceHpTo(int hp)
    {
        //Debug.Log("updatedCurrentHP");

        currentHp = hp;
		CameraUtils.Instance.blinkCamera();
		CameraShaker.Instance.ShakeOnce (20f, 20f, .5f ,.2f);
        
    }

    public void setMaximumHP(int hp)
    {
        MAX_HP = hp;
    }
		

}
