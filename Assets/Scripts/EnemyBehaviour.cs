using System;
using System.Collections;
using System.Collections.Generic;
using Quincy.Structs;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{

    public bool isDead = false;
    [SerializeField] private float detectionRadius = 6;

    public Boundary Bounds;
    public bool isActiveSeeking = true;

    public float nudgeFactor = 50;
    CircleCollider2D detectionCollider  = null;

    public static PlayerController player;
    
    // Start is called before the first frame update
    void Start()
    {
        detectionCollider = gameObject.AddComponent<CircleCollider2D>();
        detectionCollider.radius = detectionRadius;
        detectionCollider.isTrigger = true;
        rb = GetComponent<Rigidbody2D>();
    }
private Rigidbody2D rb = null;



// Update is called once per frame
    void Update()
    {

            CheckBounds();
        
    }

    private void CheckBounds()
    {
        if (transform.position.y <= -.5f) isActiveSeeking = false;
        
            if (transform.position.y < player.transform.position.y)
        {
            Reset();
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        if (!isActiveSeeking) return;
        
        Vector3 dampingForce = -rb.velocity * dampingFactor;
        
        var directionToPlayer = (other.transform.position - transform.position).normalized;
        Vector3 direction = rb.velocity.normalized;
        var distanceToPlayer = Vector3.Distance(transform.position, other.transform.position);
        
        
        var lerpedValue = Mathf.Lerp(0, nudgeFactor, distanceToPlayer / detectionRadius);
        
        var nudgeForce = (directionToPlayer - direction) * lerpedValue;
         
         
        rb.AddForce(dampingForce + nudgeForce,ForceMode2D.Force);
    }

    [SerializeField] private float dampingFactor = 1f;
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Bullet"))
        {
            Kill();
        }

        if (other.gameObject.CompareTag("Player"))
        {
            player.TakeDamage(45f,gameObject);
            Reset();
        }
    }

    private void Kill()
    {
        GameManager.Instance.trackables["Enemies"]++;
        Reset();
    }

    private void Reset()
    {
        EnemySpawner.Instance.ReturnToPool(this.gameObject);
        isActiveSeeking = true;
    }
}
