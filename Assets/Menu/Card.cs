using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class Card : MonoBehaviour
{
    Vector3 scaleChange = new Vector2(0.01f, 0.01f);
    private bool grow = false;
    private float normalizedTime1 = 0.0f;
    private float normalizedTime2 = 0.0f;
    private bool isFirstTime = true;

    [Header("Animation Settings")]
    public Animator animator;
    private void Clicked()
    {
        grow = !grow;
        normalizedTime2 = normalizedTime1;
        normalizedTime1 = Time.time;

        var time = normalizedTime1 - normalizedTime2;
        if (time > 0.27f || isFirstTime)
        {
            isFirstTime = false;
            time = 0.0f;
        }
        
        if (grow)
        {
            animator.Play("OnClick", 0, time);
        }
        else
        {
            animator.Play("OnClickAgain", 0, time);
        }
        
    }

    private void Update()
    {
        if (grow && gameObject.transform.localScale.x < 0.75f)
        {
            gameObject.transform.localScale += scaleChange;
        }
        else if (!grow && gameObject.transform.localScale.x > 0.5f)
        {
            gameObject.transform.localScale -= scaleChange;
        }
    }
}
