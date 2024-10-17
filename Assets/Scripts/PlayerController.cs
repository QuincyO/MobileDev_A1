using System;
using System.Collections;
using System.Collections.Generic;
using Quincy.Structs;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 5;
    [SerializeField]
    private Boundary xBoundary;
    
    private Camera _camera;
    
    [SerializeField]
    private bool isTestingMobile = false;

    [SerializeField] private bool isMobile = true;
    
    private Vector3 destination;
    
    private Rigidbody2D rb2d;
    
    [SerializeField] GameObject bulletPrefab;

    private LinkedList<GameObject> _activeBullets;
    private LinkedList<GameObject> _inactiveBullets;
    
    public Boundary bulletBoundaryX;
    public Boundary bulletBoundaryY;

    [SerializeField] private float bulletForce = 300;

    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();

        _activeBullets = new LinkedList<GameObject>();
        _inactiveBullets = new LinkedList<GameObject>();

    }

    // Start is called before the first frame update
    void Start()
    {
        _camera = Camera.main;
        if (!isTestingMobile)
        {
            isMobile = Application.platform == RuntimePlatform.Android ||
                       Application.platform == RuntimePlatform.IPhonePlayer;
        }
        
        if (bulletPrefab == null) return;
        for (int i = 0; i < 25; i++)
        {
            var bulletGameObject = Instantiate(bulletPrefab);
            var bullet = bulletGameObject.transform.GetComponent<Bullet>();
            bullet.xBounds = bulletBoundaryX;
            bullet.yBounds = bulletBoundaryY;
            bullet.instigator = this.gameObject;
            
            _inactiveBullets.AddLast(bulletGameObject);
            bulletGameObject.SetActive(false);
        }
        
    }

    public void Shoot()
    {
        if (_inactiveBullets.Last != null)
        {
            var bulletFab = _inactiveBullets.Last;
            _inactiveBullets.Remove(bulletFab);
            bulletFab.Value.SetActive(true);
            _activeBullets.AddLast(bulletFab);
            Bullet bullet = bulletFab.Value.GetComponent<Bullet>();
            bullet.Fire(transform,bulletForce);
        }
        else
        {
            var prefab = Instantiate(bulletPrefab);
            var bullet = prefab.GetComponent<Bullet>();
            _activeBullets.AddLast(prefab);
            bullet.xBounds = bulletBoundaryX;
            bullet.yBounds = bulletBoundaryY;
            bullet.instigator = this.gameObject;
            bullet.Fire(transform,bulletForce);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isMobile) GetTouchInput();
        else GetTraditionalInput();

        CheckBoundary();
    }

    public void ReturnToPool(GameObject bullet)
    {
        bullet.SetActive(false);
        _inactiveBullets.AddLast(bullet);
    }
    

    private void CheckBoundary()
    {
        if (transform.position.x < xBoundary.min) transform.position = new Vector3(xBoundary.min, transform.position.y, 0);
        if (transform.position.x > xBoundary.max) transform.position = new Vector3(xBoundary.max, transform.position.y, 0);
    }

    private void Move()
    {
        transform.position = destination;
    }

    private void GetTraditionalInput()
    {
        float axisX = Input.GetAxisRaw("Horizontal");
        transform.Translate(new Vector3(axisX, 0, 0) * (Time.deltaTime * moveSpeed));

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Shoot();
        }
    }

    private void GetTouchInput()
    {
        foreach (var touch in Input.touches)
        {
            var worldPosition = _camera.ScreenToWorldPoint(touch.position);

           if (worldPosition.x > 0)
           {
               transform.Translate(Vector3.right * (moveSpeed * Time.deltaTime));
           }
           else if (worldPosition.x < 0)
           {
               transform.Translate(Vector3.left * (moveSpeed * Time.deltaTime));

           }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            GameManager.Instance.ChangeScene(Scenes.MainMenu);
        }
    }
}
