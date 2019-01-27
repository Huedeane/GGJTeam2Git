using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Class Explanation
 * - Data for reward
 */
public class Reward : ScriptableObject {

    #region Variable
    private bool m_GiveExperience;
    private int m_ExperienceValue;
    private bool m_GiveMoney;
    private int m_MoneyValue;
    private bool m_GiveItem;
    private List<Item> m_ItemList;
    #endregion

    #region Getter & Setter 
    public bool GiveExperience
    {
        get
        {
            return m_GiveExperience;
        }

        set
        {
            m_GiveExperience = value;
        }
    }
    public int ExperienceValue
    {
        get
        {
            return m_ExperienceValue;
        }

        set
        {
            m_ExperienceValue = value;
        }
    }
    public bool GiveMoney
    {
        get
        {
            return m_GiveMoney;
        }

        set
        {
            m_GiveMoney = value;
        }
    }
    public int MoneyValue
    {
        get
        {
            return m_MoneyValue;
        }

        set
        {
            m_MoneyValue = value;
        }
    }
    public bool GiveItem
    {
        get
        {
            return m_GiveItem;
        }

        set
        {
            m_GiveItem = value;
        }
    }
    public List<Item> ItemList
    {
        get
        {
            return m_ItemList;
        }

        set
        {
            m_ItemList = value;
        }
    }
    #endregion
}
