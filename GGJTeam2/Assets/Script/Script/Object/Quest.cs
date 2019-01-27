using System;
using UnityEditor;
using UnityEngine;

/* Enumerator Explanation
 * E_AddedThrough
 *  - Where is the quest going to be added through
 *  - If it's not going to be added through dialogue, then it needs a precondition
 * 
 * E_CompletedThrough
 *  - After a quest is satsified, how is it going to be completed
 */
public enum E_AddedThrough { Dialogue, Other }
public enum E_CompletedThrough { Dialogue, Itself }

/* Class Explanation
 * - Data for Quest, how it's going to be completed, and what's the reward
 */
[CreateAssetMenu(menuName = "Quest", order = 3)]
public class Quest : ScriptableObject, IEquatable<Quest>
{

    private static readonly int hashCode = 95598880;

    #region Variable    
    [SerializeField] private int m_QuestID;
    [SerializeField] [TextArea(3, 10)] private string m_QuestDescription;
    [SerializeField] private E_AddedThrough m_AddedThrough;
    [SerializeField] private Condition m_QuestPreCondition;
    [SerializeField] private Dialogue m_AddedDialogueReference;
    [SerializeField] private E_CompletedThrough m_CompletedThrough;
    [SerializeField] private Condition m_QuestCompleteCondition;
    [SerializeField] private Dialogue m_CompleteDialogueReference;
    [SerializeField] private Reward m_QuestReward;
    #endregion

    #region Getter & Setter 
    public int QuestID
    {
        get
        {
            return m_QuestID;
        }

        set
        {
            m_QuestID = value;
        }
    }
    public string QuestDescription
    {
        get
        {
            return m_QuestDescription;
        }

        set
        {
            m_QuestDescription = value;
        }
    }
    public E_AddedThrough AddedThrough
    {
        get
        {
            return m_AddedThrough;
        }

        set
        {
            m_AddedThrough = value;
        }
    }
    public Condition QuestPreCondition
    {
        get
        {
            return m_QuestPreCondition;
        }

        set
        {
            m_QuestPreCondition = value;
        }
    }
    public Dialogue AddedDialogueReference
    {
        get
        {
            return m_AddedDialogueReference;
        }

        set
        {
            m_AddedDialogueReference = value;
        }
    }
    public E_CompletedThrough CompletedThrough
    {
        get
        {
            return m_CompletedThrough;
        }

        set
        {
            m_CompletedThrough = value;
        }
    }
    public Condition QuestCompleteCondition
    {
        get
        {
            return m_QuestCompleteCondition;
        }

        set
        {
            m_QuestCompleteCondition = value;
        }
    }
    public Dialogue CompleteDialogueReference
    {
        get
        {
            return m_CompleteDialogueReference;
        }

        set
        {
            m_CompleteDialogueReference = value;
        }
    }
    public Reward QuestReward
    {
        get
        {
            return m_QuestReward;
        }

        set
        {
            m_QuestReward = value;
        }
    }

   
    #endregion

    public bool Equals(Quest obj)
    {
        return (obj is Quest) && ((Quest)obj).QuestID == QuestID;
    }

    public override int GetHashCode()
    {
        return hashCode * m_QuestID;
    }
}

[CustomEditor(typeof(Quest), true)]
public class QuestScriptEditor : Editor
{

    public override void OnInspectorGUI()
    {
        serializedObject.Update();        
        var quest = target as Quest;

        SerializedProperty prop = serializedObject.FindProperty("m_Script");
        EditorGUILayout.PropertyField(prop, true, new GUILayoutOption[0]);

        //Quest Idenitifer
        DrawUILine(Color.gray);
        EditorGUILayout.LabelField("Quest Idenitifer", EditorStyles.boldLabel);
        EditorGUILayout.Space();
        EditorGUILayout.PropertyField(serializedObject.FindProperty("m_QuestID"), true);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("m_QuestDescription"), true);

        //Dialogue Data
        DrawUILine(Color.gray);
        EditorGUILayout.LabelField("Quest Data", EditorStyles.boldLabel);
        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Where Quest going to be added?", EditorStyles.miniBoldLabel);
        quest.AddedThrough = (E_AddedThrough)EditorGUILayout.EnumPopup("Added Through", quest.AddedThrough);
        
        switch (quest.AddedThrough) {
            case E_AddedThrough.Dialogue:
                EditorGUILayout.PropertyField(serializedObject.FindProperty("m_AddedDialogueReference"), true);
                break;
            case E_AddedThrough.Other:
                EditorGUILayout.PropertyField(serializedObject.FindProperty("m_QuestPreCondition"), true);
                break;

        }

        EditorGUILayout.LabelField("What is need for the quest to be satsified?", EditorStyles.miniBoldLabel);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("m_QuestCompleteCondition"), true);
        EditorGUILayout.LabelField("After a quest is satsified, how is it going to be completed?", EditorStyles.miniBoldLabel);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("m_CompletedThrough"), true);
        switch (quest.CompletedThrough)
        {
            case E_CompletedThrough.Dialogue:
                EditorGUILayout.PropertyField(serializedObject.FindProperty("m_AddedDialogueReference"), true);
                break;
            case E_CompletedThrough.Itself:
                break;
        }
       

        EditorGUILayout.LabelField("Reward for Completion of Quest", EditorStyles.miniBoldLabel);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("m_QuestReward"),  true);
        DrawUILine(Color.gray);


        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
        }
        serializedObject.ApplyModifiedProperties();
    }

    public void DrawUILine(Color color, int thickness = 2, int padding = 10)
    {
        Rect r = EditorGUILayout.GetControlRect(GUILayout.Height(padding + thickness));
        r.height = thickness;
        r.y += padding / 2;
        r.x -= 2;
        r.width += 6;
        EditorGUI.DrawRect(r, color);
    }

}
