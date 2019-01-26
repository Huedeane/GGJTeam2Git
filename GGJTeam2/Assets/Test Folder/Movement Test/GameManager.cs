using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; set; }
    public int mapSpawnPoint;
    public bool mapSpawn;

    private void Awake()
    {
        MakeSingleton();
        mapSpawn = false;
    }

    private void MakeSingleton()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void OnEnable()
    {
        EventManager.StartListening("SceneChange", StartSceneChange);
    }

    void StartSceneChange() {
        StartCoroutine("SceneChange");
    }

    IEnumerator SceneChange() {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().transform.position = GetSpawnPoint(EventManager.EventInt);
    }

    public Vector2 GetSpawnPoint(int SpawnConnectionID)
    {
        GameObject[] AreaTransitionList = GameObject.FindGameObjectsWithTag("SpawnPoint");
        
        foreach (GameObject AreaTransition in AreaTransitionList)
        {
            if (AreaTransition.GetComponent<SceneTransition>().SpawnID == SpawnConnectionID)
            {
                return AreaTransition.GetComponent<SceneTransition>().SpawnPoint.transform.position;
            }
        }
        return new Vector2(0, 0);
    }
}
