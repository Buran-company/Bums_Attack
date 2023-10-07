using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BoomBaloons : Trap
{
    
    protected override void Start()
    {
        SetValues();
    }
    protected override void FixedUpdate()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        CollidedWithSmth(collider);
    }

    public void Explode()
    {
        Collider2D[] rangeCheck = Physics2D.OverlapCircleAll(new Vector3(transform.position.x - 0.2f, transform.position.y + 0.5f, transform.position.z), 0.7f);
        for (int i = 0; i < rangeCheck.Length; i++)
        {
            if (rangeCheck[i].CompareTag("Bum") || rangeCheck[i].CompareTag("Player") || rangeCheck[i].CompareTag("Clone"))
            {
                rangeCheck[i].GetComponent<Health>().Damage(damage);
            }
        }
        Destroy(gameObject);
    }

    /*private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        UnityEditor.Handles.DrawWireDisc(new Vector3(transform.position.x - 0.2f, transform.position.y + 0.5f, transform.position.z), Vector3.forward, 0.7f);
    }*/
}
