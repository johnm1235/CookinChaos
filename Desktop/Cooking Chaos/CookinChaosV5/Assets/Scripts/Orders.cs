using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class Orders : MonoBehaviour
{
    public static Orders Instance;

    public GameObject[] pedidoPrefabs; // El prefab del pedido
    public Transform pedidoParent; // El contenedor donde aparecerán los pedidos
    public float tiempoMaximoPorPedido = 10f; // Tiempo máximo para completar un pedido
    public float intervaloEntrePedidos = 5f; // Intervalo entre la creación de nuevos pedidos
    public int numeroTotalPedidos = 5; // Número total de pedidos que se generarán
    public GameObject[] comidaPrefabs; // Array de prefabs de comida disponibles para los pedidos
    public TMP_Text logroText; // Texto para mostrar los logros
    public TMP_Text logroText2;

    public List<GameObject> pedidosActivos = new List<GameObject>(); // Lista pública de pedidos activos
    public AudioSource audioSource;
    public AudioClip audioClipPedidoCompletado;
    public AudioClip audioClipPedidoFallido;

    private float espacioEntrePedidos = 200f; // Espacio entre pedidos en el eje X
    private bool esperaParaGenerar = false; // Flag para esperar antes de generar nuevos pedidos
    private bool puedeGenerarPedidos = false; // Variable para controlar si se pueden generar pedidos

    // Contadores de pedidos completados y fallidos
    private int pedidosCompletados = 0;
    private int pedidosFallidos = 0;
    private int pedidosCompletadosSinFallar = 0;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        // Desactivar el texto del logro al inicio
        if (logroText != null)
        {
            logroText.gameObject.SetActive(false);
        }

        if (logroText2 != null)
        {
            logroText2.gameObject.SetActive(false);
        }

        StartCoroutine(GestionarPedidos());
    }

    public void CrearPedido(Vector2 posicion)
    {
        if (pedidosActivos.Count < numeroTotalPedidos)
        {
            int comidaIndex = Random.Range(0, comidaPrefabs.Length);
            GameObject comidaSeleccionada = comidaPrefabs[comidaIndex];

            // Encuentra el prefab de pedido correspondiente a esta comida
            GameObject prefabDePedido = pedidoPrefabs[comidaIndex];

            // Instancia un nuevo pedido desde el prefab
            GameObject nuevoPedido = Instantiate(prefabDePedido, pedidoParent);
            nuevoPedido.transform.localPosition = posicion;

            // Asigna un prefab de comida aleatorio al nuevo pedido
            Pedido pedidoScript = nuevoPedido.GetComponent<Pedido>();
            if (pedidoScript != null && comidaPrefabs.Length > 0)
            {
                pedidoScript.comidaPrefab = comidaPrefabs[comidaIndex];
            }

            // Añade el nuevo pedido a la lista de pedidos activos
            pedidosActivos.Add(nuevoPedido);

            // Configura la barra de tiempo para el nuevo pedido
            Slider barraTiempo = nuevoPedido.GetComponentInChildren<Slider>();
            if (barraTiempo != null)
            {
                StartCoroutine(ActualizarBarraTiempo(barraTiempo, nuevoPedido));
            }
        }
    }

    public void ReorganizarPedidos()
    {
        // Reorganiza los pedidos para que ocupen los espacios de los pedidos eliminados
        for (int i = 0; i < pedidosActivos.Count; i++)
        {
            Vector2 nuevaPosicion = new Vector2(i * espacioEntrePedidos, 0); // Actualiza la posición de cada pedido
            pedidosActivos[i].transform.localPosition = nuevaPosicion;
        }
    }

    public IEnumerator EsperarYGenerarPedido()
    {
        // Espera antes de generar un nuevo pedido
        yield return new WaitForSeconds(intervaloEntrePedidos);

        if (pedidosActivos.Count < numeroTotalPedidos)
        {
            CrearPedido(new Vector2(pedidosActivos.Count * espacioEntrePedidos, 0)); // Crea un nuevo pedido en la siguiente posición
        }

        esperaParaGenerar = false; // Restablece el flag de espera
    }

    private IEnumerator GestionarPedidos()
    {
        while (true)
        {
            if (puedeGenerarPedidos && pedidosActivos.Count < numeroTotalPedidos && !esperaParaGenerar)
            {
                StartCoroutine(EsperarYGenerarPedido());
                esperaParaGenerar = true;
            }

            yield return null;
        }
    }

    private IEnumerator ActualizarBarraTiempo(Slider barraTiempo, GameObject pedido)
    {
        float tiempoRestante = tiempoMaximoPorPedido;
        Pedido pedidoScript = pedido.GetComponent<Pedido>();

        while (tiempoRestante > 0)
        {
            if (pedido == null || (pedidoScript != null && pedidoScript.completado)) // Verifica si el pedido aún existe o está completado
            {
                yield break; // Sale de la corrutina si el objeto ha sido destruido o completado
            }

            tiempoRestante -= Time.deltaTime;
            barraTiempo.value = tiempoRestante / tiempoMaximoPorPedido;

            if (tiempoRestante <= 0)
            {
                // Verificar si el pedido no ha sido destruido antes de mostrar la imagen de tiempo agotado
                if (pedido != null)
                {
                    // Mostrar la imagen de tiempo agotado
                    RawImage timeoutImage = pedido.transform.Find("TimeOut").GetComponent<RawImage>();
                    if (timeoutImage != null)
                    {
                        timeoutImage.gameObject.SetActive(true);
                    }

                    if (pedidoScript != null)
                    {
                        pedidoScript.expirado = true; // Marcar el pedido como expirado
                        ScoreManager.Instance.RestarPuntos(10); // Restar puntos por no completar el pedido
                        pedidosFallidos++; // Incrementar el contador de pedidos fallidos
                        pedidosCompletadosSinFallar = 0; // Reiniciar el contador de pedidos completados sin fallar
                        audioSource.PlayOneShot(audioClipPedidoFallido); // Reproducir el sonido de pedido fallido
                    }

                    yield return new WaitForSeconds(2f); // Esperar 2 segundos antes de eliminar

                    if (pedido != null) // Verifica si el pedido aún existe antes de eliminar
                    {
                        EliminarPedido(pedido); // Eliminar el pedido después de mostrar la imagen de tiempo agotado
                    }
                }
                break;
            }

            yield return null;
        }
    }

    public void EliminarPedido(GameObject pedido)
    {
        pedidosActivos.Remove(pedido);
        Destroy(pedido);
        ReorganizarPedidos();

        if (!esperaParaGenerar)
        {
            StartCoroutine(EsperarYGenerarPedido());
            esperaParaGenerar = true;
        }
    }

    public bool VerificarYEliminarPedido(string itemName)
    {
        foreach (GameObject pedidoObj in pedidosActivos)
        {
            Pedido pedido = pedidoObj.GetComponent<Pedido>();
            if (pedido != null && pedido.comidaPrefab != null && pedido.comidaRequerida == itemName)
            {
                if (pedido.expirado)
                {
                    continue; // Ignorar pedidos expirados y pasar al siguiente
                }

                StartCoroutine(MarcarPedidoCompletado(pedidoObj));
                return true;
            }
        }

        return false;
    }

    private IEnumerator MarcarPedidoCompletado(GameObject pedido)
    {
        if (pedido == null) yield break; // Verifica si el pedido aún existe

        Pedido pedidoScript = pedido.GetComponent<Pedido>();
        if (pedidoScript != null)
        {
            pedidoScript.completado = true; // Marcar el pedido como completado
            audioSource.PlayOneShot(audioClipPedidoCompletado); // Reproducir el sonido de pedido completado

            // Calcular puntos basados en el tiempo restante
            Slider barraTiempo = pedido.GetComponentInChildren<Slider>();
            if (barraTiempo != null)
            {
                float tiempoRestante = barraTiempo.value * tiempoMaximoPorPedido;
                int puntos = Mathf.CeilToInt(tiempoRestante);
                ScoreManager.Instance.AgregarPuntos(puntos);

                // Mostrar el logro si el pedido se completa en 40 segundos o menos
                if (tiempoRestante >= tiempoMaximoPorPedido - 40)
                {
                    StartCoroutine(MostrarLogro());
                }
            }

            pedidosCompletados++; // Incrementar el contador de pedidos completados
            pedidosCompletadosSinFallar++; // Incrementar el contador de pedidos completados sin fallar

            // Verificar si se ha completado el segundo logro
            if (pedidosCompletadosSinFallar == 3)
            {
                StartCoroutine(MostrarSegundoLogro()); // Mostrar el segundo logro
            }
        }

        RawImage checkImage = pedido.transform.Find("CheckImage").GetComponent<RawImage>();
        if (checkImage != null)
        {
            checkImage.gameObject.SetActive(true); // Activar el "check"
        }

        yield return new WaitForSeconds(2f); // Esperar 2 segundos antes de eliminar

        if (pedido != null) // Verifica si el pedido aún existe antes de eliminar
        {
            EliminarPedido(pedido); // Eliminar el pedido después de mostrar el "check"
        }
    }

    // Método para habilitar la generación de pedidos
    public void HabilitarGeneracionPedidos()
    {
        puedeGenerarPedidos = true;
        CrearPedido(new Vector2(0, 0)); // Crear el primer pedido en la posición inicial
    }

    // Método para deshabilitar la generación de pedidos
    public void DeshabilitarGeneracionPedidos()
    {
        puedeGenerarPedidos = false;
        StopAllCoroutines(); // Detener todas las corrutinas relacionadas con la generación de pedidos
    }

    // Métodos para obtener los contadores de pedidos completados y fallidos
    public int ObtenerPedidosCompletados()
    {
        return pedidosCompletados;
    }

    public int ObtenerPedidosFallidos()
    {
        return pedidosFallidos;
    }

    private IEnumerator MostrarLogro()
    {

        string logro = SceneManager.GetActiveScene().name;

        // Verificar si el logro ya ha sido obtenido
        if (PlayerPrefs.GetInt("LogroObtenido", 0) == 1 && logro == "Level1")
        {
            yield break; // Salir de la corrutina si el logro ya ha sido obtenido
        }
        else if(PlayerPrefs.GetInt("LogroObtenido2", 0) == 1 && logro == "Level2")
        {
            yield break; // Salir de la corrutina si el logro ya ha sido obtenido
        }

        if (logroText != null && logro == "Level1")
        {
            logroText.gameObject.SetActive(true); // Mostrar el texto del logro
            PlayerPrefs.SetInt("LogroObtenido", 1); // Guardar el estado del logro
            PlayerPrefs.Save(); // Asegurarse de que se guarde inmediatamente
            yield return new WaitForSeconds(2f); // Mostrar el logro durante 2 segundos
            logroText.gameObject.SetActive(false); // Ocultar el texto del logro
        }
        else if (logroText != null && logro == "Level2")
        {
            logroText.gameObject.SetActive(true); // Mostrar el texto del logro
            PlayerPrefs.SetInt("LogroObtenido2", 1); // Guardar el estado del logro
            PlayerPrefs.Save(); // Asegurarse de que se guarde inmediatamente
            yield return new WaitForSeconds(2f); // Mostrar el logro durante 2 segundos
            logroText.gameObject.SetActive(false); // Ocultar el texto del logro
        }
    }

    private IEnumerator MostrarSegundoLogro()
    {
        string logro = SceneManager.GetActiveScene().name;
        // Verificar si el segundo logro ya ha sido obtenido
        if (PlayerPrefs.GetInt("LogroSinFallar", 0) == 1 && logro == "Level1")
        {
            yield break; // Salir de la corrutina si el segundo logro ya ha sido obtenido
        }
        else if (PlayerPrefs.GetInt("LogroSinFallar2", 0) == 1 && logro == "Level2")
        {
            yield break; // Salir de la corrutina si el segundo logro ya ha sido obtenido
        }

        if (logroText2 != null && logro == "Level1")
        {
            logroText2.gameObject.SetActive(true); // Mostrar el texto del segundo logro
            PlayerPrefs.SetInt("LogroSinFallar", 1); // Guardar el estado del segundo logro
            PlayerPrefs.Save(); // Asegurarse de que se guarde inmediatamente
            yield return new WaitForSeconds(2f); // Mostrar el logro durante 2 segundos
            logroText2.gameObject.SetActive(false); // Ocultar el texto del segundo logro
        }
        else if (logroText2 != null && logro == "Level2")
        {
            logroText2.gameObject.SetActive(true); // Mostrar el texto del segundo logro
            PlayerPrefs.SetInt("LogroSinFallar2", 1); // Guardar el estado del segundo logro
            PlayerPrefs.Save(); // Asegurarse de que se guarde inmediatamente
            yield return new WaitForSeconds(2f); // Mostrar el logro durante 2 segundos
            logroText2.gameObject.SetActive(false); // Ocultar el texto del segundo logro
        }
    }

}
