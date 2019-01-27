using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LonelinessScript : MonoBehaviour
{
    public GameObject home;

    //private float distanceFromHome;

    public readonly float MAX_HEALTH = 10000f;
    public readonly float MIN_H = 0;

    public float health;

    public bool lonely;

    public Text lonelyText;

   

    // Start is called before the first frame update
    void Start()
    {
        home = FindObjectOfType<Home>().gameObject;
        lonelyText = GameObject.Find("LonelyText").GetComponent<Text>();
        health = MAX_HEALTH;
    }

    // Update is called once per frame
    void Update()
    {
        //GetPlayerDistance();
    }

    void FixedUpdate()
    {
        if (lonely)
        {
            lonelyText.text = "Lonely " + health;            
            lonelyText.color = Color.blue;
            health -= 1f;
            if (health == MIN_H - 1f)
            {
                health ++;
            }
            if (health <= 0 ){
                Debug.Log("You are dead!");
            }
            
        }
        else
        {
            lonelyText.text = "Happy " + health;
            lonelyText.color = Color.green;
            health++;
            if (health > MAX_HEALTH)
            {
                health -= 1f;
            }
            

            
        }
        

    }

    void GetPlayerDistance()
    {
        
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
