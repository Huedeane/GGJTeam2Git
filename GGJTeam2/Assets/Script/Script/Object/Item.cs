using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item", order = 3)]
public class Item : ScriptableObject, IInteractableObject
{
    private static readonly int hashCode = 95598881;

    #region Variable
    [SerializeField] private int m_ItemID;
    [SerializeField] private string m_ItemName;
    [SerializeField] private string m_InteractionText;
    [SerializeField] private bool m_IsInteractable;
    #endregion

    #region Getter & Setter 
    public int ItemID { get => m_ItemID; set => m_ItemID = value; }
    public string ItemName { get => m_ItemName; set => m_ItemName = value; }
    public string InteractionText { get => m_InteractionText; set => m_InteractionText = value; }
    public bool IsInteractable { get => m_IsInteractable; set => m_IsInteractable = value; }
    #endregion

    public bool Equals(Item obj)
    {
        return (obj is Item) && ((Item)obj).ItemID == ItemID;
    }

    public override int GetHashCode()
    {
        return hashCode * m_ItemID;
    }

    public void Awake()
    {
        SetupInteractable();
    }

    public void SetupInteractable()
    {
        if (m_InteractionText == null || m_InteractionText.Trim().Equals(""))
        {
            m_InteractionText = "Press Z to pick up " + m_ItemName;
        }
    }

    public IEnumerator ExecuteInteraction()
    {
        yield return null;
    }
}
