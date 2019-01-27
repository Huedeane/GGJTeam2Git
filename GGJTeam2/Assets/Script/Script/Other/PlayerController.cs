using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum E_PlayerDirection { Left, Right, Up, Down}

public class PlayerController : MonoBehaviour
{

    #region Fields
    [SerializeField] private float m_PlayerSpeed = 5f;
    [SerializeField] private bool m_IsDead;
    [SerializeField] private bool m_IsMoving;
    [SerializeField] private bool m_CanMove = true;
    [SerializeField] private Rigidbody2D m_PlayerRB;
    [SerializeField] private E_PlayerDirection m_PlayerDirection;
    [SerializeField] private SpriteRenderer m_SpriteRenderer;
    [SerializeField] private float m_XAxis;
    [SerializeField] private float m_YAxis;
    [SerializeField] private float m_CurrentRunSpeed;
    [SerializeField] private Animator m_Animators;
    #endregion

    #region Properties
    public float PlayerSpeed { get => m_PlayerSpeed; set => m_PlayerSpeed = value; }
    public bool IsDead { get => m_IsDead; set => m_IsDead = value; }
    public Rigidbody2D PlayerRB { get => m_PlayerRB; set => m_PlayerRB = value; }
    public E_PlayerDirection PlayerDirection { get => m_PlayerDirection; set => m_PlayerDirection = value; }
    public SpriteRenderer SpriteRenderer { get => m_SpriteRenderer; set => m_SpriteRenderer = value; }
    public float XAxis { get => m_XAxis; set => m_XAxis = value; }
    public float YAxis { get => m_YAxis; set => m_YAxis = value; }
    public float CurrentRunSpeed { get => m_CurrentRunSpeed; set => m_CurrentRunSpeed = value; }
    public Animator Animators { get => m_Animators; set => m_Animators = value; }
    public bool CanMove { get => m_CanMove; set => m_CanMove = value; }
    #endregion

    // Use this for initialization
    void Start()
    {
        


    }

    // Update is called once per frame
    void Update()
    {
        transform.position += Vector3.zero;
        if (m_CanMove == true) {
            m_XAxis = Input.GetAxisRaw("Horizontal");
            m_YAxis = Input.GetAxisRaw("Vertical");
            m_Animators.SetFloat("XInput", m_XAxis);
            m_Animators.SetFloat("YInput", m_YAxis);

            //Set Direction
            #region SetDirection
            if (m_XAxis > 0) {
                m_PlayerDirection = E_PlayerDirection.Right;

            }
            else if (m_XAxis < 0) {
                m_PlayerDirection = E_PlayerDirection.Left;
            }
            else if (YAxis < 0) {
                m_PlayerDirection = E_PlayerDirection.Down;
            }
            else if (YAxis > 0)
            {
                m_PlayerDirection = E_PlayerDirection.Up;
            }
            #endregion

            if (Mathf.Abs(m_XAxis) > 0 || Mathf.Abs(YAxis) > 0)
            {
                m_IsMoving = true;
                m_Animators.SetBool("IsMoving", m_IsMoving);
            }
            else
            {
                m_IsMoving = false;
                m_Animators.SetBool("IsMoving", m_IsMoving);
            }

            m_PlayerRB.velocity = new Vector2(m_XAxis * m_PlayerSpeed, m_YAxis * m_PlayerSpeed);
        }
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject objectInRange = collision.transform.gameObject;
        IInteractableObject interactableObject;
        
        if (objectInRange.GetComponent<ObjectComponent>() && objectInRange.GetComponent<ObjectComponent>().GetObjectType().IsInteractable)
        {
            interactableObject = objectInRange.GetComponent<ObjectComponent>().GetObjectType();
            GameManager.Instance.interactionText.SetText(interactableObject.InteractionText);
        }

    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        GameObject objectInRange = collision.transform.gameObject;
        IInteractableObject interactableObject;
        if (objectInRange.GetComponent<ObjectComponent>() && objectInRange.GetComponent<ObjectComponent>().GetObjectType().IsInteractable)
        {
            Debug.Log("Work2");
            if (Input.GetKeyDown(KeyCode.Space))
            {
                interactableObject = objectInRange.GetComponent<ObjectComponent>().GetObjectType();
                Debug.Log("Work");
                switch (objectInRange.GetComponent<ObjectComponent>().ObjectType)
                {
                    case E_ObjectType.Item:
                        EventManager.TriggerEvent("ItemAdded", objectInRange.GetComponent<ObjectComponent>().ItemObject);
                        GameManager.Instance.interactionText.SetText("");
                        Destroy(objectInRange.gameObject);
                        break;
                    case E_ObjectType.NPC:
                        StartCoroutine(interactableObject.ExecuteInteraction());
                        break;
                }


            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        GameManager.Instance.interactionText.SetText("");
    }

}