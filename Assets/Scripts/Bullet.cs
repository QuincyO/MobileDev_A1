using System;
using System.Collections;
using System.Collections.Generic;
using Quincy.Structs;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class Bullet : MonoBehaviour
{
    public float moveForce = 100;

    private bool _isFired = false;
    public bool isActive = false;

    private Vector3 _direction;

    public Boundary Bounds;

    public GameObject instigator = null;

    public GameObject Explosion;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Fire(Transform DirectionAndPosition,float force)
    {
        isActive = true;
        transform.position = DirectionAndPosition.position;
        GetComponent<Rigidbody2D>().AddForce(DirectionAndPosition.up * force);
    }

    private void Stop()
    {
        _isFired = false;
        isActive = false;
    }

    void Move()
    {
        transform.position += _direction * (moveForce * Time.deltaTime);
    }

    void CheckBounds()
    {
        if (transform.position.x < Bounds.x.min || transform.position.x > Bounds.x.max ||
            transform.position.y < Bounds.y.min || transform.position.y > Bounds.y.max)
        {
            instigator.GetComponent<PlayerController>().ReturnToPool(this.gameObject);
            Stop();
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Instantiate(Explosion,other.contacts[0].point,Quaternion.identity);
            
            if (instigator.IsUnityNull()) return;

            GameManager.Instance.UpdateScore();
            instigator.GetComponent<PlayerController>().ReturnToPool(this.gameObject);
            Stop();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isActive)
        {
            CheckBounds();
        }

    }
}
