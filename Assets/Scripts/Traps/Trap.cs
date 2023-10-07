using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    [SerializeField]
    private TrapData data;

    [SerializeField]
    private AudioClip triggerSound;

    [Header("Animation Settings")]
    public Animator animator;

    protected int damage;

    protected virtual void Start()
    {

    }

    protected virtual void FixedUpdate()
    {

    }

    protected void SetValues()
    {
        damage = data.damage;
    }

    protected void CollidedWithSmth(Collider2D collider)
    {
        if (collider.CompareTag("Obstacle"))
        {
            var spriteRendererColor = collider.GetComponent<SpriteRenderer>().color;
            collider.GetComponent<SpriteRenderer>().color = new Color(spriteRendererColor.r, spriteRendererColor.g, spriteRendererColor.b, 0.5f);
            return;
        }
        if (collider.CompareTag("Player") || collider.CompareTag("Clone") || collider.CompareTag("Bum"))
        {
            animator.SetBool("isTriggered", true);
            GetComponent<AudioSource>().clip = triggerSound;
            GetComponent<AudioSource>().Play();
        }
    }
}
