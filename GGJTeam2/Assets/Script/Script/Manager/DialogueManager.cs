using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/* Class Explanation
 * - Manager for executing dialogue
 */
public class DialogueManager : MonoBehaviour
{
    #region Variable
    [SerializeField] private static DialogueManager dialogueManager;
    [SerializeField] private GameObject m_DialogueButtonPrefab;
    [SerializeField] private GameObject m_DialogueUI;
    [SerializeField] private GameObject m_DialogueChatList;
    [SerializeField] private GameObject m_DialogueResponse;
    [SerializeField] private NPC m_CurrentNPCFocus;
    [SerializeField] private bool m_ConversationInProgress;
    [SerializeField] private List<Dialogue> m_DialogueList;
    [SerializeField] private Dialogue m_DialogueFocus;
    #endregion

    #region Getter & Setter
    public static DialogueManager Instance
    {
        get
        {
            if (!dialogueManager)
            {
                dialogueManager = FindObjectOfType(typeof(DialogueManager)) as DialogueManager;
                if (!dialogueManager)
                {
                    Debug.LogError("Error: There is no active dialogueManager attached to a gameObject");
                }
                else
                {
                    dialogueManager.Init();
                }
            }

            return dialogueManager;
        }
        set
        {
            dialogueManager = value;
        }
    }
    public GameObject DialogueUI
    {
        get
        {
            return m_DialogueUI;
        }

        set
        {
            m_DialogueUI = value;
        }
    }
    public GameObject DialogueChatList
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
    public GameObject DialogueResponse
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
    public GameObject DialogueButtonPrefab
    {
        get
        {
            return m_DialogueButtonPrefab;
        }

        set
        {
            m_DialogueButtonPrefab = value;
        }
    } 
    public NPC CurrentNPCFocus
    {
        get
        {
            return m_CurrentNPCFocus;
        }

        set
        {
            m_CurrentNPCFocus = value;
        }
    }
    public bool ConversationInProgress
    {
        get
        {
            return m_ConversationInProgress;
        }

        set
        {
            m_ConversationInProgress = value;
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
    public Dialogue DialogueFocus
    {
        get
        {
            return m_DialogueFocus;
        }

        set
        {
            m_DialogueFocus = value;
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
        if (m_DialogueButtonPrefab == null || m_DialogueUI == null || m_DialogueChatList == null || m_DialogueResponse == null)
        {
            Debug.LogError("Not all Dialogue UI has been assigned to the DialogueManager");
        }
        if (m_DialogueList == null)
        {
            m_DialogueList = new List<Dialogue>();
        }
        m_ConversationInProgress = false;
        m_CurrentNPCFocus = null;
        m_DialogueFocus = null;
    }

    void OnEnable()
    {
        EventManager.StartListening("StartConversation", StartCoversation);
        EventManager.StartListening("EndConversation", EndConversation);
    }

    void OnDisable()
    {
        EventManager.StopListening("StartConversation", StartCoversation);
        EventManager.StopListening("EndConversation", EndConversation);
    }
    #endregion

    #region Private Method
    /* Trigger By: StartCoversation
     * Param Needed: NPC 
     * Start a conversation
     */
    private void StartCoversation()
    {
        //Get NPC from event list and set it as the current NPC
        m_CurrentNPCFocus = EventManager.eventObjectList[0] as NPC;

        
        //Set Dialogue and Conversation to true
        m_ConversationInProgress = true;
        m_DialogueUI.SetActive(true);

        //Set Dialogue Choice from NPC Dialogue List
        m_DialogueList = new List<Dialogue>(CurrentNPCFocus.DialogueList);
        
        //If NPC has a preface text, then execute it before dialogue choice appears
        if (!m_CurrentNPCFocus.NPCPrefaceText.Trim().Equals(""))
        {
            StartCoroutine("PrefaceText");
        }
        else
        {
            SetupDialogueOption();
            StartCoroutine("AwaitDialogueChoice");
        }
    }

    //Execute Dialogue that appears when you first talk to a NPC before choice
    private IEnumerator PrefaceText()
    {
        //Set Dialogue Response True
        m_DialogueChatList.SetActive(false);
        m_DialogueResponse.SetActive(true);

        //Set current Dialogue Response Text to the NPC Preface Text
        m_DialogueResponse.GetComponent<TextMeshProUGUI>().SetText(CurrentNPCFocus.NPCPrefaceText);

        //Wait for User to left click before preceding
        bool done = false;
        while (!done)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                done = true;
            }
            yield return null;
        }

        //Set up the Dailogute Option, then wait for user decision
        SetupDialogueOption();
        StartCoroutine("AwaitDialogueChoice");
    }

    private void SetupDialogueOption()
    {
        
        ResetButton();
        foreach (Dialogue dialogue in m_DialogueList)
        {
            if (dialogue.DialogueCondition == null || this.GetComponent<ConditionManager>().CheckCondition(dialogue.DialogueCondition))
            {
                GameObject newButton = Instantiate(m_DialogueButtonPrefab, transform.position, transform.rotation) as GameObject;
                newButton.transform.SetParent(m_DialogueChatList.transform, false);
                Debug.Log(dialogue.DialogueText);
                newButton.GetComponentInChildren<TextMeshProUGUI>().SetText(dialogue.DialogueText);

                Button button = newButton.GetComponent<Button>();

                switch (dialogue.DialogueConnectionType)
                {
                    case E_DialogueConnectionType.Chat:
                        //Set Chat option and execute any special dialogue action
                        button.onClick.AddListener(
                            delegate
                            {
                                m_DialogueFocus = dialogue;
                                ExecuteSpecialDialogueAction(dialogue);

                                EventManager.TriggerEvent("DialoguePerformed", dialogue);
                            });
                        break;
                        //Set NPC response and execute any special dialogue action
                    case E_DialogueConnectionType.Response:
                        button.onClick.AddListener(
                            delegate
                            {
                                m_DialogueFocus = dialogue.DialogueResponse;
                                ExecuteSpecialDialogueAction(dialogue);

                                EventManager.TriggerEvent("DialoguePerformed" , dialogue);
                            });
                        break;
                        //Trigger end conversation and execute any special dialogue action
                    case E_DialogueConnectionType.None:
                        Debug.Log("Test3");
                        button.onClick.AddListener(
                            () => DoSomething());
                        break;
                    default:
                        Debug.Log("Test2");
                        break;

                }
            }

        }
    }

    public void DoSomething()
    {
        Debug.Log("Work");
    }

    private IEnumerator AwaitDialogueChoice()
    {
        //Set Chat list to visible and wait for user decision
        m_DialogueChatList.SetActive(true);
        m_DialogueResponse.SetActive(false);
        
        yield return new WaitUntil(() => m_DialogueFocus != null);

        //After user picks a dialogue choice, execute it
        StartCoroutine(ExecuteDialogue(m_DialogueFocus));
        m_DialogueFocus = null;
    }

    //Execute Special Dialogue Action if there is any
    private void ExecuteSpecialDialogueAction(Dialogue dialogue)
    {
        switch (dialogue.DialogueSpecialAction)
        {
            case E_DialogueSpecialAction.AddQuest:
                EventManager.TriggerEvent("QuestAdded", dialogue.AddQuest);
                break;
            case E_DialogueSpecialAction.CompleteQuest:
                EventManager.TriggerEvent("QuestCompleted", dialogue.SetQuestComplete);
                break;
        }
    }

    private IEnumerator ExecuteDialogue(Dialogue dialogue)
    {
        switch (dialogue.DialogueType)
        {
            case E_DialogueType.Chat:
                //Set Chat option to visible
                m_DialogueChatList.SetActive(true);
                m_DialogueResponse.SetActive(false);

                //Execute Dialogue Connection Type
                ExecuteDialogueAction(dialogue);
                break;
            case E_DialogueType.Response:
                //Set NPC Response to Visible
                m_DialogueChatList.SetActive(false);
                m_DialogueResponse.SetActive(true);

                //Wait for User to left click to precede
                m_DialogueResponse.GetComponent<TextMeshProUGUI>().SetText(dialogue.DialogueText);
                bool done = false;
                while (!done)
                {
                    if (Input.GetKeyDown(KeyCode.Mouse0))
                    {
                        done = true;
                    }
                    yield return null;
                }

                //Execute Dialogue Connection Type
                ExecuteDialogueAction(dialogue);
                break;
        }
        yield return null;
    }

    public void ExecuteDialogueAction(Dialogue dialogue)
    {
        switch (dialogue.DialogueConnectionType)
        {
            //Set up Dialogue Chat option from current Dialogue and await user decision
            case E_DialogueConnectionType.Chat:
                m_DialogueList = dialogue.DialogueChatList;
                SetupDialogueOption();
                StartCoroutine("AwaitDialogueChoice");
                break;
            //Execute NPC dialogue response
            case E_DialogueConnectionType.Response:
                StartCoroutine(ExecuteDialogue(dialogue.DialogueResponse));
                break;
            //End the Conversation
            case E_DialogueConnectionType.None:
                StartCoroutine("EndConversation");
                break;
        }
    }

    //Delete all the Button from the chat option
    private void ResetButton()
    {
        foreach (Transform child in m_DialogueChatList.transform)
        {
            Destroy(child.gameObject);
        }
    }

    /* Trigger By: EndConversation
     * Param Needed: None 
     * End a conversation
     */
    private void EndConversation()
    {
        //Delete all button from chat option
        ResetButton();

        //Hide all UI
        m_DialogueChatList.SetActive(false);
        m_DialogueResponse.SetActive(false);
        m_DialogueUI.SetActive(false);

        //Set Converation to false
        m_ConversationInProgress = false;
    }
    #endregion

}
