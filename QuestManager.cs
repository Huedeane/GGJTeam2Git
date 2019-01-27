using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/* Class Explanation
 * - Manager for storing quest
 */
public class QuestManager : MonoBehaviour {

    #region Variable   
    [SerializeField] private static QuestManager questManager;
    [SerializeField] private GameObject m_QuestUI;
    [SerializeField] private GameObject m_QuestListUI;
    [SerializeField] private GameObject m_ButtonPrefab;
    [SerializeField] private QuestE_QuestStatusDictionary m_QuestDictionary;
    #endregion

    #region Getter & Setter
    public static QuestManager Instance
    {
        get
        {
            if (!questManager)
            {
                questManager = FindObjectOfType(typeof(QuestManager)) as QuestManager;

                if (!questManager)
                {
                    Debug.LogError("Error: There is no active QuestManager attached to a gameObject");
                }
                else
                {
                    questManager.Init();
                }
            }

            return questManager;
        }
        set
        {
            questManager = value;
        }
    }
    public GameObject QuestUI
    {
        get
        {
            return m_QuestUI;
        }

        set
        {
            m_QuestUI = value;
        }
    }
    public GameObject QuestListUI
    {
        get
        {
            return m_QuestListUI;
        }

        set
        {
            m_QuestListUI = value;
        }
    }
    public GameObject ButtonPrefab
    {
        get
        {
            return m_ButtonPrefab;
        }

        set
        {
            m_ButtonPrefab = value;
        }
    }
    public QuestE_QuestStatusDictionary QuestDictionary
    {
        get
        {
            return m_QuestDictionary;
        }

        set
        {
            m_QuestDictionary = value;
        }
    }
    #endregion

    #region Setup
    private void Awake()
    {
        Instance = Instance;
    }
    void Init()
    {
        if (m_QuestDictionary == null)
        {
            m_QuestDictionary = new QuestE_QuestStatusDictionary();
        }
    }

    void OnEnable()
    {
        EventManager.StartListening("QuestAdded", AddQuest);     
        EventManager.StartListening("ConditionComplete", CheckQuestCompletion);
        EventManager.StartListening("QuestCompleted", CompleteQuest);
    }

    void OnDisable()
    {
        EventManager.StopListening("QuestAdded", AddQuest);
        EventManager.StopListening("ConditionComplete", CheckQuestCompletion);
        EventManager.StopListening("QuestCompleted", CompleteQuest);
    }


    #endregion

    #region Private Method
    /* Trigger By: QuestAdded
     * Param Needed: Quest 
     * Add quest to the quest list
     */
    private void AddQuest()
    {
        foreach (Quest quest in EventManager.eventObjectList) {
          
            if (!QuestDictionary.ContainsKey(quest))
            {         
                QuestDictionary.Add(quest, E_QuestStatus.InProgress);
            }
            else
            {
                
                Debug.LogError("Error: Quest that is being added already exist");
            }
        }      
    }

    /* Trigger By: ConditionComplete
     * Triggers: QuestCompleted
     * Set quest to satsified if quest condition is true in as the condition tracker
     */
    private void CheckQuestCompletion()
    {
        //Cycle through quest that are in progress and change their status to satsified

        var cloneQuestLog = m_QuestDictionary.ToDictionary(entry => entry.Key,
                                               entry => entry.Value);

        foreach (KeyValuePair<Quest,E_QuestStatus> questLog in cloneQuestLog) {
            Quest questKey = questLog.Key;
            E_QuestStatus questValue = questLog.Value;
            if (questValue == E_QuestStatus.InProgress) {
                bool thisBool = true;
                if (ConditionManager.Instance.ConditionTrackerDictionary.TryGetValue(questKey.QuestCompleteCondition, out thisBool))
                {
                    if (questKey.CompletedThrough == E_CompletedThrough.Itself)
                    {
                        EventManager.TriggerEvent("QuestCompleted");
                    }

                    else
                    {
                        m_QuestDictionary[questKey] = E_QuestStatus.Satsified;
                    }
                }
                else
                {
                    Debug.LogError("No Quest has been found to be completed");
                }
            }
        }
    }

    /* Trigger By: QuestCompleted
     * Param Needed: Quest 
     * Set quest to be complete
     */
    private void CompleteQuest()
    {
        foreach (Quest quest in EventManager.eventObjectList)
        {
            m_QuestDictionary[quest] = E_QuestStatus.Completed;
        }
    }
    #endregion

    #region Public Method
    public bool CheckQuestCompletion(Quest quest)
    {
        E_QuestStatus thisQuestStatus = E_QuestStatus.Satsified;
        if (m_QuestDictionary.TryGetValue(quest, out thisQuestStatus))
        {
            return true;
        }
        else
        {
            Debug.LogError("No Quest has been found to be satsified");
        }
        return false;
    }
    #endregion
}
