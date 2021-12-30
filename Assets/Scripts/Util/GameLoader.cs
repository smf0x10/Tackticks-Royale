using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLoader : MonoBehaviour
{
    private void Awake()
    {
        Debug.Log("Awake");
        Debug.Log(FindObjectOfType<SpawnManager>());
    }
}
