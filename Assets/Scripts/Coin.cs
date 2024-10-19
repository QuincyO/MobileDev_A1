using System;
using System.Collections;
using System.Collections.Generic;
using Quincy.Structs;
using UnityEngine;
using Random = UnityEngine.Random;

public class Coin : MonoBehaviour
{
    private CircleCollider2D _collider;
    private Rigidbody2D rb;
    public Boundary bounds;
    public bool canMove = true;
    
    private float amplitude = 0.5f;
    private float frequency = 2f;
    
    private void Awake()
    { 
        _collider = gameObject.AddComponent<CircleCollider2D>();
        if (_collider == null)
        {
            Debug.LogError("No CircleCollider2D found");
            return;
        }
        _collider.isTrigger = true;
        rb = gameObject.AddComponent<Rigidbody2D>();
        rb.isKinematic = true;
        Reset();
    }



    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.trackables["Coins"]++;
            GameManager.Instance.trackables["Score"] += 10;
            GameManager.UpdateScoreUI();
            SoundManager.Play("Coin");
            Reset();
        }
    }

    public float maxXVal;
    public float moveSpeed = 2;
    void Move()
    {
        if (transform.position.x > maxXVal ) maxXVal = transform.position.x;
        
        if (canMove)transform.position = new Vector3(Mathf.Sin(Time.time * frequency ) * amplitude, transform.position.y - moveSpeed*Time.deltaTime + transform.position.z);
    }

    void CheckBounds()
    {
        if (transform.position.y < bounds.y.min)
        {
            GameManager.Instance.trackables["Score"] -= 5;
            Reset();
        }
    }
    void Update()
    {
        Move();
        
        CheckBounds();
    }

     void Reset()
     {
         canMove = false;
         var xPos = Random.Range(bounds.x.min, bounds.x.max);
         transform.position = new Vector3(xPos, bounds.y.max, 0);
         amplitude = Random.Range(.5f, 2f);
         frequency = Random.Range(2, 6);
         moveSpeed = Random.Range(2,8);
         StartCoroutine(StartMoving());
     }

     IEnumerator StartMoving()
     {
         yield return new WaitForSeconds(Random.Range(3, 9));
         canMove = true;
         
     }
    
    
}
