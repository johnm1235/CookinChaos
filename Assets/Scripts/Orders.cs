using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    public List<GameObject> pedidosActivos = new List<GameObject>(); // Lista pública de pedidos activos

    private float espacioEntrePedidos = 200f; // Espacio entre pedidos en el eje X
    private bool esperaParaGenerar = false; // Flag para esperar antes de generar nuevos pedidos
    private bool puedeGenerarPedidos = false; // Variable para controlar si se pueden generar pedidos

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
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

            // Calcular puntos basados en el tiempo restante
            Slider barraTiempo = pedido.GetComponentInChildren<Slider>();
            if (barraTiempo != null)
            {
                float tiempoRestante = barraTiempo.value * tiempoMaximoPorPedido;
                int puntos = Mathf.CeilToInt(tiempoRestante * 5); // Ejemplo: 10 puntos por segundo restante
                ScoreManager.Instance.AgregarPuntos(puntos);
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

}
