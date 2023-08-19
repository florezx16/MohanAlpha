using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CollectiablesLogic : MonoBehaviour
{

    Animator animator;
    public TextMeshProUGUI collectiablesText; //Titulo de pistas recolectadas
    public static int collectiablesCount = 0; //Contador de pistas
    public float interactionDistance = 1f;//Distancia de interaccion
    public float CollectCasting;//Duracion de casteo de recoleccion
    public KeyCode interactionKey = KeyCode.E;//Tecla de interaccion
    private Transform collectibleObject;//Objeto para recolectar
    private Coroutine collectiableCasting;//Casteo de recoleccion
    public static CollectiablesLogic Instance;
    private static List<GameObject> collectedObjects = new List<GameObject>();
    public GameObject WinCanvas;
    public GameObject PlayCanvas;
    public AudioManager audioManager;

    //Variables de evento
    public delegate void CollectiableHandler(int collectObjCount);
    public static event CollectiableHandler OnCollectObj;
    private static int currentCollectiblesCount = 0;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    public static int CurrentCollectiblesCount
    {
        get { return currentCollectiblesCount; }
        private set { currentCollectiblesCount = value; }
    }


    void Start()
    {
        animator = GetComponent<Animator>();
        collectiablesText.text = collectiablesCount + "/7";//Definiendo contador
    }

    // Update is called once per frame
    void Update()
    {
        //Verificar tecla de interaccion
        if (Input.GetKey(interactionKey) && collectibleObject != null)
        {
            if (collectiableCasting == null)//Verificamos casteo no iniciado
            {
                animator.SetBool("IsPickUp", true);//Cambio de animacion
                collectiableCasting = StartCoroutine(CastingCollect(CollectCasting, collectibleObject));//Iniciamos casteo
            }
        }
        else if (collectiableCasting != null)
        {
            animator.SetBool("IsPickUp", false);//Cambio de animacion
            StopCoroutine(collectiableCasting);//Pausamos casteo
            collectiableCasting = null;//Eliminamos instancia
        }


        if (collectiablesCount == 7)
        {
            //Win configuration
            WinCanvas.SetActive(true); //Llamamos el canvas de win
            PlayCanvas.SetActive(false); //Pausamos el canvas de play
            Time.timeScale = 0; //Pausamos el juego
            audioManager.PlayGameWin();
            Debug.Log("Win");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        //Verificar si el jugador ha entrado en zona
        if (other.gameObject.CompareTag("Collectibles"))
        {
            Debug.Log("ENTRO EN RANGO");
            collectibleObject = other.transform;//Definiendo objeto a recolectar
        }
    }

    void OnTriggerExit(Collider other)
    {
        //Verificar si el jugador ha salido de zona
        if (other.gameObject.CompareTag("Collectibles"))
        {
            Debug.Log("SALIO DE RANGO");
            if (collectiableCasting != null)//Pausamos casteo si se ha iniciado
            {
                animator.SetBool("IsPickUp", false);//Cambio de animacion
                StopCoroutine(collectiableCasting);//Pausa de casteo
                collectiableCasting = null;//Eliminamos instancia
            }
            collectibleObject = null;//Eliminamos el objeto a recolectar
        }
    }

    private void AddCollectiable(Transform collectible)
    {
        if (collectible != null)//Si el objeto existe
        {
            collectiablesCount++; // Incrementar contador
            CurrentCollectiblesCount = collectiablesCount;
            OnCollectObj?.Invoke(collectiablesCount);//Pasamos el valor al evento
            collectiablesText.text = collectiablesCount + "/7"; //Actualizar numero de pistas
            //Destroy(collectible.gameObject);//Destruir pista en el juego
            CollectObject(collectible);
            //collectible.gameObject.SetActive(false); // Ocultamos pista en el juego
            animator.SetBool("IsPickUp", false);//Cambiamos animacion
        }
    }

    private IEnumerator CastingCollect(float countdownValue, Transform collectible)
    {
        Debug.Log("iniciando casting");
        float currentCount = countdownValue;//Definimos maximo de casting
        yield return new WaitForSeconds(0.1f);//Tiempo para proxima iteraccion
        while (currentCount > 0)
        {
            Debug.Log("conteo -->" + currentCount);
            yield return new WaitForSeconds(0.1f);
            currentCount--;
        }
        //Aplicamos resto de logica
        Debug.Log("Termino");
        AddCollectiable(collectible);
    }

    public static void ResetCollectsCounts()
    {
        collectiablesCount = 0;
        currentCollectiblesCount = 0;
    }

    public void UpdateCountView()
    {
        collectiablesText.text = collectiablesCount + "/7";
    }

    public void CollectObject(Transform collectible)
    {
        collectedObjects.Add(collectible.gameObject);
        collectible.gameObject.SetActive(false);
    }

    public static void ReactivateCollectedObjects()
    {
        if (collectedObjects.Count > 0)
        {
            foreach (GameObject obj in collectedObjects)
            {
                obj.SetActive(true);
            }
            collectedObjects.Clear(); // Limpiamos la lista una vez que todos los objetos han sido reactivados.
        }
    }

}


