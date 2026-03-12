using NUnit.Framework;
using UnityEngine;

public class DeadZoneScript : MonoBehaviour
{

    //[SerializeField] permite asignar el GameManager desde el inspector.
    //gameManager es el objeto que tiene el script GameManagerScript.
    //Desde aquí se llamará a GameOver() y AddScore().
    [SerializeField] private GameObject gameManager;

    void Start()
    {
        
    }

    
    void Update()
    {
        
    }

    //Esta función se ejecuta automáticamente cuando algo entra en el trigger de la Dead Zone.
    //other es el collider del objeto que ha entrado.
    //Para que esto funcione: La Dead Zone debe tener un Collider2D con “Is Trigger” activado. Los objetos que caen (jugador y cajas) deben tener Rigidbody2D.
    void OnTriggerEnter2D(Collider2D other)
    {
        // Verificamos si el objeto que entra en la zona de muerte es el jugador o una caja
        //other.gameObject.transform.Find("Player") != null Esto detecta si el jugador está como hijo del objeto.
        //¿Por qué? Porque cuando el jugador está encima de una caja, se convierte en hijo de la caja (por el script del Player).
        //Si la caja cae con el jugador encima, esta condición detecta al jugador aunque no sea el objeto principal.
        if (other.gameObject.CompareTag("Player") || other.gameObject.transform.Find("Player") != null)
        {
            // Si el jugador entra en la zona de muerte, llamamos a la función GameOver del GameManager
            //Accede al script GameManagerScript del GameManager.
            //Llama a la función GameOver().
            //Esto: Muestra el mensaje de Game Over. Muestra el botón de reset. Detiene la suma de puntos.
            gameManager.GetComponent<GameManagerScript>().GameOver();
        }

        //Comprueba si el nombre del objeto empieza por "Box".
        //Esto identifica las cajas que caen.
        else if (other.gameObject.name.StartsWith("Box"))
        {
            // Si una caja entra en la zona de muerte, la destruimos la caja y sumamos un punto al marcador
            gameManager.GetComponent<GameManagerScript>().AddScore();
            Destroy(other.gameObject);
        }
    }
}
