using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LonelinessScript : MonoBehaviour
{
    public GameObject home;

    public float distanceFromHome;

    public bool lonely;

    public Text lonelyText;

    // Start is called before the first frame update
    void Start()
    {
        home = FindObjectOfType<Home>().gameObject;
        lonelyText = GameObject.Find("LonelyText").GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        GetPlayerDistance();
    }

    void FixedUpdate()
    {
        if (lonely)
        {
            lonelyText.text = "Lonely";
            lonelyText.color = Color.blue;
        }
        else
        {
            lonelyText.text = "Happy";
            lonelyText.color = Color.green;
        }

    }

    void GetPlayerDistance()
    {
        distanceFromHome = Vector2.Distance(this.transform.position,home.transform.position);

        if (distanceFromHome > 6)
        {
            lonely = true;
        }
        else
            lonely = false;
    }
}
