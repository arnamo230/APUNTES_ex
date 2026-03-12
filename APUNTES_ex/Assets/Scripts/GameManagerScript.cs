using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class GameManagerScript : MonoBehaviour
{
    //[SerializeField]: permite asignar estos objetos desde el inspector aunque sean private.
    //SpawnPoint1/2/3: posiciones posibles donde aparecerán cajas.
    //boxPrefab: prefab de la caja que se va a instanciar durante la partida.
    //initialBoxPrefab: caja inicial (la que aparece al resetear).
    //playerSpawnPoint: posición donde reaparece el jugador al resetear.
    //initialBoxSpawnPoint: posición donde aparece la caja inicial al resetear.
    //scoreText: texto UI (TMP) donde se muestra el marcador.
    //resetButton: botón de reinicio.
    //score: contador de puntos.
    //isGameOver: indica si la partida ha terminado.
    [SerializeField] private GameObject SpawnPoint1;
    [SerializeField] private GameObject SpawnPoint2;
    [SerializeField] private GameObject SpawnPoint3;
    [SerializeField] private GameObject boxPrefab;
    [SerializeField] private GameObject initialBoxPrefab;
    [SerializeField] private GameObject playerSpawnPoint;
    [SerializeField] private GameObject initialBoxSpawnPoint;
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private Button resetButton;


    // Variable para llevar el marcador de puntos
    private int score = 0;

    public bool isGameOver = false;

    void Start()
    {
        // Llamamos a la función SpawnBox cada 2 segundos para generar cajas de forma periódica
        //InvokeRepeating("SpawnBox", 2f, 2f): Llama a la función SpawnBox por primera vez a los 2 segundos. Luego la repite cada 2 segundos. Esto genera cajas de forma periódica.
        InvokeRepeating("SpawnBox", 2f, 2f);

        //El botón de reset solo se muestra cuando el juego termina, por lo que lo ocultamos al iniciar la escena
       resetButton.gameObject.SetActive(false);
    }

    void Update()
    {
        
    }

    public void GameOver()
    {
        //Cambiar el texto del marcador
        //scoreText: es el texto de la UI (TMP_Text) que muestra el marcador.
        //.text = ...: cambia lo que se ve en pantalla.
        //"Game Over, Final Score: ": texto fijo del mensaje.
        //this.score.ToString(): convierte el número de puntos (int) a texto.
        scoreText.text = "Game Over, Final Score: " + this.score.ToString();

        //Marcar que el juego ha terminado
        isGameOver = true;

        //resetButton: es el botón de la UI.
        //.gameObject: accede al objeto al que pertenece el botón.
        //.SetActive(true): lo hace visible y usable.
        //Antes estaba oculto en Start(), ahora aparece para que el jugador pueda reiniciar.
        // Mostramos el botón de reset para que el jugador pueda reiniciar el juego
        resetButton.gameObject.SetActive(true);
    }

    public void SpawnBox()
    {
        // Generamos un número aleatorio entre 0 y 2 para seleccionar uno de los tres puntos de spawn
        int spawnPointIndex = UnityEngine.Random.Range(0, 3);

        //Se crea una variable spawnPosition que luego se rellenará según el punto elegido. 
        Vector3 spawnPosition;

        //Según el número aleatorio (spawnPointIndex), elige uno de los tres puntos de spawn.
        //Cada SpawnPointX es un GameObject colocado en la escena.
        //.transform.position obtiene su posición exacta.
        switch (spawnPointIndex)
        {
            case 0:
                spawnPosition = SpawnPoint1.transform.position;
                break;
            case 1:
                spawnPosition = SpawnPoint2.transform.position;
                break;
            case 2:
                spawnPosition = SpawnPoint3.transform.position;
                break;
            default:
                spawnPosition = SpawnPoint1.transform.position;
                break;
        }
        // Instanciamos una nueva caja en la posición seleccionada
        //Para crear objetos durante el juego, necesitas instanciarlos desde un prefab
        //Crea una copia del prefab boxPrefab.
        //La coloca en spawnPosition.
        //Quaternion.identity significa sin rotación.
        Instantiate(boxPrefab, spawnPosition, Quaternion.identity);
    }


    public void AddScore()
    {
        // Si el juego ha terminado, no incrementamos el marcador
        //Comprueba si isGameOver es true.
        //Si lo es, ejecuta return; → sale de la función inmediatamente.
        if (isGameOver)
        {
            return; 
        }

        // Incrementamos el marcador en 1 cada vez que se llama a esta función
        //this.score es la variable privada que guarda los puntos.
        //++ significa sumar 1.
        //Cada vez que se llama a AddScore(), el jugador gana un punto.
        this.score++;
        scoreText.text = "Score: " +  this.score.ToString();
    }

    public void ResetGame()
    {
        // Reiniciamos el marcador a 0 y actualizamos el texto en pantalla
        this.score = 0;
        scoreText.text = "Score: " + this.score.ToString();
        // Reiniciamos el estado del juego para permitir que se sigan sumando puntos
        isGameOver = false;

        // Reposicionamos al jugador y a la caja inicial en sus puntos de spawn correspondientes
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        player.transform.position = playerSpawnPoint.transform.position;

        // Reiniciamos la velocidad del jugador para evitar que siga moviéndose después de ser reposicionado   
        player.GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero; 

        // Destruimos todas las cajas que estén actualmente en la escena para limpiar el juego
        Instantiate(initialBoxPrefab, initialBoxSpawnPoint.transform.position, Quaternion.identity);

        // Ocultamos el botón de reset nuevamente para que no se muestre durante el juego
        resetButton.gameObject.SetActive(false);
    }

}
