using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{

    #region Fields
    [SerializeField]
    private float m_MoveSpeed;
    private Vector2 m_LastMovement;
    private bool m_CanMove;
    private bool m_IsMoving;
    private Animator m_Anima;
    private Rigidbody2D m_Rb;
    private static bool m_PlayerExists;
    #endregion

    #region Properties
    public float MoveSpeed
    {
        get
        {
            return m_MoveSpeed;
        }

        set
        {
            m_MoveSpeed = value;
        }
    }
    public Vector2 LastMovement
    {
        get
        {
            return m_LastMovement;
        }

        set
        {
            m_LastMovement = value;
        }
    }
    public bool CanMove
    {
        get
        {
            return m_CanMove;
        }

        set
        {
            m_CanMove = value;
        }
    }
    public bool IsMoving
    {
        get
        {
            return m_IsMoving;
        }

        set
        {
            m_IsMoving = value;
        }
    }
    public Animator Anima
    {
        get
        {
            return m_Anima;
        }

        set
        {
            m_Anima = value;
        }
    }
    public Rigidbody2D Rb
    {
        get
        {
            return m_Rb;
        }

        set
        {
            m_Rb = value;
        }
    }
    public static bool PlayerExists
    {
        get
        {
            return m_PlayerExists;
        }

        set
        {
            m_PlayerExists = value;
        }
    }
    public static PlayerController Instance { get; set; }
    #endregion

    private void Awake()
    {
        MakeSingleton();
    }
    private void MakeSingleton()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    // Use this for initialization
    void Start()
    {
        //Anima = GetComponent<Animator>();
        Rb = GetComponent<Rigidbody2D>();
        PlayerExists = true;
        CanMove = true;


    }

    // Update is called once per frame
    void Update()
    {
        if (Rb.velocity.magnitude > 0)
        {
            IsMoving = true;
        }
        else
        {
            IsMoving = false;
        }

        if (CanMove)
        {
            if (Input.GetAxisRaw("Horizontal") > 0.5f || Input.GetAxisRaw("Horizontal") < -0.5f)
            {
                //transform.Translate(new Vector3(Input.GetAxisRaw("Horizontal") * moveSpeed * Time.deltaTime, 0f, 0f));
                Rb.velocity = new Vector2(Input.GetAxisRaw("Horizontal") * MoveSpeed, Rb.velocity.y);
                LastMovement = new Vector2(Input.GetAxisRaw("Horizontal"), 0f);
            }
            else
            {
                Rb.velocity = new Vector2(0f, Rb.velocity.y);
            }

            if (Input.GetAxisRaw("Vertical") > 0.5f || Input.GetAxisRaw("Vertical") < -0.5f)
            {
                //transform.Translate(new Vector3(0f, Input.GetAxisRaw("Vertical") * moveSpeed * Time.deltaTime, 0f));
                Rb.velocity = new Vector2(Rb.velocity.x, Input.GetAxisRaw("Vertical") * MoveSpeed);
                LastMovement = new Vector2(0f, Input.GetAxisRaw("Vertical"));
            }
            else
            {
                Rb.velocity = new Vector2(Rb.velocity.x, 0f);
            }

            //Anima.SetFloat("MoveX", Input.GetAxisRaw("Horizontal"));
            //Anima.SetFloat("MoveY", Input.GetAxisRaw("Vertical"));
            //Anima.SetFloat("LastMoveX", LastMovement.x);
            //Anima.SetFloat("LastMoveY", LastMovement.y);
        }
        else
        {
            Rb.velocity = new Vector2(0f, 0f);
        }

        //Anima.SetBool("PlayerMoving", IsMoving);
    }
}