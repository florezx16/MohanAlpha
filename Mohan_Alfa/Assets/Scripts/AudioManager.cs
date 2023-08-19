using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private AudioSource audidoSource;   //Referencia a nuestro audio source
    public AudioClip MainMenu;          //Preparamos el audio de MainMenu
    public AudioClip GamePlay;          //Preparamos el audio de GamePlay
    public AudioClip GameOver;          //Preparamos el audio de GameOver
    public AudioClip GameWin;           //Preparamos el audio de GameWin

    // Start is called before the first frame update
    void Start()
    {
        audidoSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    //Prepararemos las funciones para reproducir cada uno de los sonidos

    public void PlayMainMenu()
    {
        audidoSource.clip = MainMenu;
        audidoSource.Play();
    }

    public void PlayGamePlay()
    {
        audidoSource.clip = GamePlay;
        audidoSource.Play();
    }

    public void PlayGameOver()
    {
        audidoSource.clip = GameOver;
        audidoSource.Play();
    }

    public void PlayGameWin()
    {
        audidoSource.clip = GameWin;
        audidoSource.Play();
    }
}
