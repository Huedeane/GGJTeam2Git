using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[Serializable] public class QuestE_QuestStatusDictionary : SerializableDictionary<Quest, E_QuestStatus> { }
[Serializable] public class ItemE_ItemStatusDictionary : SerializableDictionary<Item, E_ItemStatus> { }

/* Enumerator Explanation
 * 
 * E_QuestStatus
 *  - Status of the Quest
 *  - Satsified is when the quest complete condition is satsified
 *  but is currently waiting to be completed
 *  
 * E_ItemStatus
 *  - Status of if a certain item has been obtained
 *  
 */

public enum E_QuestStatus { NotBeenAdded, InProgress, Satsified, Completed}
public enum E_ItemStatus { NotObtained, Obtained}


/* Class Explanation
 * - Condition that can be created that gates certain object from being accessabile or interacted upon
 * - Some Condition are need certain quest, item, or dialogue performed
 */
[CreateAssetMenu(menuName = "Condition", order = 2)]
public class Condition : ScriptableObject, IEquatable<Condition>
{

    private static readonly int hashCode = 10428263;

    #region Variable
    [SerializeField] private int m_ConditionId;
    [SerializeField] [TextArea(3, 10)] private string m_ConditionDescription;
    [SerializeField] private bool m_NeedQuest;
    [SerializeField] private QuestE_QuestStatusDictionary m_QuestDictionary;
    [SerializeField] private bool m_NeedItem;
    [SerializeField] private ItemE_ItemStatusDictionary m_ItemDictionary;
    [SerializeField] private bool m_NeedDialoguePerformed;
    [SerializeField] private List<Dialogue> m_DialoguePerformedList;
    #endregion

    #region Getter & Setter
    public int ConditionId
    {
        get
        {
            return m_ConditionId;
        }

        set
        {
            m_ConditionId = value;
        }
    }
    public string ConditionDescription
    {
        get
        {
            return m_ConditionDescription;
        }

        set
        {
            m_ConditionDescription = value;
        }
    }
    public bool NeedQuest
    {
        get
        {
            return m_NeedQuest;
        }

        set
        {
            m_NeedQuest = value;
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
    public bool NeedItem
    {
        get
        {
            return m_NeedItem;
        }

        set
        {
            m_NeedItem = value;
        }
    }
    public ItemE_ItemStatusDictionary ItemDictionary
    {
        get
        {
            return m_ItemDictionary;
        }

        set
        {
            m_ItemDictionary = value;
        }
    }
    public bool NeedDialoguePerformed
    {
        get
        {
            return m_NeedDialoguePerformed;
        }

        set
        {
            m_NeedDialoguePerformed = value;
        }
    }
    public List<Dialogue> DialoguePerformedList
    {
        get
        {
            return m_DialoguePerformedList;
        }

        set
        {
            m_DialoguePerformedList = value;
        }
    }
    #endregion

    public bool Equals(Condition obj)
    {
        return (obj is Condition) && ((Condition)obj).m_ConditionId == m_ConditionId;
    }

    public override int GetHashCode()
    {
        return hashCode * m_ConditionId;
    }
}

//Custom Editor for Condition
[CustomEditor(typeof(Condition), true)]
public class ConditionScriptEditor : Editor
{

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        var condition = target as Condition;

        SerializedProperty prop = serializedObject.FindProperty("m_Script");
        EditorGUILayout.PropertyField(prop, true, new GUILayoutOption[0]);

        //-------Condition Identifier
        CustomEditorResource.DrawUILine(Color.gray);
        EditorGUILayout.LabelField("Condition Idenitifer", EditorStyles.boldLabel);
        EditorGUILayout.Space();
        EditorGUILayout.PropertyField(serializedObject.FindProperty("m_ConditionId"), true);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("m_ConditionDescription"), true);

        //-------Condition Data
        CustomEditorResource.DrawUILine(Color.gray);
        EditorGUILayout.LabelField("Condition Data", EditorStyles.boldLabel);
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Condition where certain Quest need to be complete/uncomplete", EditorStyles.miniBoldLabel);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("m_NeedQuest"), true);
        if (condition.NeedQuest == true)
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("m_QuestDictionary"), true);
        }
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Condition where certain Item need to be obtained/unobtained", EditorStyles.miniBoldLabel);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("m_NeedItem"), true);
        if (condition.NeedItem == true) {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("m_ItemDictionary"), true);
        }
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Condition where certain Dialogue need to be fufilled", EditorStyles.miniBoldLabel);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("m_NeedDialoguePerformed"), true);
        if (condition.NeedDialoguePerformed == true)
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("m_DialoguePerformedList"), true);
        }
        CustomEditorResource.DrawUILine(Color.gray);

        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
        }
        serializedObject.ApplyModifiedProperties();
    }

    

}
