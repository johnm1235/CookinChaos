using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Station : MonoBehaviour
{
    protected bool isPlayerNearby = false;
    public GameObject interactionUI;

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

            interactionUI.SetActive(true);

        }
    }

    protected void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;

            interactionUI.SetActive(false);

        }
    }
}
