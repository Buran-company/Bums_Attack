using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

public class FieldOfView : MonoBehaviour
{
    [Header("Animation Settings")]
    public Animator animator;

    public List<LayerMask> targetLayers;

    public LayerMask obstructionLayer;

    public bool isSeeSmth { get; private set; }
    public Vector3 targetPosition = new Vector3(0f, 0f, 0f);
    public Vector3 curPositionForPatrolling = new Vector3(0f, 0f, 0f);
    public bool isInPatrol = false;

    public bool isTurnedLeft;
    public float fieldOfViewRadius;
    public float attackDistance;

    [SerializeField] private AudioClip attackClip;
    void Start()
    {
        if (PersistantManagerScript.Instance.isEnteringMode == true)
        {
            fieldOfViewRadius = 7f;
        }
        StartCoroutine(FOVCheck());
    }

    private IEnumerator FOVCheck()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.2f);
            FOV();
        }
    }

    public IEnumerator Patrolling()
    {
        isInPatrol = true;
        animator.SetBool("isInAttack", false);
        GetComponent<AudioSource>().Stop();
        if (curPositionForPatrolling == new Vector3(0f, 0f, 0f))
        {
            curPositionForPatrolling = gameObject.transform.position;
        }
        targetPosition = new Vector3(Random.Range(curPositionForPatrolling.x - 1f, curPositionForPatrolling.x + 1f), Random.Range(curPositionForPatrolling.y - 1f, curPositionForPatrolling.y + 1f), 0);
        yield return new WaitForSeconds(5f);
        isInPatrol = false;
    }

    private void FOV()
    {
        foreach (var targetLayer in targetLayers)
        {
            Collider2D[] rangeCheck = Physics2D.OverlapCircleAll(transform.position, fieldOfViewRadius, targetLayer);

            if (rangeCheck.Length > 0)
            {
                for (int i = 0; i < rangeCheck.Length; i++)
                {
                    Transform target = rangeCheck[i].transform;

                    if ((isTurnedLeft && target.position.x < transform.position.x) || (!isTurnedLeft && target.position.x > transform.position.x))
                    {
                        Vector2 directionToTarget = (target.position - transform.position).normalized;
                        float distanceToTarget = Vector2.Distance(transform.position, target.position);
                        if (!Physics2D.Raycast(transform.position, directionToTarget, distanceToTarget, obstructionLayer))
                        {
                            curPositionForPatrolling = new Vector3(0f, 0f, 0f);
                            isInPatrol = false;
                            isSeeSmth = true;
                            targetPosition = target.position;
                            if (distanceToTarget > attackDistance)
                            {
                                animator.SetBool("isInAttack", false);
                                GetComponent<AudioSource>().Stop();
                            }
                            else
                            {
                                animator.SetBool("isInAttack", true);
                                if (GetComponent<AudioSource>().clip != attackClip && !animator.GetBool("isDead"))
                                {
                                    GetComponent<AudioSource>().clip = attackClip;
                                    GetComponent<AudioSource>().Play();
                                }
                            }
                            break;
                        }
                        else
                        {
                            isSeeSmth = false;
                        }
                    }
                    else
                    {
                        isSeeSmth = false;
                    }
                }
            }
            else
            {
                animator.SetBool("isInAttack", false);
                GetComponent<AudioSource>().Stop();
                isSeeSmth = false;
            }

            if (isSeeSmth)
            {
                break;
            }
        }

        if (!isSeeSmth && isInPatrol == false)
        {
            StartCoroutine(Patrolling());
        }
    }
}
