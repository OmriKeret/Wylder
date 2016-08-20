using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System;

public abstract class Enemy : MonoBehaviour, ICharCollider {

    protected const int MAX_HP = 5;
    protected const int UNREACHABLE = 0, CLIMB = 1, JUMP = 2, FLOOR = 3;
    protected Vector3 raysDelta;

    protected int currentHp;
    protected Rigidbody2D _rigidbody;
    protected bool inCombat;
    protected eCharState state;
    protected Dictionary<PlatformerMotor2D.MotorState,Action> moveSM;
    protected PlatformerMotor2D _motor;

    protected List<Transform> rays;
    protected List<bool> raysCurrentHit;
    public bool m_FacingRight = false;
    float distToGround;
    protected int layerMask;
    protected int currentFPJ;

    protected Animator animator;
    protected PlatformerMotor2D.MotorState priorState;
    protected System.Reflection.MethodInfo changeMotorState;

    //States
    protected bool isGrounded;

    //Counters
    protected bool halt = false;
    protected int walkingCounter = 0;


    public float movement { get; private set; }
    public float distanceCheckForJump;
    public float distanceCheckForAttack;
    public List<LineCastModel> lineCastVectors;
    public Vector2 jumpMin;
    public Vector2 jumpMax;
    public Vector2 player;
    protected bool isLanding;

    protected static readonly LayerMask jumpMask = ~LayerMask.GetMask("bones", "EnemyWeapon");
    protected static readonly LayerMask playerMask = LayerMask.GetMask("PlayerExternal");
    protected static readonly LayerMask restMask = 0xFFFF;

    public virtual void Awake()
    {
        currentHp = MAX_HP;
        animator = GetComponent<Animator>();
        
    }

    public virtual void Start()
    {

        _motor = GetComponent<PlatformerMotor2D>();
        changeMotorState = typeof(PlatformerMotor2D).GetProperty("motorState").GetSetMethod(true);
        
        movement = -1;

        moveSM = new Dictionary<PlatformerMotor2D.MotorState, Action> {
            {PlatformerMotor2D.MotorState.OnGround,GroundRaycast },
            {PlatformerMotor2D.MotorState.Jumping,Jump },
            {PlatformerMotor2D.MotorState.Falling, Air }
        };

        lineCastVectors = new List<LineCastModel>
        {
            new LineCastModel() {MainObject = transform, Start = jumpMin, End = distanceCheckForJump * Vector2.right , Invoker = Jump, Mask = jumpMask},
            new LineCastModel() {MainObject = transform, Start = jumpMax, End = distanceCheckForJump * Vector2.right , Invoker = Flip, Mask = jumpMask},
            new LineCastModel() {MainObject = transform, Start = player, End = distanceCheckForJump * Vector2.right , Invoker = Attack, Mask = playerMask},
            new LineCastModel() {MainObject = transform, Start = new Vector2(), End = new Vector2() , Invoker = Rest, Mask = 0},
        };
    }

    //protected virtual void Update()
    //{
    //    if (!halt)
    //    {
    //        if (inCombat)
    //        {

    //        }
    //        else
    //        {
    //            Raycastion();
    //            Behaviours();
    //            UpdateAnimation();
    //        }
    //    }
    //}

    protected virtual void FixedUpdate()
    {
        if (moveSM.Keys.Contains(_motor.motorState))
        {
            Debug.Log(_motor.motorState.ToString());
            moveSM[_motor.motorState].Invoke();
        }
        else
        {
            Debug.Log("Missing " + _motor.motorState);
        }

        priorState = _motor.motorState;
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        
    }

    IEnumerator FlipAndWait(int seconds)
    {
        halt = true;
        yield return new WaitForSeconds(seconds);
        Flip();
        yield return new WaitForSeconds(seconds);
        halt = false;
    }

    protected abstract void Attack();

    protected abstract void Idle();

    protected virtual void Flip()
    {

        Debug.Log("Flip");
        if (movement < 0)
            transform.rotation = new Quaternion(0, 180f, 0, 1);
        else
            transform.rotation = new Quaternion(0, 0, 0, 1);
        Vector3 scale = transform.localScale;
        scale += new Vector3(0, 0, -2 * scale.z);
        transform.localScale = scale;
        movement *= -1;
        lineCastVectors.ForEach((lineCaster) => lineCaster.Flip());

    }

    protected virtual void GroundRaycast()
    {
        _motor.fallFast = false;

        Debug.Log("Movement " + movement);
        lineCastVectors.ForEach((lineCaster) => lineCaster.Cast(false));
        lineCastVectors.Reverse();
        LineCastModel mostRelevantHit = null;
        int hitCount = 0;
        lineCastVectors.ForEach((lineCaster) =>
        {
            if (lineCaster.CheckAndReset() && mostRelevantHit == null)
            {
                hitCount++;
                Debug.Log("Hit " + hitCount);
                mostRelevantHit = lineCaster;
            }
        });
        lineCastVectors.Reverse();
        if (mostRelevantHit == null) //No hit means the way is clear
            Patrol();       
        else
            mostRelevantHit.Invoker.Invoke();
    }

    protected virtual void Patrol()
    {

        _motor.normalizedXMovement = movement;
        animator.Play("Run");
        
    }

    protected void Jump()
    {
        _motor.Jump();
        animator.Play("Jump");
    }

    protected void Air()
    {
        changeMotorState.Invoke(_motor, new object[] { PlatformerMotor2D.MotorState.Falling });
        Debug.Log("Air");
        animator.Play("Air");
    }

    protected void Land()
    {
        Debug.Log("Land");
    }

    protected void Rest()
    {
        lineCastVectors[(int)eLineCaster.rest].Mask = restMask;
        movement = 0;
        animator.Play("IDLE");
        Debug.Log("Rest");
    }

    protected void WakeUp()
    {
        lineCastVectors[(int)eLineCaster.rest].Mask = 0;
        Debug.Log("WakeUp");
    }

    protected void NowLanding(bool land)
    {
        isLanding = land;
    }

    public eCharState GetState()
    {
        throw new NotImplementedException();
    }

    public void ActiveHitAnimation()
    {
        animator.SetTrigger("GotHit");
    }

    public void ActiveDeathAnimation()
    {
        animator.SetTrigger("Death");
    }

    public void ActiveHitSound()
    {
        throw new NotImplementedException();
    }

    public void ActiveDeathSound()
    {
        throw new NotImplementedException();
    }

    public bool Hit(int dmg)
    {
        currentHp -= dmg;
        return currentHp > 0;
    }

    public void ActiveCounterAnimation()
    {
        throw new NotImplementedException();
    }

    public int getAttackStrength()
    {
        throw new NotImplementedException();
    }

    protected enum eLineCaster
    {
        minJump = 0,
        maxJump,
        player,
        rest
    }
}
