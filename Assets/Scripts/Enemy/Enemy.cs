using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public abstract class Enemy : MonoBehaviour {

    protected const int MAX_HP = 5;
    protected const int UNREACHABLE = 0, CLIMB = 1, JUMP = 2, FLOOR = 3;
    protected Vector3 raysDelta = new Vector3(.5f, 0 ,0);

    protected int currentHp;
    protected Rigidbody2D _rigidbody;

    public GameObject raysCollection;
    [Range (0,100)]
    public float MaxVelocity;
    [Range(0, 100)]
    public float CliimbMaxSpeed;

    protected List<Transform> rays;
    protected List<bool> raysCurrentHit;
    protected bool m_FacingRight = true;
    float distToGround;
    protected int layerMask;

    //States
    protected bool isGrounded;
    protected bool isClimbing;

    public virtual void Awake()
    {
        currentHp = MAX_HP;
    }

    public virtual void Start()
    {
        rays = raysCollection.GetComponentsInChildren<Transform>().Where(child=> child != raysCollection.transform).ToList<Transform>();
        _rigidbody = GetComponent<Rigidbody2D>();
        layerMask = ~(1 << LayerMask.NameToLayer("EnemyExternal"));// | ~(1 << LayerMask.NameToLayer("EnemyInternal"));
        distToGround = GetComponents<Collider2D>().Min<Collider2D>(colider => colider.bounds.extents.y);

    }

    protected virtual void Update()
    {
        Raycastion();
        Behaviours();
    }

    void Raycastion()
    {
        raysCurrentHit = new List<bool>();
        rays.ForEach(ray =>
        {
            Debug.DrawLine(ray.position, ray.position + raysDelta, Color.blue);
            raysCurrentHit.Add(Physics2D.Linecast(ray.position, ray.position + raysDelta, layerMask));
        });

        isGrounded = Physics2D.Raycast(transform.position, -Vector2.up, distToGround + 0.1f);
    }

    void Behaviours()
    {
        Patrol();
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        
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
        if (_rigidbody.velocity.magnitude <= MaxVelocity)
        {
            _rigidbody.AddForce(new Vector2(xv, yv), ForceMode2D.Force);
        }
    }

    protected virtual void Climb()
    {
        isClimbing = true;
        if (_rigidbody.velocity.y <= CliimbMaxSpeed)
        {
            _rigidbody.AddForce(Vector2.up* CliimbMaxSpeed, ForceMode2D.Force);
        }
        
    }

    protected virtual void Jump(float force)
    {
        if (!isGrounded || isClimbing)
        {
            return;
        }
        _rigidbody.AddForce(new Vector2(_rigidbody.velocity.x, _rigidbody.velocity.x).normalized * force, ForceMode2D.Impulse);
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

    protected virtual void Patrol()
    {
        if (raysCurrentHit[UNREACHABLE])
        {
            Flip();
            Debug.Log(rays[UNREACHABLE].name);
        }
        else if (raysCurrentHit[CLIMB])
        {
            Climb();
        }
        else if (raysCurrentHit[JUMP])
        {
            Jump(1);
        }
        else if (raysCurrentHit[FLOOR])
        {
            Debug.Log("Floor");
            Move(10, 20);
        }
        else
        {
            Move(10);
        }
    }
}
