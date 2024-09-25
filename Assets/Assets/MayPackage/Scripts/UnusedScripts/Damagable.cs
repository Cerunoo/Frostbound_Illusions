using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damagable : MonoBehaviour
{
    Animator animator;

    [SerializeField]
    private int _maxHealth = 1;

    public int MaxHealth
    {
        get
        {
            return _maxHealth;
        }
        set
        {
            _maxHealth = value;
        }
    }

    private int _health = 1;

    public int Health
    {
        get
        {
            return _health;
        }
        set
        {
            _health = value;

            if( _health < 0)
            {
               IsAlive = false;
            }
        }
    }

    private bool _isAlive = true;

    public bool IsAlive
    {
        get
        {
            return _isAlive;
        }
        set
        {
            _isAlive = value;
            animator.SetBool(AnimationStrings.isAlive, value);
        }
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void Hit(int damage)
    {
        
    }
}
