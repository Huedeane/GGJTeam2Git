using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class EventManager : MonoBehaviour
{

    private static List<GameObject> m_eventObjectList;
    private static GameObject m_eventObject;
    private Dictionary<string, UnityEvent> eventDictionary;
    private static EventManager eventManager;

    public static EventManager Instance
    {
        get
        {
            if (!eventManager)
            {
                eventManager = FindObjectOfType(typeof(EventManager)) as EventManager;

                if (!eventManager)
                {
                    Debug.LogError("There needs to be one active EventManger script on a GameObject in your scene.");
                }
                else
                {
                    eventManager.Init();
                }
            }

            return eventManager;
        }
        set
        {
            eventManager = value;
        }
    }

    public static List<GameObject> eventObjectList
    {
        get
        {
            return m_eventObjectList;
        }

        set
        {
            m_eventObjectList = value;
        }
    }

    public static GameObject eventObject
    {
        get
        {
            return m_eventObjectList[0];
        }

        set
        {
            m_eventObject = value;
        }
    }

    private void Awake()
    {
        Instance = Instance;
    }

    void Init()
    {
        if (eventDictionary == null)
        {
            eventDictionary = new Dictionary<string, UnityEvent>();
        }
        if (eventObjectList == null)
        {
            eventObjectList = new List<GameObject>();
        }
    }

    private static void SetEventObjectList(GameObject gameObject)
    {
        eventObjectList.Clear();
        eventObjectList.Add(gameObject);
    }
    private static void SetEventObjectList(List<GameObject> setList)
    {
        eventObjectList.Clear();
        eventObjectList = setList;
    }

    public static void StartListening(string eventName, UnityAction listener)
    {
        UnityEvent thisEvent = null;
        if (Instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.AddListener(listener);
        }
        else
        {
            thisEvent = new UnityEvent();
            thisEvent.AddListener(listener);
            Instance.eventDictionary.Add(eventName, thisEvent);
        }
    }

    public static void StopListening(string eventName, UnityAction listener)
    {
        if (eventManager == null) return;
        UnityEvent thisEvent = null;
        if (Instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.RemoveListener(listener);
        }
    }

    public static void TriggerEvent(string eventName)
    {
        UnityEvent thisEvent = null;
        if (Instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            Debug.Log("Start Event: " + eventName);
            thisEvent.Invoke();
            Debug.Log("End Event: " + eventName);
        }
    }

    public static void TriggerEvent(string eventName, GameObject gameObject)
    {

        SetEventObjectList(Instantiate(gameObject));
        UnityEvent thisEvent = null;
        if (Instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            Debug.Log("Start Event: " + eventName);
            thisEvent.Invoke();
            Debug.Log("End Event: " + eventName);
        }
    }

    public static void TriggerEvent(string eventName, List<GameObject> gameObjectsList)
    {
        List<GameObject> newList = new List<GameObject>();
        foreach (GameObject gameObject in gameObjectsList)
        {
            newList.Add(gameObject);
        }

        SetEventObjectList(newList);
        UnityEvent thisEvent = null;
        if (Instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            Debug.Log("Start Event: " + eventName);
            thisEvent.Invoke();
            Debug.Log("End Event: " + eventName);
        }
    }



}
