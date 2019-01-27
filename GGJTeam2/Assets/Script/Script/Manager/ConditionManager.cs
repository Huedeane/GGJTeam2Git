using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable] public class ConditionBoolDictionary : SerializableDictionary<Condition, bool> { }

/* Class Explanation
 * - Manager for checking condition
 */
public class ConditionManager : MonoBehaviour {
    #region Variable
    [SerializeField] private static ConditionManager conditionManager;
    [SerializeField] private ConditionBoolDictionary m_ConditionTrackerDictionary;
    #endregion

    #region Getter & Setter
    public static ConditionManager Instance
    {
        get
        {
            if (!conditionManager)
            {
                conditionManager = FindObjectOfType(typeof(ConditionManager)) as ConditionManager;

                if (!conditionManager)
                {
                    Debug.LogError("Error: There is no active ConditionManager attached to a gameObject");
                }
                else
                {
                    conditionManager.Init();
                }
            }

            return conditionManager;
        }
        set
        {
            conditionManager = value;
        }
    }
    public ConditionBoolDictionary ConditionTrackerDictionary
    {
        get
        {
            return m_ConditionTrackerDictionary;
        }

        set
        {
            m_ConditionTrackerDictionary = value;
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
        
        if (m_ConditionTrackerDictionary == null)
        {
            m_ConditionTrackerDictionary = new ConditionBoolDictionary();
        }
    }

    void OnEnable()
    {
        //Add and Remove from Condition Tracker
        EventManager.StartListening("QuestAdded", AddCondition);       
        EventManager.StartListening("QuestCompleted", RemoveCondition);

        /*
        //Update Condition Tracker
        EventManager.StartListening("QuestAdded", CheckQuestAddedCondition);      
        EventManager.StartListening("QuestCompleted", CheckQuestCompletedCondition);
        EventManager.StartListening("ItemObtained", CheckQuestItemObtainedCondition);
        EventManager.StartListening("ItemDropped", CheckQuestItemDroppedCondition);
        */
        EventManager.StartListening("DialoguePerformed", CheckDialoguePerformedCondition);
        EventManager.StartListening("ConditionSubsetFufilled", CheckConditionComplete);
    }

    void OnDisable()
    {
        //Add and Remove from Condition Tracker
        EventManager.StopListening("QuestAdded", AddCondition);
        EventManager.StopListening("QuestCompleted", RemoveCondition);

        //Update Condition Tracker
        EventManager.StopListening("QuestAdded", CheckQuestAddedCondition);
        EventManager.StopListening("QuestCompleted", CheckQuestCompletedCondition);
        EventManager.StopListening("ItemObtained", CheckQuestItemObtainedCondition);
        EventManager.StopListening("ItemDropped", CheckQuestItemDroppedCondition);
        EventManager.StopListening("DialoguePerformed", CheckDialoguePerformedCondition);
        EventManager.StopListening("ConditionSubsetFufilled", CheckConditionComplete);
    }
    #endregion

    #region Public Method
    //Manually Add Condition
    public void AddCondition(Condition condition)
    {
        
        if (!m_ConditionTrackerDictionary.ContainsKey(condition))
        {
            Condition conditionCopy = Instantiate(condition);
            m_ConditionTrackerDictionary.Add(conditionCopy, false);
        }
        else
        {
            Debug.LogError("Error: Condition already exist");
        }
    }

    //Manually Remove Condition
    public void RemoveCondition(Condition condition)
    {
        bool thisBool = true;
        if (m_ConditionTrackerDictionary.TryGetValue(condition, out thisBool))
        {
            m_ConditionTrackerDictionary.Remove(condition);
        }
        else
        {
            Debug.LogError("Error: Condition does not exist");
        }
    }

    //Check if condition is satsified
    public bool CheckCondition(Condition condition)
    {
        //If there is already a true condition in the condition tracker that matches then return true
        bool thisBool = true;
        if (m_ConditionTrackerDictionary.TryGetValue(condition, out thisBool))
        {
            return true;
        }
        else
        {
            //Check each catagories and see if those categories has been satsified
            int conditionPassed = 0;
            int conditionPassedNeed =
                condition.QuestDictionary.Count +
                condition.ItemDictionary.Count +
                condition.DialoguePerformedList.Count;


            if (condition.NeedQuest)
            {
                //Cross reference Quest Log in Quest Manager with condition
                foreach (KeyValuePair<Quest, E_QuestStatus> keyValue in condition.QuestDictionary)
                {
                    Quest questRequirement = keyValue.Key;
                    E_QuestStatus questStatusRequirement = keyValue.Value;

                    QuestE_QuestStatusDictionary questList = QuestManager.Instance.QuestDictionary;
                    E_QuestStatus actual;
                    E_QuestStatus stored;

                    switch (questStatusRequirement)
                    {
                        case E_QuestStatus.Completed:
                            actual = E_QuestStatus.Completed;
                            if (questList.TryGetValue(questRequirement, out stored) && stored == actual)
                            {
                                conditionPassed++;
                            }
                            break;
                        case E_QuestStatus.Satsified:
                            actual = E_QuestStatus.Satsified;
                            if (questList.TryGetValue(questRequirement, out stored) && stored == actual)
                            {
                                conditionPassed++;
                            }
                            break;
                        case E_QuestStatus.InProgress:
                            actual = E_QuestStatus.InProgress;
                            if (questList.TryGetValue(questRequirement, out stored) && stored == actual)
                            {
                                conditionPassed++;
                            }
                            break;
                        case E_QuestStatus.NotBeenAdded:
                            actual = E_QuestStatus.NotBeenAdded;
                            if (!questList.TryGetValue(questRequirement, out stored) && stored == actual)
                            {
                                conditionPassed++;
                            }
                            break;
                    }
                }
            }
            if (condition.NeedItem)
            {
                throw new NotImplementedException();
            }
            if (condition.NeedDialoguePerformed)
            {
                throw new NotImplementedException();
            }
            if (conditionPassed == conditionPassedNeed)
            {
                //If existing condition that matches with the Tracker exist and is false, then set to true
                bool thisBool2 = false;
                if (m_ConditionTrackerDictionary.TryGetValue(condition, out thisBool2))
                {
                    m_ConditionTrackerDictionary[condition] = true;
                }
                return true;
            }
            else
                return false;
        }
    }

    #endregion

    #region Private Method
    /* Trigger By: QuestAdded
     * Param Needed: Condition 
     * Add condition to the Condition Tracker
     */
    private void AddCondition()
    {
        foreach (Quest quest in EventManager.eventObjectList)
        {
            Condition condition = quest.QuestCompleteCondition;
            if (!m_ConditionTrackerDictionary.ContainsKey(condition))
            {
                m_ConditionTrackerDictionary.Add(Instantiate(condition), false);
            }
            else
            {
                Debug.LogError("Error: Condition already exist");
            }
        }
    }

    /* Trigger By: QuestCompleted
     * Param Needed: Condition 
     * Remove condtion from the Condition Tracker
     */
    private void RemoveCondition()
    {
        Quest quest = EventManager.eventObject as Quest;
        Condition condition = quest.QuestCompleteCondition;
        bool thisBool = true;
        if (m_ConditionTrackerDictionary.TryGetValue(condition, out thisBool))
        {
            m_ConditionTrackerDictionary.Remove(condition);
        }
        else
        {
            Debug.LogError("Error: Condition does not exist");
        }
    }

    /* Trigger By: DialoguePerformed
     * Triggers: ConditionSubsetFufilled
     * Param Needed: Dialogue 
     * If dialogue is performed, then remove it from the condition in condition tracker
     */
    private void CheckDialoguePerformedCondition()
    {
        Dialogue dialogue = EventManager.eventObject as Dialogue;

        var cloneConditionTracker = m_ConditionTrackerDictionary.ToDictionary(entry => entry.Key,
                                               entry => entry.Value);

        foreach (Condition condition in cloneConditionTracker.Keys)
        {
            ///Remove Dialogue from condition
            if (condition.NeedDialoguePerformed == true && condition.DialoguePerformedList.Contains(dialogue))
            {
                condition.DialoguePerformedList.Remove(dialogue);
                EventManager.TriggerEvent("ConditionSubsetFufilled", condition);
            }
        }
    }

    private void UpdateConditionTrackerKey(Condition condition)
    {
        

    }

    private void CheckQuestItemDroppedCondition()
    {
        throw new NotImplementedException();
    }

    private void CheckQuestItemObtainedCondition()
    {
        throw new NotImplementedException();
    }

    private void CheckQuestCompletedCondition()
    {
        throw new NotImplementedException();
    }

    private void CheckQuestAddedCondition()
    {
        throw new NotImplementedException();
    }

    /* Trigger By: ConditionSubsetFufilled
     * Triggers: ConditionComplete
     * Param Needed: Condition 
     * Check if Condition in condition tracker is satsified
     */
    private void CheckConditionComplete()
    {
        Condition condition = EventManager.eventObject as Condition;

        Debug.Log("Test");
        bool thisBool = false;
        if (m_ConditionTrackerDictionary.TryGetValue(condition, out thisBool))
        {
            //If all condition categories are zero, then condition is satsified
            if (condition.DialoguePerformedList.Count == 0 && condition.ItemDictionary.Count == 0 && condition.QuestDictionary.Count == 0)
            {

                m_ConditionTrackerDictionary[condition] = true;
                EventManager.TriggerEvent("ConditionComplete");
            }
        }
        else
        {
            Debug.LogError("Error: Condition does not exist in dictionary or is already true");
        }
    }
    #endregion

  

    

    

    
}
