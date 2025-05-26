using System;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer), typeof(Animator)) ]
public abstract class Enemy : MonoBehaviour
{


    protected SpriteRenderer sr;
    protected Animator anim;
    protected int health;
    [SerializeField] protected int maxHealth;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected virtual void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        if (maxHealth <= 0) maxHealth = 5;

        health = maxHealth;

    }

    public virtual void TakeDamage(int DamageValue, DamageType DamageType = DamageType.Default)
    {
        health -= DamageValue;
        if (health <= 0)
        {
            anim.SetTrigger("Death");

            if (transform.parent != null)
                Destroy(transform.parent.gameObject, 0.5f);
            else
                Destroy(gameObject, 0.5f);


        }
    }

    public enum DamageType
    {
        Default,
        JumpedOn,
    }

}

