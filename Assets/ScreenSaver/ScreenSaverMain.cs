using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScreenSaverMain : MonoBehaviour
{

    [SerializeField]
    private GameObject snowflakePrefab;

    public TextMeshProUGUI screenSaverText;

    public static ScreenSaverMain Instance { get; private set; }

    public bool isTouchable1 = false;
    public bool isTouchable2 = false;
    // Start is called before the first frame update

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogWarning("Found more then one dialogue manager in the scene");
        }
        Instance = this;
    }
    void Start()
    {
        screenSaverText.enabled = false;
        StartCoroutine(WaitAtStart());
        StartCoroutine(SpawnSnowflake());
        StartCoroutine(EndScreenSaver());
    }

    private IEnumerator SpawnSnowflake()
    {
        yield return new WaitForSeconds(0.5f);
        _ = Instantiate(snowflakePrefab, new Vector3(Random.Range(-3f, 9f), 5.33f), Quaternion.identity);
        StartCoroutine(SpawnSnowflake());
    }

    private IEnumerator EndScreenSaver()
    {
        yield return new WaitForSeconds(50.0f);
        screenSaverText.enabled = true;
        yield return new WaitForSeconds(7.0f);
        UnityEngine.SceneManagement.SceneManager.LoadScene(2);
    }

    private IEnumerator WaitAtStart()
    {
        yield return new WaitForSeconds(2.0f);
        isTouchable1 = true;
    }

    public void IsTouched()
    {
        StartCoroutine(WaitAfterTouch());
    }

    private IEnumerator WaitAfterTouch()
    {
        yield return new WaitForSeconds(2.0f);
        isTouchable2 = true;
    }
}
