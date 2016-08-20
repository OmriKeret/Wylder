using UnityEngine;
using System.Collections;

public class ZombieWeapon : MonoBehaviour
{

    public CollisionFacade collisionFacade;
    private GameObject root;
    // Use this for initialization
    void Start()
    {
        root = transform.root.gameObject;
        collisionFacade = root.GetComponent<Zombie>().GetCollisionFacade();
    }

    void Update()
    {

    }

    public void OnTriggerEnter2D(Collider2D col)
    {
        collisionFacade.Collision(root, col.gameObject);
    }

    public void OnCollisionEnter2D(Collision2D col)
    {
        //collisionFacade.Collision(this.gameObject, col.gameObject);
    }


}
