using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    [Header("Animation Settings")]
    public Animator animator;

    [SerializeField]
    private AudioClip triggerSound;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(TimeOfFalling());
    }

    // Update is called once per frame
    void Update()
    {
        if (animator.GetBool("isDead")) return;
        transform.position = new Vector3(transform.position.x, transform.position.y - 4.0f * Time.deltaTime);
    }

    private IEnumerator TimeOfFalling()
    {
        yield return new WaitForSeconds(2.0f);
        animator.SetBool("isDead", true);
        GetComponent<AudioSource>().clip = triggerSound;
        GetComponent<AudioSource>().Play();
    }

    private void Death()
    {
        Destroy(gameObject);
    }
}
