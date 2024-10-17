using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireButton : MonoBehaviour
{
    public GameObject playerObject;
    [SerializeField] private static PlayerController playerController;

    private void Awake()
    {
        playerController = playerObject.GetComponent<PlayerController>();
    }

    void Start()
    {
        
    }

    public void ShootWeapon()
    {
        playerController.Shoot();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
