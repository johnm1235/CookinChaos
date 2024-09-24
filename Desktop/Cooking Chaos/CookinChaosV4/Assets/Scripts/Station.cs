using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
using TMPro;  

public abstract class Station : MonoBehaviour
{
    [SerializeField]private TMP_Text interactText; 
    protected bool isPlayerNearby = false;
    private Color originalColor;

    [SerializeField] private Color selectedColor;
    private void Start()
    {
        originalColor = this.GetComponent<Renderer>().material.color;
     //   interactText.gameObject.SetActive(false); 
    }

    protected void Update()
    {
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.Space))
        {
            InteractWithStation();
        }
    }

    protected abstract void InteractWithStation();

    protected void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = true;
            SelectedObject();
            ShowInteractText();  // Mostrar el texto cuando el jugador esté cerca
        }
    }

    protected void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;
            DeselectedObject();
            HideInteractText();  // Ocultar el texto cuando el jugador salga
        }
    }

    private void SelectedObject()
    {
        foreach (Transform child in this.transform)
        {
            if (child.GetComponent<Renderer>() != null)
            {
                child.GetComponent<Renderer>().material.color = Color.red;
            }
        }
        this.GetComponent<Renderer>().material.color = Color.red;
    }

    private void DeselectedObject()
    {
        foreach (Transform child in this.transform)
        {
            if (child.GetComponent<Renderer>() != null)
            {
                child.GetComponent<Renderer>().material.color = originalColor;
            }
        }
        this.GetComponent<Renderer>().material.color = originalColor;
    }

    private void ShowInteractText()
    {
       // interactText.text = "[ Space ]";  // Cambiar el texto según sea necesario
      //  interactText.gameObject.SetActive(true);  // Activar el texto
    }

    private void HideInteractText()
    {
      //  interactText.gameObject.SetActive(false);  // Ocultar el texto
    }
}
