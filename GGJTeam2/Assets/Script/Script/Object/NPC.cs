using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/* Class Explanation
 * - Data for NPC and how it handles interaction
 */
[CreateAssetMenu(menuName = "NPC", order = 4)]
public class NPC : ScriptableObject, IInteractableObject
{
    #region Variable
    [SerializeField] private int m_NPCId;       
    [SerializeField] private string m_NPCName;
    [SerializeField] [TextArea(3, 10)] private string m_NPCPrefaceText;
    [SerializeField] private List<Dialogue> m_DialogueList;

    //IInteractableObject
    [SerializeField] private bool m_IsInteractable;
    [SerializeField] private string m_InteractionText;
    #endregion

    #region Getter & Setter
    public int NPCId
    {
        get
        {
            return m_NPCId;
        }

        set
        {
            m_NPCId = value;
        }
    }
    public string NPCName
    {
        get
        {
            return m_NPCName;
        }

        set
        {
            m_NPCName = value;
        }
    }
    public string NPCPrefaceText
    {
        get
        {
            return m_NPCPrefaceText;
        }

        set
        {
            m_NPCPrefaceText = value;
        }
    }
    public List<Dialogue> DialogueList
    {
        get
        {
            return m_DialogueList;
        }

        set
        {
            m_DialogueList = value;
        }
    }

    //IInteractableObject
    public bool IsInteractable
    {
        get
        {
            return m_IsInteractable;
        }

        set
        {
            m_IsInteractable = value;
        }
    }
    public string InteractionText
    {
        get
        {
            return m_InteractionText;
        }

        set
        {
            m_InteractionText = value;
        }
    }    
    #endregion

    public void Awake()
    {
        if (m_NPCName.Trim().Equals(""))
        {
            m_NPCName = "Null";
        }
        SetupInteractable();
    }

    //IInteractableObject
    public void SetupInteractable() {
        /* Set default interaction text if none is present
         */
        if (m_InteractionText == null || m_InteractionText.Trim().Equals(""))
        {
            if (NPCName.Trim().Equals(""))
            {
                m_InteractionText = "Press E to Talk";
            }
            else
            {
                m_InteractionText = "Press E to Talk to " + NPCName;
            }
        }
    }

    //IInteractableObject
    public IEnumerator ExecuteInteraction()
    {
        if (GameManager.Instance.uiAccess == true)
        {
            GameManager.Instance.interactionText.gameObject.SetActive(false);
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().CanMove = false;
            /* - NPC is set to not be interactable
             * - Freezes the user camera and makes cursor invisible
             */
            m_IsInteractable = false;

            // Set the ui access to false and starts conversation
            GameManager.Instance.uiAccess = false;
            
            EventManager.TriggerEvent("StartConversation", this);

            // Wait till the conversation is done
            yield return new WaitUntil(() => DialogueManager.Instance.ConversationInProgress == false);

            // Restore NPC interactable, cursor, and UI Access
            GameManager.Instance.uiAccess = true;
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().CanMove = true;
            m_IsInteractable = true;
            GameManager.Instance.interactionText.gameObject.SetActive(true);
        }
        else {
            Debug.LogError("Error: UI cannot be accessed at this time");
        }       
        yield return null;
    }

    public bool Equals(NPC obj)
    {
        return (obj is NPC) && ((NPC)obj).m_NPCId == m_NPCId;
    }

    public override int GetHashCode()
    {
        return base.GetHashCode() * m_NPCId;
    }
}

//Custom Editor for NPC
[CustomEditor(typeof(NPC), true)]
public class NPCScriptEditor : Editor
{

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        var npc = target as NPC;

        SerializedProperty prop = serializedObject.FindProperty("m_Script");
        EditorGUILayout.PropertyField(prop, true, new GUILayoutOption[0]);

        //-------NPC Identifier
        CustomEditorResource.DrawUILine(Color.gray);
        EditorGUILayout.LabelField("NPC Idenitifer", EditorStyles.boldLabel);
        EditorGUILayout.Space();
        EditorGUILayout.PropertyField(serializedObject.FindProperty("m_NPCId"), true);

        //-------NPC Data
        CustomEditorResource.DrawUILine(Color.gray);
        EditorGUILayout.LabelField("NPC Data", EditorStyles.boldLabel);
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Name of the NPC", EditorStyles.miniBoldLabel);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("m_NPCName"), true);
        EditorGUILayout.LabelField("Text That appears before you talk to the NPC", EditorStyles.miniBoldLabel);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("m_NPCPrefaceText"), true);
        EditorGUILayout.LabelField("Dialogue of the NPC and what the Dialogue is used for", EditorStyles.miniBoldLabel);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("m_DialogueList"), true);
        EditorGUILayout.LabelField("Can the Player interact with the NPC?", EditorStyles.miniBoldLabel);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("m_IsInteractable"), true);
        EditorGUILayout.LabelField("Text that appears when player hover over the NPC", EditorStyles.miniBoldLabel);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("m_InteractionText"), true);

        CustomEditorResource.DrawUILine(Color.gray);

        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
        }
        serializedObject.ApplyModifiedProperties();
    }

}
