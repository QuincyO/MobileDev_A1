using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{

    public bool isDead = false;
    [SerializeField] private float detectionRadius = 100;

    public Boundary xBounds;
    public Boundary yBounds;
    public bool isActive = false;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    
    
    // Update is called once per frame
    void Update()
    {
        if (isActive)
        {
            CheckBounds();
        }
    }

    private void CheckBounds()
    {
        if (transform.position.x < xBounds.min || transform.position.x > xBounds.max ||
            transform.position.y < yBounds.min || transform.position.y > yBounds.max)
        {
            Deactivate();
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Bullet"))
        {
            Deactivate();
        }
        
    }

    private void Deactivate()
    {
        EnemySpawner.Instance.ReturnToPool(this.gameObject);
        isActive = false;
    }
}
