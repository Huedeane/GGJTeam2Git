using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ObjectComponent : MonoBehaviour, IComponentObject
{

    #region Variable
    [SerializeField] private E_ObjectType m_ObjectType;
    [SerializeField] private NPC m_NPCObject;
    [SerializeField] private Item m_ItemObject;
    #endregion

    #region Getter & Setter
    public E_ObjectType ObjectType
    {
        get
        {
            return m_ObjectType;
        }

        set
        {
            m_ObjectType = value;
        }
    }
    public NPC NPCObject
    {
        get
        {
            return m_NPCObject;
        }

        set
        {
            m_NPCObject = value;
        }
    }
    public Item ItemObject
    {
        get
        {
            return m_ItemObject;
        }

        set
        {
            m_ItemObject = value;
        }
    }
    #endregion

    public void Start()
    {
        switch (m_ObjectType)
        {
            case E_ObjectType.NPC:
                m_NPCObject = Instantiate(m_NPCObject);
                break;
            case E_ObjectType.Item:
                m_ItemObject = Instantiate(m_ItemObject);
                break;
        }
    }

    public IInteractableObject GetObjectType()
    {
        switch (m_ObjectType)
        {
            case E_ObjectType.NPC:
                return m_NPCObject;
            case E_ObjectType.Item:
                return m_ItemObject;
            default:
                return null;
        }

    }

    
}

[CustomEditor(typeof(ObjectComponent), true)]
public class ComponentObjectScriptEditor : Editor
{
    SerializedProperty m_NPCObjectProp;
    SerializedProperty m_ItemObjectProp;
    string[] propertiesInBaseClass = new string[] {
        "m_ObjectType", "ComponentObject",
        "m_NPCObject", "ComponentObject",
        "m_ItemObject", "ComponentObject" ,};

    private void OnEnable()
    {

        m_NPCObjectProp = serializedObject.FindProperty("m_NPCObject");
        m_ItemObjectProp = serializedObject.FindProperty("m_ItemObject");
    }

    public override void OnInspectorGUI()
    {
        DrawPropertiesExcluding(serializedObject, propertiesInBaseClass);
        serializedObject.Update();

        var componentObject = target as ObjectComponent;

        EditorGUILayout.LabelField("What type of object is this?", EditorStyles.boldLabel);
        componentObject.ObjectType = (E_ObjectType)EditorGUILayout.EnumPopup("Object Type", componentObject.ObjectType);


        switch (componentObject.ObjectType)
        {
            case E_ObjectType.NPC:
                EditorGUILayout.PropertyField(m_NPCObjectProp, new GUIContent("NPC"), true);
                break;
            case E_ObjectType.Item:
                EditorGUILayout.PropertyField(m_ItemObjectProp, new GUIContent("Item"), true);
                break;

        }


        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
        }
        serializedObject.ApplyModifiedProperties();
    }

}
