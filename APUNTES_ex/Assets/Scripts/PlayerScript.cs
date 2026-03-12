
using Unity.Hierarchy;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;

public class PlayerScript : MonoBehaviour
{

    //Componentes que usaremos
    private Rigidbody2D rb;
    private PlayerInput playerInput;
    private Animator animator;

    //Variable para el movimiento (horizontal)
    private Vector2 movementInput;
    
    //Variables para configurar la velocidad de movimiento y la fuerza de salto
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 5f;

    // Variable para verificar si el jugador está en el suelo
    public bool isGrounded = false;

    void Start()
    {
        //Obtenemos los componentes necesarios
        rb = GetComponent<Rigidbody2D>();
        playerInput = GetComponent<PlayerInput>();
        animator = GetComponent<Animator>();

        // Evita que el raycast detecte el propio collider del jugador
        Physics2D.queriesStartInColliders = false;

        // Inicializamos el parámetro de animación "IsGrounded" en false para que el jugador comience en el aire. Inicializa el estado del jugador
        // El jugador empieza en el aire hasta que el raycast confirme lo contrario. Evita que el jugador pueda saltar inmediatamente al iniciar la escena. Evita que la animación de “en el suelo” se active antes de tiempo.
        isGrounded = false;

        //Esta línea sincroniza el estado lógico del script con el estado visual del Animator.
        //Si el jugador comienza en el aire, el Animator también reflejará ese estado y no mostrará las animaciones de estar en el suelo hasta que el raycast confirme que el jugador ha aterrizado.
        //Esto asegura que las animaciones se correspondan correctamente con la situación del jugador desde el inicio del juego.
        animator.SetBool("IsGrounded", isGrounded);
       
    }

