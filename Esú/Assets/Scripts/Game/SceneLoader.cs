using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    //GameStatus gameStatus;

    private void Start()
    {
        //gameStatus = FindObjectOfType<GameStatus>();
    }
    public void LoadNextScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;//faz a variavel guardar o indice da cena atual de jogo
        SceneManager.LoadScene(currentSceneIndex + 1);//chama a função de carregar uma cena e passa como parametro o indice atual + 1 para ir para próxima cena
    }

    public void FirstScreen()
    {
        //gameStatus.Reset();
        SceneManager.LoadScene(0);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}

