using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistantManagerScript : MonoBehaviour
{
    public static PersistantManagerScript Instance { get; private set; }

    public bool isEnteringMode;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
