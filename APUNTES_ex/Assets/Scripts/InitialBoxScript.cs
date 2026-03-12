using UnityEngine;
using System.Collections;

public class InitialBoxScript : MonoBehaviour
{
    void Start()
    {
        //Varias opciones para hacerla desaparecer al iniciar la escena, puedes usar cualquiera de las siguientes:
        // Opción 1: Destruir el GameObject
        //Destroy(gameObject, 2f); --> Destruye el GameObject después de 2 segundos
        // Opción 2:Llama al método DestroyBox después de 2 segundos
        //Invoke("DestroyBox", 2f); 
        // Opción 3: Coroutina para destruir el GameObject después de un tiempo



        //Inicia una corutina. Espera 10 segundos. Luego destruye la caja.
        StartCoroutine(DestroyAfterTime(10f));


    }

    void Update()
    {
        
    }

    //Destruye el GameObject al que está unido este script.
    void DestoyBox()
    {
        Destroy(gameObject);
    }

    //IEnumerator → indica que es una corutina.
    //yield return new WaitForSeconds(time);
    //Pausa la ejecución durante time segundos. En este caso, 10 segundos.
    //Destroy(gameObject); Después de esperar, destruye la caja.
    IEnumerator DestroyAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }
}
