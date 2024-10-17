using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionFX : MonoBehaviour
{
    public Animator animator;

    private void Awake()
    {
        if (animator == null)
            animator = GetComponent<Animator>();
        
    }

    // Start is called before the first frame update
    void Start()
    {
        animator.Play("Explosion");
        
        StartCoroutine(DestroyAfterPlay());
    }

    private bool CheckAnimation()
    {
        return animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1;
    }

    IEnumerator DestroyAfterPlay()
    {
        yield return new WaitUntil(CheckAnimation);
        Destroy(gameObject);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
