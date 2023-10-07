using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    [SerializeField]
    private GameObject snowflakePrefab;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnSnowflake());
    }

    private IEnumerator SpawnSnowflake()
    {
        yield return new WaitForSeconds(0.2f);
        _ = Instantiate(snowflakePrefab, new Vector3(Random.Range(-10f, 7f), -1.0f), Quaternion.identity);
        StartCoroutine(SpawnSnowflake());
    }
}
