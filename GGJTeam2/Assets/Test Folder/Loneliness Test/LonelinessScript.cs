using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LonelinessScript : MonoBehaviour
{
    //public GameObject home;

    //public float distanceFromHome;

    public bool lonely;

    public Text lonelyText;

    public float lonelinessAmt;

    float currentLoneliness = 100f;

    // Start is called before the first frame update
    void Start()
    {
        lonelinessAmt = currentLoneliness;

        //home = FindObjectOfType<Home>().gameObject;
        lonelyText = GameObject.Find("LonelyText").GetComponent<Text>();

        InvokeRepeating("CheckLoneliness", 10f, 10f);
    }

    // Update is called once per frame
    void Update()
    {
       
            
    }

    void FixedUpdate()
    {
        if (lonely)
        {
            lonelyText.text = "Lonely " + lonelinessAmt;
            lonelyText.color = Color.blue;
            
        }
        else
        {
            lonelyText.text = "Happy";
            lonelyText.color = Color.green;
        }

    }

    private void CheckLoneliness()
    {
        if (lonely && lonelinessAmt > 0)
            lonelinessAmt -= 1;

        else if (!lonely && lonelinessAmt < 100)
            lonelinessAmt += 1;
    }

    public void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "home")
        {
            lonely = false;
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "home")
        {
            lonely = true;
        }
    }
}
