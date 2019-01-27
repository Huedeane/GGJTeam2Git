using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUpTest : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown((KeyCode.E)))
        {
            Debug.Log(("press"));
            PopUpInfoManager test = GameObject.FindWithTag("Manager").GetComponent<PopUpInfoManager>();
            test.AddInfo(new Info("Quest Added", "Make Info Manager", 5f));
            StartCoroutine(test.ActivePopUpDialog());
        }
        if (Input.GetKeyDown((KeyCode.X)))
        {
            Debug.Log(("press"));
            PopUpInfoManager test = GameObject.FindWithTag("Manager").GetComponent<PopUpInfoManager>();
            test.AddInfo(new Info("Quest Completed", "Make Info Manger", 5f));
            StartCoroutine(test.ActivePopUpDialog());
        }
        if (Input.GetKeyDown((KeyCode.Z)))
        {
            Debug.Log(("press"));
            PopUpInfoManager test = GameObject.FindWithTag("Manager").GetComponent<PopUpInfoManager>();
            test.AddInfo(new Info("Reaction", "Hieu Will Remember That", 5f));
            StartCoroutine(test.ActivePopUpDialog());
        }
    }
}
