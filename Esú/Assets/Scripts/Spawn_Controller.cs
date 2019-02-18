using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn_Controller : MonoBehaviour {

    public GameObject gameController;
   
    private Vector2 spawn;
    private bool activeSpawn = false;

    private void Awake()
    {
        spawn = transform.position;
    }

    // Use this for initialization
    void Start () {
        spawn = transform.position;
    }
	
	// Update is called once per frame
	void Update () {
	}

    public Vector2 returnSpawnPoint {
        get { return spawn; }
    }

    public GameObject returnSpawnActive
    {
        get { return gameObject; }
    }


    public void OnTriggerEnter2D(Collider2D collision)
    {
       
      if(collision.tag == "Player") {
            Debug.Log("Respawn");
            activeSpawn = true;
            gameController.GetComponent<Game_Controller>().setActiveSpawn(gameObject);
       }
    }


}