    void Update()
    {
        //Obtenemos el input de movimiento del jugador
        //playerInput es el componente PlayerInput del nuevo Input System.
        //.actions["Move"] accede a la acción llamada "Move" que has configurado en el Input Actions.
        //.ReadValue<Vector2>() lee el valor actual de esa acción como un Vector2, que representa la dirección del movimiento en el eje horizontal (x) y vertical (y).
        //Ese valor se guarda en movementInput, que luego se usa para mover al jugador.
        movementInput = playerInput.actions["Move"].ReadValue<Vector2>();

        // Actualizamos el parámetro de velocidad en el Animator para controlar las animaciones de movimiento
        //animator.SetFloat("velocityX", ...):
        // En el Animator tienes un parámetro float llamado "velocityX".Aquí le estás pasando la velocidad horizontal del jugador.
        //movementInput.x es el valor horizontal del input (negativo izquierda, positivo derecha).
        //Mathf.Abs lo convierte en valor absoluto, así que da igual si vas a izquierda o derecha, siempre será positivo.
        //Esto sirve para que la animación de “caminar” se active tanto si vas a la derecha como a la izquierda.
        animator.SetFloat("velocityX", Mathf.Abs(movementInput.x));

        //Comprueba si el input horizontal es positivo → el jugador se está moviendo hacia la derecha.
        if (movementInput.x > 0)
        {
            // Si el jugador se mueve hacia la derecha, aseguramos que la escala del sprite esté orientada hacia la derecha
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if(movementInput.x < 0)
        {
            // Si el jugador se mueve hacia la izquierda, invertimos la escala del sprite para que mire hacia la izquierda
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }


    //FixedUpdate() se usa para física, porque se ejecuta a intervalos constantes. Si lo pusieras en Update(), el movimiento sería irregular.
    //Entonces aqui pones el código que aplica el movimiento al Rigidbody2D, y el raycast para verificar si el jugador está en el suelo, porque ambos afectan a la física del jugador.
    void FixedUpdate()
    {
        //Aplicamos el movimiento al Rigidbody2D
        //Cambia la velocidad horizontal del Rigidbody2D.
        //movementInput.x es el input del jugador (−1 izquierda, 1 derecha).
        //moveSpeed es la velocidad configurada en el inspector.
        //rb.linearVelocity.y mantiene la velocidad vertical (para no romper la gravedad ni el salto).
        rb.linearVelocity = new Vector2(movementInput.x * moveSpeed, rb.linearVelocity.y);

        // Realizamos un raycast hacia abajo para verificar si el jugador está en el suelo
        //Lanza un rayo desde la posición del jugador hacia abajo.   0.47f es la distancia del rayo (ajustada al tamaño del sprite).
        //Si el rayo toca algo → devuelve información en hit, si no toca nada → hit.collider será null.
        //hit es simplemente una variable que guarda el resultado del raycast. Si el rayo toca algo, dentro de hit tendrás:hit.collider → el collider que ha tocado, hit.point → el punto exacto donde chocó, hit.distance → la distancia recorrida por el rayo, 
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 0.47f);

        //Comprobación de si está en el suelo
        if (hit.collider != null)
        {
            // Si el raycast detecta un collider debajo del jugador, consideramos que está en el suelo
            isGrounded = true;
            animator.SetBool("IsGrounded", isGrounded);
        }else
        {   // Si el raycast no detecta ningún collider, el jugador está en el aire
            isGrounded = false;
            animator.SetBool("IsGrounded", isGrounded);
        }
        
    }

    // Función para manejar el input de salto del jugador. Jump: es el nombre de la función que has asignado en el PlayerInput a la acción de salto.
    //context trae información sobre el estado del input (si se ha empezado a pulsar, si se ha soltado, etc.). Es obligatorio cuando usas el nuevo Input System con funciones tipo “event”.
    public void Jump(InputAction.CallbackContext context)
    {
        //Verificamos la fase del input para aplicar la fuerza de salto en la etapa "performed"
        //context.performed:  Solo es true cuando la acción de input ha llegado a la fase “performed”. En un botón, suele ser cuando lo pulsas(no cuando lo mantienes ni cuando lo sueltas).
        //&& isGrounded: Además de que el input esté en fase correcta, exige que el jugador esté en el suelo. Así evitas saltos dobles en el aire.
        if (context.performed && isGrounded)
        {

            //rb.AddForce(...): aplica una fuerza al Rigidbody2D.
            //Vector2.up: es el vector (0,1), o sea, hacia arriba.
            //Vector2.up * jumpForce: Escala ese vector según la fuerza de salto que has configurado.
            //ForceMode2D.Impulse: Aplica la fuerza como un impulso instantáneo, no como una fuerza continua. Es perfecto para saltos: un “golpe” hacia arriba en un solo frame.
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }

    // Función para manejar la interacción con las cajas al entrar en contacto con ellas
    //Esta función se ejecuta automáticamente cuando el jugador entra en contacto con otro collider 2D.
    void OnCollisionEnter2D(Collision2D collision)
    {
        // Verificamos si el objeto con el que colisionamos es una caja (su nombre comienza con "Box")
        //collision.gameObject.name.StartsWith("Box"); Mira el nombre del objeto con el que chocaste.Si empieza por "Box" → se considera una caja.
        //this.transform.parent == null: Comprueba si el jugador no tiene ya un padre. Si ya estuviera “pegado” a otra caja, no queremos cambiarlo.
     if (collision.gameObject.name.StartsWith("Box") && this.transform.parent == null)
        {
            // Si el jugador no tiene un objeto padre, asignamos la caja como su hijo para que se mueva junto con el jugador
            //Hace que el jugador se convierta en hijo del objeto con el que chocó (la caja).
            //En Unity, cuando un objeto es hijo de otro: Se mueve junto con él. Su posición se vuelve relativa al padre. Si la caja se desplaza, el jugador también.
            this.transform.SetParent(collision.transform);
        }

    }

    //Esta función se ejecuta automáticamente cuando el jugador deja de colisionar con otro collider 2D.
    void OnCollisionExit2D(Collision2D collision)
    {
        // Verificamos si el objeto con el que dejamos de colisionar es una caja (su nombre comienza con "Box")
        if(collision.gameObject.name.StartsWith("Box"))

        {   // Si el jugador deja de colisionar con la caja, removemos la relación de padre-hijo para que la caja ya no se mueva junto con el jugador
            this.transform.SetParent(null);
        }
    }



}
