using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snowflake : MonoBehaviour
{
    private float FlyUp = 1.0f;

    private void Start()
    {
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "Menu")
        {
            FlyUp = -1.0f;
        }
    }
    void Update()
    {
        transform.position = new Vector3(transform.position.x - FlyUp * 2.0f * Time.deltaTime, transform.position.y - FlyUp * 2.0f * Time.deltaTime);
        if (transform.position.y < -2.3f || transform.position.y > 18.0f) Destroy(gameObject);
    }
}
