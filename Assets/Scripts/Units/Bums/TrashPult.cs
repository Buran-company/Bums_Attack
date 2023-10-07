using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashPult : MovableUnit
{
    [SerializeField]
    private GameObject trashCanPrefab;

    private GameObject newTrashCan;

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

    private void LoadingShell()
    {
        // Abort if we already attacked recently.
        if (Time.time - _lastAttackTime < GetComponent<Health>().attackSpeed) return;

        newTrashCan = Instantiate(trashCanPrefab, new Vector3(transform.localPosition.x - 0.15f, transform.localPosition.y + 0.25f, transform.localPosition.z), Quaternion.identity);

        // Remember that we recently attacked.
        _lastAttackTime = Time.time;
    }

    protected void DeathTrashPult()
    {
        Destroy(gameObject);
    }
}
