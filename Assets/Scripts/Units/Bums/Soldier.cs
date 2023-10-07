using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Soldier : MovableUnit
{
    protected override void Start()
    {
        if (PersistantManagerScript.Instance.isEnteringMode)
        {
            targetPosition = new Vector3(2.4f, 2.2f, 0f);
        }
        SetValues();
    }
    protected override void FixedUpdate()
    {
        if (animator.GetBool("isDead")) return;
        if (animator.GetBool("isInAttack")) return;
        Swarm();
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

    protected void DeathSoldier()
    {
        Destroy(gameObject);
    }
}
