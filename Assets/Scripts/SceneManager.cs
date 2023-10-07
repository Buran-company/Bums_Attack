using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManager : MonoBehaviour
{
    public static SceneManager instance { get; private set; }

    [Header("Dialogues")]
    [SerializeField] private TextAsset dialogue0;
    [SerializeField] private AudioClip christmasinmurmanskClip;
    [SerializeField] private TextAsset dialogue1;

    [SerializeField]
    private GameObject soldierPrefab;

    [SerializeField]
    private GameObject janitorPrefab;

    [SerializeField]
    private GameObject boomBaloonsPrefab;

    [SerializeField]
    private GameObject trashPultPrefab;

    [SerializeField]
    private GameObject LehaPrefab;

    [SerializeField]
    private GameObject bombPrefab;

    private int countOfJanitors = 0;
    private int countOfBoomBaloons = 0;

    [SerializeField] private AudioClip battleClip;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Found more then one dialogue manager in the scene");
        }
        instance = this;

        PersistantManagerScript.Instance.isEnteringMode = true;
    }

    void Start()
    {
        DialogueManager.instance.EnterDialogueMode(dialogue0, christmasinmurmanskClip);
        //StartEducationBattle();
        //DialogueManager.instance.EnterDialogueMode(dialogue1);
        //StartBombardment();
    }

    public void StartEducationBattle()
    {
        GetComponent<AudioSource>().clip = battleClip;
        GetComponent<AudioSource>().Play();
        StartCoroutine(SpawnLeha());
        StartCoroutine(StartSecondDialogue());
        StartCoroutine(SpawnJanitors());
        for (int i = 0; i < 20; i++)
        {
            _ = Instantiate(soldierPrefab, new Vector3(Random.Range(-10f, -8f), Random.Range(0f, 4f), 0), Quaternion.identity);
        }
        for (int i = 0; i < 3; i++)
        {
            _ = Instantiate(trashPultPrefab, new Vector3(Random.Range(-10f, -8f), Random.Range(0f, 4f), 0), Quaternion.identity);
        }
        StartCoroutine(SpawnBoomBaloons());
    }

    private IEnumerator SpawnJanitors()
    {
        if (countOfJanitors < 17)
        {
            countOfJanitors++;
            _ = Instantiate(janitorPrefab, new Vector3(Random.Range(-3f, 0f), Random.Range(0f, 4f), 0), Quaternion.identity);
        }
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(SpawnJanitors());
    }

    private IEnumerator SpawnBoomBaloons()
    {
        if (countOfBoomBaloons < 4)
        {
            countOfBoomBaloons++;
            _ = Instantiate(boomBaloonsPrefab, new Vector3(Random.Range(-6f, -3f), Random.Range(1f, 3f), 0), Quaternion.identity);
        }
        yield return new WaitForSeconds(1f);
        StartCoroutine(SpawnBoomBaloons());
    }

    private IEnumerator SpawnLeha()
    {
        yield return new WaitForSeconds(10.0f);
        _ = Instantiate(LehaPrefab, new Vector3(1.24f, 2.29f, 0), Quaternion.identity);
    }

    private IEnumerator StartSecondDialogue()
    {
        yield return new WaitForSeconds(20.0f);
        StartCoroutine(AudioFadeOut.FadeOut(GetComponent<AudioSource>(), 1.0f));
        DialogueManager.instance.EnterDialogueMode(dialogue1, christmasinmurmanskClip);
    }

    public void StartBombardment()
    {
        _ = Instantiate(bombPrefab, new Vector3(-2.5f, 4.0f, 0), Quaternion.identity);
        _ = Instantiate(bombPrefab, new Vector3(-1.5f, 5.0f, 0), Quaternion.identity);
        _ = Instantiate(bombPrefab, new Vector3(-0.5f, 6.0f, 0), Quaternion.identity);
        _ = Instantiate(bombPrefab, new Vector3(0.5f, 7.0f, 0), Quaternion.identity);
        StartCoroutine(ScreenSaver());
    }

    private IEnumerator ScreenSaver()
    {
        yield return new WaitForSeconds(2.2f);
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }
}
