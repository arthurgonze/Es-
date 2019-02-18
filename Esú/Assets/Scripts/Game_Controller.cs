using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game_Controller : MonoBehaviour
{

    private enum Gamestatus
    {
        Running,
        Failed,
        Complete,
        Ending
    }


    public GameObject player;
    public GameObject StartSpawn;

    private GameObject activeSpawn;
    private Gamestatus status = Gamestatus.Running;
    private SceneLoader sceneLoader;
    

    // Use this for initialization
    void Awake()
    {
        activeSpawn = StartSpawn;
        player.transform.position = activeSpawn.transform.position;
        sceneLoader = FindObjectOfType<SceneLoader>();
    }

    // Update is called once per frame
    void Update()
    {
        VerificaStatusGame();
    }
    
    //verifica falha de percurso e invoca funçao de respawn
    private void VerificaStatusGame()
    {
        if (status == Gamestatus.Failed)
        {
            RespawnPlayer(activeSpawn);
        }
        else if (status == Gamestatus.Complete)
        {
            status = Gamestatus.Running;
            NextStage();
        }
        else if (status == Gamestatus.Ending)
        {
            CreditsScreen();
        }

    }

    private void CreditsScreen()
    {
        ////segue tela de crditos
    }

    private void NextStage()
    {
        sceneLoader.LoadNextScene();
    }


    public void TaskCompleted()
    {
        status = Gamestatus.Complete;
    }


    //respawna o player na ultima posiçao valida e retorna o fluxo de jogo pra RUNNING
    private void RespawnPlayer(GameObject activeSpawn)
    {
        Debug.Log(" player foi para  o Respawn");
        player.transform.position = activeSpawn.GetComponent<Spawn_Controller>().returnSpawnPoint;
        changeGameStatus();
    }


    //Chama no evento onColider dos spawn pra pegar spawn ativo
    public void setActiveSpawn(GameObject spawn)
    {
        Debug.Log("New Respawn");
        activeSpawn = spawn;
    }




    //Chamado por um inimigo ou evento que interrompa o fluxo do jogo
    public void changeGameStatus()
    {
        if (status == Gamestatus.Running)
        {
            status = Gamestatus.Failed;
            Debug.Log("Status alterado para: " + status);
            RespawnPlayer(activeSpawn);
        }
        else
        {
            status = Gamestatus.Running;
            Debug.Log("Status alterado para: " + status);
        }

    }
}
