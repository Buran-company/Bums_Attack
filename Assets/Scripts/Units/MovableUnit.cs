using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovableUnit : MonoBehaviour
{
    [SerializeField]
    private UnitData data;

    [Header("Animation Settings")]
    public Animator animator;

    private int damage;
    protected float _lastAttackTime;
    private bool isBum;
    protected bool isMoving;

    public Vector3 targetPosition;

    [SerializeField] private AudioClip attackClip;

    protected virtual void Start()
    {

    }

    protected virtual void FixedUpdate()
    {

    }

    protected void SetValues()
    {
        GetComponent<FieldOfView>().isTurnedLeft = data.isTurnedLeft;
        GetComponent<FieldOfView>().fieldOfViewRadius = data.fieldOfViewRadius;
        GetComponent<FieldOfView>().attackDistance = data.attackDistance;
        GetComponent<Health>().SetHealth(data.hp, data.hp);
        GetComponent<Health>().SetSpeed(data.speed, data.attackSpeed);
        damage = data.damage;
        isBum = data.isBum;
    }

    protected void Swarm()
    {
        targetPosition = GetComponent<FieldOfView>().targetPosition;
        if (targetPosition == gameObject.transform.position)
        {
            return;
        }
        var isTurnedLeft = GetComponent<FieldOfView>().isTurnedLeft;
        if ((gameObject.transform.position.x > targetPosition.x && !isTurnedLeft) || (gameObject.transform.position.x < targetPosition.x && isTurnedLeft))
        {
            GetComponent<FieldOfView>().isTurnedLeft = !isTurnedLeft;
            GetComponent<SpriteRenderer>().flipX = !GetComponent<SpriteRenderer>().flipX;
        }
        if (targetPosition != new Vector3(0f, 0f, 0f))
        {
            gameObject.transform.position = Vector2.MoveTowards(gameObject.transform.position, targetPosition, GetComponent<Health>().speed * Time.deltaTime);
        }
    }

    protected void CollidedWithSmth(Collider2D collider)
    {
        if (collider.CompareTag("Obstacle"))
        {
            var spriteRendererColor = collider.GetComponent<SpriteRenderer>().color;
            collider.GetComponent<SpriteRenderer>().color = new Color(spriteRendererColor.r, spriteRendererColor.g, spriteRendererColor.b, 0.5f);
            return;
        }
    }

    protected void ExitCollidedWithSmth(Collider2D collider)
    {
        if (collider.CompareTag("Obstacle"))
        {
            var spriteRendererColor = collider.GetComponent<SpriteRenderer>().color;
            collider.GetComponent<SpriteRenderer>().color = new Color(spriteRendererColor.r, spriteRendererColor.g, spriteRendererColor.b, 1f);
            return;
        }
    }

    protected void CollisionStayed(Collision2D collision)
    {
        if (animator.GetBool("isDead")) return;
        if (isBum)
        {
            // CompareTag is cheaper than .tag ==
            if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Clone"))
            {
                animator.SetBool("isInAttack", true);
                if (GetComponent<AudioSource>().clip != attackClip)
                {
                    GetComponent<AudioSource>().clip = attackClip;
                    GetComponent<AudioSource>().Play();
                }
                collision.collider.GetComponent<Health>().Damage(damage);

                // Remember that we recently attacked.
                _lastAttackTime = Time.time;
            }
        }
        else
        {
            if (collision.gameObject.CompareTag("Bum"))
            {
                animator.SetBool("isInAttack", true);
                if (GetComponent<AudioSource>().clip != attackClip)
                {
                    GetComponent<AudioSource>().clip = attackClip;
                    GetComponent<AudioSource>().Play();
                }
                collision.collider.GetComponent<Health>().Damage(damage);

                // Remember that we recently attacked.
                _lastAttackTime = Time.time;
            }
        }
    }
}
