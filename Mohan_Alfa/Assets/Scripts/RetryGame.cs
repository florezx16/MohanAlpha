using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static CollectiablesLogic;


public class RetryGame : MonoBehaviour
{
    public EnemyChasing enemyChasing;
    public MainCharacterController MainCharacterController;
    public GameObject PlayCanvas;
    public GameObject gameOverCanvas;
    public GameObject WinCanvas;
    public AudioManager audioManager;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void RetryOnGameOver()
    {
        // Recarga la escena actual
        Debug.Log("Reiniciando canvas de gameplay");
        Time.timeScale = 1;//Quitamos la pausa
        //SceneManager.LoadScene(SceneManager.GetActiveScene().name);//Reiniciamos la escena
        PlayCanvas.SetActive(true);
        gameOverCanvas.SetActive(false);
        audioManager.PlayGamePlay();
        enemyChasing.ResetChase();
        enemyChasing.ResetEnemyPosition();
        MainCharacterController.PlayerResetPosition();
    }

    public void RetryOnWin()
    {
        // Recarga la escena actual
        Debug.Log("Reiniciando canvas de gameplay");
        Time.timeScale = 1;//Quitamos la pausa
        //SceneManager.LoadScene(SceneManager.GetActiveScene().name);//Reiniciamos la escena
        PlayCanvas.SetActive(true);
        WinCanvas.SetActive(false);
        audioManager.PlayGamePlay();
        enemyChasing.ResetChase();
        enemyChasing.ResetEnemyPosition();
        MainCharacterController.PlayerResetPosition();
    }
}
