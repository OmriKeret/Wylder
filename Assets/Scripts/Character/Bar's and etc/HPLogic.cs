using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using EZCameraShake;

public class HPLogic : MonoBehaviour {

    private float MAX_HP;
    private float currentHp;

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

	public void reduceHpTo(float hp)
    {
        //Debug.Log("updatedCurrentHP");

        currentHp = hp;
		CameraUtils.Instance.blinkCamera();
		CameraShaker.Instance.ShakeOnce (20f, 20f, .5f ,.2f);
        
    }

    public void setMaximumHP(float hp)
    {
        MAX_HP = hp;
    }
		

}
