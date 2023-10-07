using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Janitor : MovableUnit
{
    private bool isSpawning = true;
    protected override void Start()
    {
        SetValues();
        StartCoroutine(Spawning());
    }
    protected override void FixedUpdate()
    {
        if (isSpawning) return;
        if (animator.GetBool("isDead")) return;
        if (animator.GetBool("isInAttack")) return;
        Swarm();
    }

    protected IEnumerator Spawning()
    {
        yield return new WaitForSeconds(0.8f);
        isSpawning = false;
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        ExitCollidedWithSmth(collider);
    }

    private void OnTriggerStay2D(Collider2D collider)
    {
        CollidedWithSmth(collider);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        // Abort if we already attacked recently.
        if (Time.time - _lastAttackTime < GetComponent<Health>().attackSpeed) return;
        if (animator.GetBool("isDead")) return;

        CollisionStayed(collision);
    }

    protected void DeathJanitor()
    {
        Destroy(gameObject);
    }
}
