using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System;

public abstract class Enemy : MonoBehaviour {

    protected const int MAX_HP = 5;
    protected const int UNREACHABLE = 0, CLIMB = 1, JUMP = 2, FLOOR = 3;
    protected Vector3 raysDelta;

    protected int currentHp;
    protected Rigidbody2D _rigidbody;

    public GameObject raysCollection;
    [Range(0, 10)]
    public float raySize;
    [Range (0,100)]
    public float MaxVelocity;
    [Range(0, 100)]
    public float RunForce;
    [Range(0, 100)]
    public float ClimbMaxSpeed;
    [Range(0, 100)]
    public float JumpForce;

    protected List<Transform> rays;
    protected List<bool> raysCurrentHit;
    protected bool m_FacingRight = true;
    float distToGround;
    protected int layerMask;

    protected Animator animator;

    //States
    protected bool isGrounded;
    protected bool isClimbing;

    //Counters
    protected bool halt = false;
    protected int walkingCounter = 0;

    public virtual void Awake()
    {
        currentHp = MAX_HP;
        animator = GetComponent<Animator>();
        raysDelta = new Vector3(raySize, 0, 0);
    }

    public virtual void Start()
    {
        rays = raysCollection.GetComponentsInChildren<Transform>().Where(child=> child != raysCollection.transform).ToList<Transform>();
        _rigidbody = GetComponent<Rigidbody2D>();
        layerMask = ~(1 << LayerMask.NameToLayer("bones"));
        distToGround = rays[FLOOR].transform.position.y;

    }

    protected virtual void Update()
    {
        if (!halt)
        {
            Raycastion();
            Behaviours();
            UpdateAnimation();
        }
    }

    protected void Raycastion()
    {
        raysCurrentHit = new List<bool>();
        rays.ForEach(ray =>
        {
            Debug.DrawLine(ray.position, ray.position + raysDelta, Color.blue);
            raysCurrentHit.Add(Physics2D.Linecast(ray.position, ray.position + raysDelta, layerMask));
        });

        Debug.DrawLine(rays[FLOOR].position, (Vector2)rays[FLOOR].position - Vector2.up * 0.1f , Color.blue);
        isGrounded = Physics2D.Linecast(transform.position, (Vector2)rays[FLOOR].position - Vector2.up * 0.1f, layerMask);
        Debug.Log(string.Format("Grounded: {0}",isGrounded));
    }

    protected void Behaviours()
    {
        Patrol();
    }

    private void UpdateAnimation()
    {
        animator.SetBool("Grounded", isGrounded);
        animator.SetBool("InCombat", false);
        animator.SetFloat("Velocity", Mathf.Abs(_rigidbody.velocity.x));
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        
    }

    IEnumerator Wait(int seconds)
    {
        halt = true;
        yield return new WaitForSeconds(seconds);
        Flip();
        yield return new WaitForSeconds(seconds);
        halt = false;
    }

    protected abstract int Attack();

    protected abstract void Idle();

    protected virtual void Move(float velocity)
    {
        Move(velocity, 0);
    }

    protected virtual void Move(float xv, float yv)
    {
        if (!isGrounded)
        {
            return;
        }

        xv *= m_FacingRight ? 1 : -1; //Flip velocity
        Vector2 speed = new Vector2(xv, yv);
        if (_rigidbody.velocity.magnitude <= MaxVelocity)
        {
            _rigidbody.AddForce(speed, ForceMode2D.Force);
        }
        animator.SetBool("Run", speed.magnitude > 2);
    }

    protected virtual void Climb()
    {
        isClimbing = true;
        if (_rigidbody.velocity.y <= ClimbMaxSpeed)
        {
            _rigidbody.AddForce(Vector2.up* ClimbMaxSpeed, ForceMode2D.Force);
        }
        
    }

    protected virtual void Jump(float force)
    {
        if (!isGrounded || isClimbing)
        {
            return;
        }
        _rigidbody.AddForce(new Vector2(_rigidbody.velocity.x * 0.2f, _rigidbody.velocity.x).normalized * force, ForceMode2D.Impulse);
    }

    protected virtual void Flip()
    {
        // Switch the way the player is labelled as facing.
        m_FacingRight = !m_FacingRight;

        // Multiply the player's x local scale by -1.
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;

        raysDelta *= -1;
    }

    void turnAround()
    {
        float speed = _rigidbody.velocity.x;
        if (speed > 0.5f)
        {
            Move(0.25f * speed);
        }
        else
        {
            StartCoroutine(Wait(2));
            //Flip();
            //StartCoroutine(Wait(1));
        }
        
    }

    protected virtual void Patrol()
    {
        if (raysCurrentHit[UNREACHABLE])
        {
            turnAround();
            Debug.Log(rays[UNREACHABLE].name);
        }
        else if (raysCurrentHit[CLIMB] && false) //Currently inactive!
        {
            Climb();
        }
        else if (raysCurrentHit[JUMP])
        {
            Jump(JumpForce);
        }
        else if (raysCurrentHit[FLOOR])
        {
            Debug.Log("Floor");
            Move(RunForce, 30);
        }
        else
        {
            Move(RunForce);
        }
    }
}
