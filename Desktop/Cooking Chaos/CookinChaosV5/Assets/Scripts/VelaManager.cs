using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VelaManager : MonoBehaviour
{
    public List<Vela> velas;  // Lista de todas las velas en la escena
    public float intervaloApagado;  // Intervalo de tiempo entre el apagado de una vela y otra

    private void Start()
    {
        // Iniciar la corrutina para apagar velas en orden aleatorio
        StartCoroutine(ApagarVelasAleatoriamente());
    }

    // Corrutina para apagar velas en orden aleatorio
    IEnumerator ApagarVelasAleatoriamente()
    {
        while (true)
        {
            // Esperar el intervalo definido antes de apagar otra vela
            yield return new WaitForSeconds(intervaloApagado);

            // Apagar una vela al azar de la lista
            ApagarVelaAleatoria();
        }
    }

    // M�todo para apagar una vela al azar
    private void ApagarVelaAleatoria()
    {
        // Filtrar velas que est�n encendidas
        List<Vela> velasEncendidas = velas.FindAll(vela => !vela.estaApagada);

        if (velasEncendidas.Count > 0)
        {
            // Elegir una vela aleatoria de las encendidas
            int indiceAleatorio = Random.Range(0, velasEncendidas.Count);
            Vela velaSeleccionada = velasEncendidas[indiceAleatorio];

            // Apagar la vela seleccionada
            velaSeleccionada.ApagarVela();
        }
    }
}
