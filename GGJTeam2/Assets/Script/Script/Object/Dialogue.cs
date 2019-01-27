using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/* Enumerator Explanation
 * 
 * E_DialogueIdentifier
 *  - Identify where the Dialogue is in the chain
 *  
 * E_DialogueUsage
 *  - What is the main purpose of that dialogue
 *  
 * E_DialogueType
 *  - Identify what type of dialogue it is and what it's used for
 *  - Chat
 *      - Dialogue options that user can choose
 *  - Response
 *      - NPC Response to the User
 * 
 * E_DialogueConnectionType
 *  - Connection to the next the Dialogue if any and what type of dialogue it is
 *  - If none, then the dialogue conversation ends
 *  
 * E_DialogueSpecialAction
 *  - Special Action that happen during or after current Dialogue is finished
 * 
 */

public enum E_DialogueIdentifier { Start, StartAndEnd, Middle, End };
public enum E_DialogueUsage { Conversation, AddQuest, InProgressQuest, CompleteQuest }
public enum E_DialogueType { Chat, Response };
public enum E_DialogueConnectionType { Chat, Response, None };
public enum E_DialogueSpecialAction { None, AddQuest, CompleteQuest };

/* Class Explanation
 * - Dialogue that can be created for which is used for interacting with the npc
 */
[CreateAssetMenu(menuName = "Dialogue", order = 1)]
public class Dialogue : ScriptableObject, IEquatable<Dialogue>
{

    private static readonly int hashCode = 80443852;

    #region Variable
    [SerializeField] private int m_DialogueGroupID;
    [SerializeField] private int m_DialogueID;
    [SerializeField] private Condition m_DialogueCondition;
    [SerializeField] private E_DialogueIdentifier m_DialgoueIdentifier;
    [SerializeField] private E_DialogueUsage m_DialogueUsage;
    [SerializeField] private E_DialogueType m_DialogueType;
    [SerializeField] [TextArea(3, 10)] private string m_DialogueText;
    [SerializeField] private E_DialogueConnectionType m_DialogueConnectionType;
    [SerializeField] private List<Dialogue> m_DialogueChatList;
    [SerializeField] private Dialogue m_DialogueResponse;
    [SerializeField] private E_DialogueSpecialAction m_DialogueSpecialAction;
    [SerializeField] private Quest m_AddQuest;
    [SerializeField] private Quest m_SetQuestComplete;
    [SerializeField] private bool m_HasInfoPopUp;
    [SerializeField] private List<Info> m_InfoPopUpList;
    #endregion

    #region Getter & Setter
    public int DialogueGroupID
    {
        get
        {
            return m_DialogueGroupID;
        }

        set
        {
            m_DialogueGroupID = value;
        }
    }
    public int DialogueID
    {
        get
        {
            return m_DialogueID;
        }

        set
        {
            m_DialogueID = value;
        }
    }
    public Condition DialogueCondition
    {
        get
        {
            return m_DialogueCondition;
        }

        set
        {
            m_DialogueCondition = value;
        }
    }
    public E_DialogueIdentifier DialgoueIdentifier
    {
        get
        {
            return m_DialgoueIdentifier;
        }

        set
        {
            m_DialgoueIdentifier = value;
        }
    }
    public E_DialogueUsage DialogueUsage
    {
        get
        {
            return m_DialogueUsage;
        }

        set
        {
            m_DialogueUsage = value;
        }
    }
    public E_DialogueType DialogueType
    {
        get
        {
            return m_DialogueType;
        }

        set
        {
            m_DialogueType = value;
        }
    }
    public string DialogueText
    {
        get
        {
            return m_DialogueText;
        }

        set
        {
            m_DialogueText = value;
        }
    }
    public E_DialogueConnectionType DialogueConnectionType
    {
        get
        {
            return m_DialogueConnectionType;
        }

        set
        {
            m_DialogueConnectionType = value;
        }
    }
    public List<Dialogue> DialogueChatList
    {
        get
        {
            return m_DialogueChatList;
        }

        set
        {
            m_DialogueChatList = value;
        }
    }
    public Dialogue DialogueResponse
    {
        get
        {
            return m_DialogueResponse;
        }

        set
        {
            m_DialogueResponse = value;
        }
    }
    public E_DialogueSpecialAction DialogueSpecialAction
    {
        get
        {
            return m_DialogueSpecialAction;
        }

        set
        {
            m_DialogueSpecialAction = value;
        }
    }
    public Quest AddQuest
    {
        get
        {
            return m_AddQuest;
        }

        set
        {
            m_AddQuest = value;
        }
    }
    public Quest SetQuestComplete
    {
        get
        {
            return m_SetQuestComplete;
        }

        set
        {
            m_SetQuestComplete = value;
        }
    }
    public bool HasInfoPopUp
    {
        get
        {
            return m_HasInfoPopUp;
        }

        set
        {
            m_HasInfoPopUp = value;
        }
    }
    public List<Info> InfoPopUpList
    {
        get
        {
            return m_InfoPopUpList;
        }

        set
        {
            m_InfoPopUpList = value;
        }
    }
    #endregion

