using UnityEngine;

/* Class Explanation
 * - Data to populate the InfoPopUpManager which display at the top right
 * that promopt special info for certain situation.
 */
public class Info : ScriptableObject
{
    #region Variable
    private string m_InfoContext;
    private string m_InfoText;
    private float m_InfoTime;

    public Info(string m_InfoContext, string m_InfoText, float m_InfoTime)
    {
        this.m_InfoContext = m_InfoContext;
        this.m_InfoText = m_InfoText;
        this.m_InfoTime = m_InfoTime;
    }
    #endregion

    #region Getter & Setter
    public string InfoContext
    {
        get
        {
            return m_InfoContext;
        }

        set
        {
            m_InfoContext = value;
        }
    }
    public string InfoText
    {
        get
        {
            return m_InfoText;
        }

        set
        {
            m_InfoText = value;
        }
    }
    public float InfoTime
    {
        get
        {
            return m_InfoTime;
        }

        set
        {
            m_InfoTime = value;
        }
    }
    #endregion
}
