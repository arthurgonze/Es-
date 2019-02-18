using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Failed_area : MonoBehaviour {

    public GameObject controller;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
       
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Fracassei");
            controller.GetComponent<Game_Controller>().changeGameStatus();
        }
    }
}
