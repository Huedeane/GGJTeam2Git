using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneTransition : MonoBehaviour
{
    [SerializeField] private int m_SpawnID;
    [SerializeField] private Transform m_SpawnPoint;
    [SerializeField] private Object m_SceneTransitionTo;
    [SerializeField] private int m_SceneTransitionSpawnID;
    [SerializeField] private Animator m_FadeAnimation;
    [SerializeField] private Image m_FadeImage;

    public int SpawnID { get => m_SpawnID; set => m_SpawnID = value; }
    public Transform SpawnPoint { get => m_SpawnPoint; set => m_SpawnPoint = value; }
    public Object SceneTransitionTo { get => m_SceneTransitionTo; set => m_SceneTransitionTo = value; }
    public int SceneTransitionSpawnID { get => m_SceneTransitionSpawnID; set => m_SceneTransitionSpawnID = value; }
    public Animator FadeAnimation { get => m_FadeAnimation; set => m_FadeAnimation = value; }
    public Image FadeImage { get => m_FadeImage; set => m_FadeImage = value; }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().CanMove = false;
            StartCoroutine(Transition());
        }

    }

    IEnumerator Transition()
    {
        FadeAnimation.SetBool("Fade", true);
        yield return new WaitUntil(() => FadeImage.color.a == 1);
        SceneManager.LoadScene(m_SceneTransitionTo.name);
        EventManager.TriggerEvent("SceneChange", m_SceneTransitionSpawnID);
    }

}
