using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camp_Controller : MonoBehaviour {

    public enum TypeObjective {
        None,
        Item,
        Rescue
    }

    public TypeObjective type;
    public GameObject gameManager;
    public GameObject targetItem;
    public GameObject targetDoor;


    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player") {
            switch (type) {
                case TypeObjective.None:
                    FinishStage();
                    break;
                case TypeObjective.Item:
                    if (ConsultItem(targetItem)) {
                        FinishStage();
                    }
                    break;
                case TypeObjective.Rescue:
                    if (ConsultDoor(targetDoor))
                    {
                        FinishStage();
                    }
                    break;
            }

        }
    }

    private bool ConsultDoor(GameObject targetDoor)
    {
        //////consulta se a porta target foi aberta e o escravo liberto
        return false;
    }

    private bool ConsultItem(GameObject targetItem)
    {
        //////consulta existencia do item target no inventario
        return false;
    }

    private void FinishStage()
    {
        gameManager.GetComponent<Game_Controller>().TaskCompleted();
    }
}
