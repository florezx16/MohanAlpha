using UnityEngine;
using UnityEngine.AI;
using static CollectiablesLogic;

public class EnemyChasing : MonoBehaviour
{
    public Transform playerTransform;   // Referencia al transform del jugador
    public float defaultSpeed = 3.5f;   // Velocidad por defecto del enemigo
    private NavMeshAgent agent;         // Referencia al NavMeshAgent
    public GameObject gameOverCanvas;   // Canvas de gameOver
    public GameObject GameplayCanvas;   // Canvas de gameplay
    private Animator animator;          // Animator de Mohan
    private bool isChasing = false;     // Persecucion iniciada si/no
    public AudioSource alertSound;      // Efectos de sonido
    private bool AlertPlayed = false;   // Bandera de verificacion de efecto de sonido
    private Vector3 initialPosition;    //Guardamos posicion inicial
    private Quaternion initialRotation; //Gaurdamos la rotacion
    public AudioManager audioManager;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        agent.speed = defaultSpeed;

        // Se suscribe al evento
        CollectiablesLogic.OnCollectObj += IncreaseSpeedOnCollect;
    }

    private void Start()
    {
        initialPosition = transform.position;
        initialRotation = transform.rotation;
        gameOverCanvas.SetActive(false);

        if (playerTransform == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                playerTransform = player.transform;
            }
            else
            {
                Debug.LogWarning("Sin jugador");
            }
        }
    }

    private void Update()
    {
        if (CollectiablesLogic.CurrentCollectiblesCount > 0)
        {
            //Debug.Log("Iniciando persecucion");
            agent.SetDestination(playerTransform.position);
            animator.SetBool("IsRunning", true);
            isChasing = true;

            if (!AlertPlayed)
            {
                alertSound.Play();
                AlertPlayed = true;
            }
        }
    }

    private void OnDestroy()
    {
        //Nos desligamos del evento
        CollectiablesLogic.OnCollectObj -= IncreaseSpeedOnCollect;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            gameOverCanvas.SetActive(true); //Llamamos el canvas de GameOver
            GameplayCanvas.SetActive(false); //Pausamos el canvas de play
            Time.timeScale = 0; //Pausamos el juego
            audioManager.PlayGameOver();
            Debug.Log("GameOver");
        }
    }

    private void IncreaseSpeedOnCollect(int currentCollectibles)
    {
        //Aumentamos la velocidad del enemigo
        agent.speed = defaultSpeed * (1 + currentCollectibles);
    }

    public void ResetChase()
    {
        CollectiablesLogic.ResetCollectsCounts();//Reiniciamos contadores
        CollectiablesLogic.Instance.UpdateCountView();//Reiniciamos el contador en el canvas
        CollectiablesLogic.ReactivateCollectedObjects();//Volvemos a activar las pistas recolectadas
        agent.isStopped = true;  //Detenemos el evento
        AlertPlayed = false; //Activamos nuevamente la bandera para reproducir la alerta
        agent.ResetPath(); //Reiniciamos el Nav del enemigo
        animator.SetBool("IsRunning", false);  //Cambiamos animacion
        Debug.Log("Reinicio de contador y chasing");
    }

    public void ResetEnemyPosition()
    {
        agent.enabled = false; // Desactiva el NavMeshAgent.
        transform.position = initialPosition;
        transform.rotation = initialRotation;
        agent.enabled = true; // Reactiva el NavMeshAgent.
    }
}
