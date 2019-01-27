using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; set; }
    public bool uiAccess;
    public int mapSpawnPoint;
    public TextMeshProUGUI interactionText;

    private void Awake()
    {
        MakeSingleton();
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
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        Debug.Log(SceneManager.GetActiveScene().name);
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().transform.position = GetSpawnPoint(EventManager.EventInt);
        interactionText = GameObject.FindGameObjectWithTag("Interaction").GetComponent<TextMeshProUGUI>();
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
