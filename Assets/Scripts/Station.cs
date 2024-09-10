using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Station : MonoBehaviour
{
    protected bool isPlayerNearby = false;
    private Color originalColor;

    private void Start()
    {
        originalColor = this.GetComponent<Renderer>().material.color;
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

        }
    }

    protected void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;


            DeselectedObject();

        }
    }

    private void SelectedObject()
    {
        this.GetComponent<Renderer>().material.color = Color.red;
    }

    private void DeselectedObject()
    {
        this.GetComponent<Renderer>().material.color =originalColor;
    }


}
