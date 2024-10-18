using System;
using System.Collections;
using System.Collections.Generic;
using Quincy.Interfaces;
using Quincy.Structs;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

public class PlayerController : MonoBehaviour ,IDamageable
{
    [SerializeField]
    private float moveSpeed = 5;
    [SerializeField]
    private Boundary Bounds;
    
    private Camera _camera;
    
    [SerializeField]
    private bool isTestingMobile = false;

    [SerializeField] private bool isMobile = true;
    
    private Vector3 destination;
    
    private Rigidbody2D rb2d;
    
    [SerializeField] GameObject bulletPrefab;

    private LinkedList<GameObject> _activeBullets;
    private LinkedList<GameObject> _inactiveBullets;
    
    public Boundary bulletBoundary;

    [SerializeField] private float bulletForce = 300;
    private IDamageable _damageableImplementation;



    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();

        _activeBullets = new LinkedList<GameObject>();
        _inactiveBullets = new LinkedList<GameObject>();
        bulletBoundary = GameManager.Instance.enemyBoundary;
        Bounds = GameManager.Instance.playerBoundary;
        Health = MaxHealth;

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
            bullet.Bounds = bulletBoundary;
            bullet.instigator = this.gameObject;
            
            _inactiveBullets.AddLast(bulletGameObject);
            bulletGameObject.SetActive(false);
        }
        
        transform.position = new Vector3(0, Bounds.y.max, 0);
        Health = MaxHealth;
        
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
            bullet.Bounds = bulletBoundary;
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
        if (transform.position.x < Bounds.x.min) transform.position = new Vector3(Bounds.x.min, transform.position.y, 0);
        if (transform.position.x > Bounds.x.max) transform.position = new Vector3(Bounds.x.max, transform.position.y, 0);
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
            TakeDamage(30, other.gameObject);
        }
    }

    public float Health
    {
        get { return _health; }
        set
        {
            _health = Mathf.Clamp(value,0,_maxHealth);
            if (_health == 0)
            {
                OnDeath();
            }
            HealthChanged?.Invoke(this);
        }
    }

    public event Action<PlayerController> HealthChanged;
    public event Action<GameObject> DeathEvent;
    public event Action<GameObject> HitEvent;
    private void OnHit(GameObject attacker)
    {
        HitEvent?.Invoke(attacker);
    }
    
    public void TakeDamage(float damage,GameObject attacker)
    {
        Health -= damage;
        OnHit(attacker);
    }

    private void OnDeath()
    {
        GameManager.Instance.ChangeScene(Scenes.Lose);
        DeathEvent?.Invoke(gameObject);
    }

    private float _health;

    public float MaxHealth
    {
        get { return _maxHealth; }
        set { _maxHealth = value; }
    }
    [SerializeField] float _maxHealth;


    public void Heal(float heal)
    {
        Health += heal;
    }
}
