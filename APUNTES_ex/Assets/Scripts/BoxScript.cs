using UnityEngine;

public class BoxScript : MonoBehaviour
{

    // Velocidad a la que la caja cae.
    //Es privada, así que solo el script puede modificarla.
    private float speed = 2f;


    void Start()
    {
        
    }

    //transform.position += … Modifica la posición de la caja.
    //+= significa “suma a la posición actual”.
    //Vector3.down Es el vector (0, -1, 0). Representa hacia abajo en Unity.
    //speed Multiplica la dirección por la velocidad. Cuanto mayor sea speed, más rápido cae.
    //Time.deltaTime
    //Hace que el movimiento sea suave y constante, independientemente de los FPS.
    //Sin deltaTime, la caja caería más rápido en PCs potentes y más lento en PCs lentos.
    void Update()
    {
        transform.position += Vector3.down * speed * Time.deltaTime;
    }


}