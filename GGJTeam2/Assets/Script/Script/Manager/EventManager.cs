using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class EventManager : MonoBehaviour
{

    private static List<ScriptableObject> m_eventObjectList;
    private static ScriptableObject m_eventObject;
    private static int m_eventInt;
    private static Item m_eventItem;
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

    public static List<ScriptableObject> eventObjectList
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

    public static ScriptableObject eventObject
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

    public static int EventInt { get => m_eventInt; set => m_eventInt = value; }
    public static Item EventItem { get => m_eventItem; set => m_eventItem = value; }

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
            eventObjectList = new List<ScriptableObject>();
        }
    }

    private static void SetEventObjectList(ScriptableObject scriptableObject)
    {
        eventObjectList.Clear();
        eventObjectList.Add(scriptableObject);
    }
    private static void SetEventObjectList(List<ScriptableObject> setList)
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

    public static void TriggerEvent(string eventName, int eventInt)
    {
        m_eventInt = eventInt;
        UnityEvent thisEvent = null;
        if (Instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            Debug.Log("Start Event: " + eventName);
            thisEvent.Invoke();
            Debug.Log("End Event: " + eventName);
        }
    }

    public static void TriggerEvent(string eventName, Item eventItem)
    {
        m_eventItem = eventItem;
        UnityEvent thisEvent = null;
        if (Instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            Debug.Log("Start Event: " + eventName);
            thisEvent.Invoke();
            Debug.Log("End Event: " + eventName);
        }
    }

    public static void TriggerEvent(string eventName, ScriptableObject scriptableObjects)
    {

        SetEventObjectList(Instantiate(scriptableObjects));
        UnityEvent thisEvent = null;
        if (Instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            Debug.Log("Start Event: " + eventName);
            thisEvent.Invoke();
            Debug.Log("End Event: " + eventName);
        }
    }

    public static void TriggerEvent(string eventName, List<ScriptableObject> scriptableObjectsList)
    {
        List<ScriptableObject> newList = new List<ScriptableObject>();
        foreach (ScriptableObject scriptableObjects in scriptableObjectsList)
        {
            newList.Add(Instantiate(scriptableObjects));
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
