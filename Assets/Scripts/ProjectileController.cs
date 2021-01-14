using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    // Serialized Parameters
    [SerializeField] float projectileDuration;

    // Cached Variables
    private Rigidbody2D myRigidbody2D;

    private void Awake()
    {
        GetComponents();
    }

    private void GetComponents()
    {
        myRigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        Destroy(gameObject, projectileDuration);
    }

    public void LaunchProjectile(Vector2 direction, float force)
    {
        myRigidbody2D.AddForce(direction * force);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        EnemyController myEnemyController = collision.collider.GetComponent<EnemyController>();
        if (myEnemyController)
        {
            myEnemyController.FixBot();
        }
        Destroy(gameObject);
    }
}
