using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasNav : MonoBehaviour
{
    //Definimos los canvas a utilizas
    public GameObject MainCanvas;       
    public GameObject CreditsCanvas;
    public GameObject PlayCanvas;

    public AudioManager audioManager;   //Traemos nuestro audio manager

    // Start is called before the first frame update
    void Start()
    {
        //EN un inicio reproduciremos la musica dle main menu
        audioManager.PlayMainMenu();

    }

    // Update is called once per frame
    void Update()
    {

    }

    //Preparamos las funciones para activar cada uno de los canvas dentro del juego
    public void FromCredits2Main()
    {
        MainCanvas.SetActive(true);
        CreditsCanvas.SetActive(false);
    }

    public void FromMain2Credits()
    {
        MainCanvas.SetActive(false);
        CreditsCanvas.SetActive(true);
    }

    public void FromMain2Play()
    {
        audioManager.PlayGamePlay();
        MainCanvas.SetActive(false);
        PlayCanvas.SetActive(true);
    }

    //Prepararemos la funcion para salir del juego
    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }
}