    public bool Equals(Dialogue obj)
    {
        return (obj is Dialogue) && ((Dialogue)obj).m_DialogueID == m_DialogueID;
    }

    public override int GetHashCode()
    {
        return hashCode * m_DialogueID;
    }
}

//Custom Editor for Dialogue
[CustomEditor(typeof(Dialogue), true)]
public class DialogueScriptEditor : Editor
{

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        var dialogue = target as Dialogue;

        SerializedProperty prop = serializedObject.FindProperty("m_Script");
        EditorGUILayout.PropertyField(prop, true, new GUILayoutOption[0]);

        //Dialogue Idenitifer
        CustomEditorResource.DrawUILine(Color.gray);
        EditorGUILayout.LabelField("Dialogue Idenitifer", EditorStyles.boldLabel);
        EditorGUILayout.Space();
        EditorGUILayout.PropertyField(serializedObject.FindProperty("m_DialogueGroupID"), true);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("m_DialogueID"), true);
        EditorGUILayout.LabelField("Identify Where Dialogue is in the Chain", EditorStyles.miniBoldLabel);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("m_DialgoueIdentifier"), true);
        EditorGUILayout.LabelField("Identify what the Dialogue is used for", EditorStyles.miniBoldLabel);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("m_DialogueUsage"), true);

        //Dialogue Data
        CustomEditorResource.DrawUILine(Color.gray);
        EditorGUILayout.LabelField("Dialogue Data", EditorStyles.boldLabel);
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Condition for if the Dialogue will be shown", EditorStyles.miniBoldLabel);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("m_DialogueCondition"), true);
        EditorGUILayout.LabelField("Identify whether Dialogue is a Chat Option or NPC Response", EditorStyles.miniBoldLabel);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("m_DialogueType"), true);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("m_DialogueText"), true);
        dialogue.DialogueConnectionType = (E_DialogueConnectionType)EditorGUILayout.EnumPopup("Dialogue Connection Type", dialogue.DialogueConnectionType);
        switch (dialogue.DialogueConnectionType)
        {
            case E_DialogueConnectionType.Chat:
                EditorGUILayout.PropertyField(serializedObject.FindProperty("m_DialogueChatList"), true);
                break;
            case E_DialogueConnectionType.Response:
                EditorGUILayout.PropertyField(serializedObject.FindProperty("m_DialogueResponse"), true);
                break;
        }

        //Dialogue  Special
        CustomEditorResource.DrawUILine(Color.gray);
        EditorGUILayout.LabelField("Dialogue Special", EditorStyles.boldLabel);
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Condition for if the Dialogue will be shown", EditorStyles.miniBoldLabel);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("m_HasInfoPopUp"), true);
        if (dialogue.HasInfoPopUp == true)
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("m_InfoPopUpList"), true);
        }
        EditorGUILayout.LabelField("Special action that activates at end of Dialogue", EditorStyles.miniBoldLabel);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("m_DialogueSpecialAction"), true);
        switch (dialogue.DialogueSpecialAction)
        {
            case E_DialogueSpecialAction.None:
                break;
            case E_DialogueSpecialAction.AddQuest:
                EditorGUILayout.PropertyField(serializedObject.FindProperty("m_AddQuest"), true);
                break;
            case E_DialogueSpecialAction.CompleteQuest:
                EditorGUILayout.PropertyField(serializedObject.FindProperty("m_SetQuestComplete"), true);
                break;

        }
        CustomEditorResource.DrawUILine(Color.gray);

        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
        }
        serializedObject.ApplyModifiedProperties();
    }

}


