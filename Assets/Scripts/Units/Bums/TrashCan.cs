using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashCan : MonoBehaviour
{
    [SerializeField]
    private GameObject effectPrefab;

    [Header("Animation Settings")]
    public Animator animator;

    public bool isReady = false;

    private readonly float MoveSpeed = 1.0f;
    private readonly float frequency = 1.5f;  // Speed of sine movement
    private readonly float magnitude = 2.0f;   // Size of sine movement
    private float timer;
    private Vector3 axis;
    private Vector3 pos;

    [SerializeField] private AudioClip flyingClip;
    [SerializeField] private AudioClip landedClip;
    [SerializeField] private AudioClip deathClip;
    void Start()
    {
        StartCoroutine(WaitForReady());
    }

    // Update is called once per frame
    void Update()
    {
        if (!isReady) return;
        pos += transform.right * Time.deltaTime * MoveSpeed;
        transform.position = pos + axis * Mathf.Sin((Time.time - timer) * frequency) * magnitude;
    }

    private IEnumerator WaitForReady()
    {
        yield return new WaitForSeconds(0.25f);
        pos = transform.position;
        axis = transform.up;
        timer = Time.time;
        isReady = true;
        StartCoroutine(TimeOfFlying());
        GetComponent<AudioSource>().clip = flyingClip;
        GetComponent<AudioSource>().Play();
    }

    private IEnumerator TimeOfFlying()
    {
        yield return new WaitForSeconds(2.0f);
        isReady = false;
        _ = Instantiate(effectPrefab, new Vector3(transform.localPosition.x - 0.0f, transform.localPosition.y + 0.8f, transform.localPosition.z), Quaternion.identity);
        animator.SetBool("isStay", true);
        GetComponent<AudioSource>().clip = landedClip;
        GetComponent<AudioSource>().Play();
        StartCoroutine(TimeOfStaying());
    }

    private IEnumerator TimeOfStaying()
    {
        yield return new WaitForSeconds(10.0f);
        animator.SetBool("isDead", true);
        GetComponent<AudioSource>().clip = deathClip;
        GetComponent<AudioSource>().Play();
    }

    private void Death()
    {
        Destroy(gameObject);
    }
}
