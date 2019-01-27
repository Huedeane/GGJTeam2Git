using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
//public static LonelinessScript instance;
 
    public GameObject dog;
    public LonelinessScript script;
    public Transform bar;

    // Start is called before the first frame update
    void Start()
    {
        //instance.health;
        dog = GameObject.Find("Player");
        script = dog.GetComponent<LonelinessScript>();
        
        //bar.localScale = new Vector3(.4f,1f);

    }

    // Update is called once per frame
    void Update()
    {
        bar.localScale = new Vector3(script.health/script.MAX_HEALTH,1f,1f);
    }
}
