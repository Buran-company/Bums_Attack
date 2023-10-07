using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashCanEffect : MonoBehaviour
{
    [SerializeField]
    private UnitData data;

    [Header("Animation Settings")]
    public Animator animator;

    private void Start()
    {
        StartCoroutine(Crash());
        StartCoroutine(TimeOfStaying());
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (GetComponent<BoxCollider2D>().enabled && (collision.collider.CompareTag("Clone") || collision.collider.CompareTag("Bum")))
        {
            collision.collider.GetComponent<Health>().Damage(data.damage);
        }
    }

    private void OnTriggerStay2D(Collider2D collider)
    {
        if (animator.GetBool("isDead")) return;
        if (collider.CompareTag("Clone"))
        {
            collider.GetComponent<Health>().DebuffSpeed(30.0f);
            collider.GetComponent<Health>().DebuffAttackSpeed(30.0f);
            var spriteRendererColor = collider.GetComponent<SpriteRenderer>().color;
            collider.GetComponent<SpriteRenderer>().color = new Color(0.0f, spriteRendererColor.g, spriteRendererColor.b, spriteRendererColor.a);
        }
        else if (collider.CompareTag("Bum"))
        {
            collider.GetComponent<Health>().BoostSpeed(30.0f);
            collider.GetComponent<Health>().BoostAttackSpeed(30.0f);
            var spriteRendererColor = collider.GetComponent<SpriteRenderer>().color;
            collider.GetComponent<SpriteRenderer>().color = new Color(spriteRendererColor.r, 0.0f, spriteRendererColor.b, spriteRendererColor.a);
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.CompareTag("Clone") || collider.CompareTag("Bum"))
        {
            collider.GetComponent<Health>().ReturnDefaultSpeed();
            collider.GetComponent<Health>().ReturnDefaultAttackSpeed();
            var spriteRendererColor = collider.GetComponent<SpriteRenderer>().color;
            collider.GetComponent<SpriteRenderer>().color = new Color(255.0f, 255.0f, spriteRendererColor.b, spriteRendererColor.a);
        }
    }

    private IEnumerator Crash()
    {
        yield return new WaitForSeconds(0.1f);
        GetComponent<BoxCollider2D>().enabled = false;
    }

    private IEnumerator TimeOfStaying()
    {
        yield return new WaitForSeconds(10.0f);
        animator.SetBool("isDead", true);
    }

    private void Death()
    {
        Destroy(gameObject);
    }
}
