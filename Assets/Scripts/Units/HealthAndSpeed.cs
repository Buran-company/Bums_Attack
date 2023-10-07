using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{

    [Header("Animation Settings")]
    public Animator animator;

    private int health;
    private int maxHealth;

    private float defaultSpeed;
    private float defaultAttackSpeed;
    private float maxBoostSpeed = 0.0f;
    private float maxBoostAttackSpeed = 0.0f;
    private float maxDebuffSpeed = 0.0f;
    private float maxDebuffAttackSpeed = 0.0f;

    public float speed { get; private set; }
    public float attackSpeed { get; private set; }

    [SerializeField] private AudioClip deathClip;

    public void SetHealth(int maxHealth, int health)
    {
        this.maxHealth = maxHealth;
        this.health = health;
    }

    public void SetSpeed(float speed, float attackSpeed)
    {
        this.speed = speed;
        this.attackSpeed = attackSpeed;
        defaultSpeed = speed;
        defaultAttackSpeed = attackSpeed;
    }

    public void Damage(int amount)
    {
        if (amount < 0)
        {
            throw new System.ArgumentOutOfRangeException("Cannot Have Negative Damage");
        }

        health -= amount;

        if (health <= 0) 
        {
            Die();
        }
    }

    public void Heal(int amount)
    {
        if (amount < 0)
        {
            throw new System.ArgumentOutOfRangeException("Cannot Have Negative Heal");
        }

        if (health + amount > maxHealth)
        {
            health = maxHealth;
        }
        else
        {
            health += amount;
        }
    }

    public void BoostSpeed(float percentage)
    {
        if (percentage > maxBoostSpeed)
        {
            speed = defaultSpeed + defaultSpeed * percentage / 100.0f;
            maxBoostSpeed = percentage;
        }
    }

    public void DebuffSpeed(float percentage)
    {
        if (percentage > maxDebuffSpeed)
        {
            speed = defaultSpeed - defaultSpeed * percentage / 100.0f;
            maxDebuffSpeed = percentage;
        }
    }

    public void ReturnDefaultSpeed()
    {
        speed = defaultSpeed;
        maxBoostSpeed = 0.0f;
        maxDebuffSpeed = 0.0f;
    }

    public void BoostAttackSpeed(float percentage)
    {
        if (percentage > maxBoostAttackSpeed)
        {
            attackSpeed = defaultAttackSpeed - defaultAttackSpeed * percentage / 100.0f;
            maxBoostAttackSpeed = percentage;
        }
    }

    public void DebuffAttackSpeed(float percentage)
    {
        if (percentage > maxDebuffAttackSpeed)
        {
            attackSpeed = defaultAttackSpeed + defaultAttackSpeed * percentage / 100.0f;
            maxDebuffAttackSpeed = percentage;
        }
    }

    public void ReturnDefaultAttackSpeed()
    {
        attackSpeed = defaultAttackSpeed;
        maxBoostAttackSpeed = 0.0f;
        maxDebuffAttackSpeed = 0.0f;
    }

    private void Die()
    {
        animator.SetBool("isDead", true);
        GetComponent<AudioSource>().clip = deathClip;
        GetComponent<AudioSource>().loop = false;
        GetComponent<AudioSource>().Play();
    }
}
