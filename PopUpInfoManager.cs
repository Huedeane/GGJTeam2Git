using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PopUpInfoManager : MonoBehaviour
{
    #region Variable
    [SerializeField] private GameObject m_UI_PopUpInfo;
    [SerializeField] private GameObject m_InfoContext;
    [SerializeField] private GameObject m_InfoText;
    [SerializeField] private bool m_IsInfoUIActive;
    [SerializeField] private List<Info> m_InfoQueueList;
    #endregion

    #region Getter & Setter
    public GameObject UI_PopUpInfo
    {
        get
        {
            return m_UI_PopUpInfo;
        }

        set
        {
            m_UI_PopUpInfo = value;
        }
    }
    public GameObject InfoContext
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
    public GameObject InfoText
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
    public bool IsInfoUIActive
    {
        get
        {
            return m_IsInfoUIActive;
        }

        set
        {
            m_IsInfoUIActive = value;
        }
    }
    public List<Info> InfoQueueList
    {
        get
        {
            return m_InfoQueueList;
        }

        set
        {
            m_InfoQueueList = value;
        }
    }
    #endregion

    public void Awake()
    {
        if (m_UI_PopUpInfo == null)
        {
            Debug.LogError("Must Assign a PopUpInfo UI to the PopUpInfoManager!");
        }
        SetupInfoManager();
    }

    public IEnumerator ActivePopUpDialog()
    {
        if (m_IsInfoUIActive == false)//no info pop up atm
        {
            m_IsInfoUIActive = true;
            m_UI_PopUpInfo.SetActive(true);
            m_InfoContext.SetActive(true);
            m_InfoText.SetActive(true);
            while (m_InfoQueueList != null && m_InfoQueueList.Count != 0)
            {
                m_InfoText.GetComponent<TextMeshProUGUI>().SetText(m_InfoQueueList[0].InfoText);
                m_InfoContext.GetComponent<TextMeshProUGUI>().SetText(m_InfoQueueList[0].InfoContext);
                yield return new WaitForSeconds(m_InfoQueueList[0].InfoTime);
                m_InfoQueueList.RemoveAt(0);
            }
            m_IsInfoUIActive = false;
            m_UI_PopUpInfo.SetActive(false);
        } 
    }

    public void SetupInfoManager()
    {
        m_IsInfoUIActive = false;
        m_InfoQueueList = new List<Info>();
    }

    public void AddInfo(Info info)
    {
        m_InfoQueueList.Add(info);
    }
}
