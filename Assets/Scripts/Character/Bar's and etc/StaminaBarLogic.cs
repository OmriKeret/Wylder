using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class StaminaBarLogic : MonoBehaviour
{

    public Image progressBarEmpty;
    public Image progressBarFull;
    private int MAX_STAMINA;
    private int currentStamina;

    //GUI fill logic
    public float changeColorDuration = 0.2f;
    public float speed = 0.7f; //speed for stamina to go down
    float barDisplay = 0f;
    float currentRatio = 0f;
    bool shouldChange;

	//public float circleRatio = 0.666666666f;
    Animator anim;

    //logic connections
    private PlayerStatsLogic statsLogic;
   
    void Awake()
    {
        var staminaEmpty = GameObject.Find("StaminaHolder/StaminaEmpty");
        progressBarFull = GameObject.Find("StaminaHolder/Stamina").GetComponent<Image>();
        progressBarEmpty = staminaEmpty.GetComponent<Image>();
        statsLogic = this.gameObject.GetComponent<PlayerStatsLogic>();
        anim = staminaEmpty.GetComponent<Animator>();
		barDisplay = 1f;
    }

    private void updateRatio()
    {   
		currentRatio = (float)((float)currentStamina / (float)MAX_STAMINA);
        if (progressBarFull.fillAmount > currentRatio)
        {
            //stamina bar go down
            progressBarFull.fillAmount = currentRatio;
            anim.SetTrigger("hit");
        }
        else
        {
			setShouldChange();
            //stamina bar go up
            
        }
        //Debug.Log("RATIO IS:" + currentRatio);
        //Debug.Log("currentDisplay is: " + barDisplay);
    }

    public void setShouldChange()
    {
        shouldChange = true;
    }

    public void updateCurrentStamina(int stamina)
    {
        currentStamina = stamina;
        updateRatio();
    }

    public void setMaximumStamina(int stamina)
    {
        MAX_STAMINA = stamina;
    }

    void Update()
    {
        if (shouldChange)
        {
            if (barDisplay > 1)
            {
                barDisplay = 1f;
                return;
            }
            if (barDisplay < 0)
            {
                barDisplay = 0f;
                return;
            }
            if (barDisplay < currentRatio)
            {
                barDisplay += speed * Time.deltaTime;
                if (barDisplay >= currentRatio)
                {
                    barDisplay = currentRatio;
                    shouldChange = false;
                }
                progressBarEmpty.fillAmount = barDisplay;
                progressBarFull.fillAmount = barDisplay;
            }
            else
            {
                barDisplay -= speed * Time.deltaTime;
                if (barDisplay <= currentRatio)
                {
                    barDisplay = currentRatio;
                    shouldChange = false;
                }
                progressBarEmpty.fillAmount = barDisplay;
            }
        }
       

    }
}