using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ItemState { Raw, Cut, Cooked, Mix }

public class Item : MonoBehaviour
{
    public string itemName;

    // Iconos para los diferentes estados
    public Sprite itemIcon;
    public Sprite cutIcon;
    public Sprite cookedIcon;


    // Modelos 3D para los diferentes estados
    public GameObject rawModel;
    public GameObject cutModel;
    public GameObject cookedModel;

    // Estado actual del �tem
    public ItemState itemState = ItemState.Raw; // Estado inicial configurado a Raw

    // Prefab del �cono en el mundo
    public GameObject worldIconPrefab;

    // Lista de instancias de �conos en el mundo
    private List<GameObject> worldIconInstances = new List<GameObject>();

    public Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;
        UpdateModel();
        //UpdateWorldIcon(); // Esta l�nea se comenta, no queremos que el �cono se actualice autom�ticamente al inicio
    }


    public void Update()
    {
        foreach (var iconInstance in worldIconInstances)
        {
            if (iconInstance != null)
            {
                iconInstance.transform.LookAt(mainCamera.transform);
            }
        }
    }

    // Crear el �cono sobre el �tem
    private void CreateWorldIcon(Sprite iconSprite)
    {
        if (worldIconPrefab != null)
        {
            // Calcular la posici�n del nuevo �cono
            Vector3 iconPosition = transform.position + Vector3.up + Vector3.right * worldIconInstances.Count;

            GameObject newIconInstance = Instantiate(worldIconPrefab, iconPosition, Quaternion.identity);
            newIconInstance.transform.SetParent(transform);
            Image iconImage = newIconInstance.GetComponentInChildren<Image>();
            if (iconImage != null)
            {
                iconImage.sprite = iconSprite;
                Debug.Log("Sprite asignado: " + iconSprite.name);
            }
            else
            {
                Debug.LogError("No se encontr� el componente Image en el prefab del �cono.");
            }
            worldIconInstances.Add(newIconInstance);
        }
        else
        {
            Debug.LogError("worldIconPrefab no est� asignado.");
        }
    }

    public void AssignReferences(GameObject itemInstance)
    {
        rawModel = itemInstance.transform.Find("RawModel").gameObject;
        cutModel = itemInstance.transform.Find("CutModel").gameObject;
        cookedModel = itemInstance.transform.Find("CookedModel").gameObject;

        if (rawModel == null || cutModel == null || cookedModel == null)
        {
            Debug.LogError("No se encontraron los modelos 3D correspondientes en el prefab.");
        }
    }

    public void ChangeState(ItemState newState)
    {
        itemState = newState;    // Cambiar el estado
     
        UpdateModel();
       // UpdateWorldIcon();
    }
    
    private void UpdateWorldIcon()
    {
        // Limpiar �conos anteriores
        foreach (var iconInstance in worldIconInstances)
        {
            if (iconInstance != null)
            {
                Destroy(iconInstance);
            }
        }
        worldIconInstances.Clear();

        // Crear nuevo �cono basado en el estado actual
        switch (itemState)
        {
            case ItemState.Raw:
                Debug.Log("Cambiando estado a Raw");
                CreateWorldIcon(itemIcon);
                break;
            case ItemState.Cut:
                Debug.Log("Cambiando estado a Cut");
                CreateWorldIcon(cutIcon);
                break;
            case ItemState.Cooked:
                Debug.Log("Cambiando estado a Cooked");
                CreateWorldIcon(cookedIcon);
                break;
        }
    }

    private void UpdateModel()
    {
        if (rawModel != null) rawModel.SetActive(false);
        if (cutModel != null) cutModel.SetActive(false);
        if (cookedModel != null) cookedModel.SetActive(false);
        Debug.Log("Actualizando modelo de " + itemName + " a " + itemState);

        switch (itemState)
        {
            case ItemState.Raw:
                if (rawModel != null) rawModel.SetActive(true);
                break;
            case ItemState.Cut:
                if (cutModel != null) cutModel.SetActive(true);
                break;
            case ItemState.Cooked:
                if (cookedModel != null) cookedModel.SetActive(true);
                break;
        }
    }

    public void CreateWorldIconBasedOnState()
    {
        UpdateWorldIcon(); // Llama a la funci�n que ya tienes implementada
    }


    // Destruir todos los �conos del mundo al destruir el �tem
    private void OnDestroy()
    {
        foreach (var iconInstance in worldIconInstances)
        {
            if (iconInstance != null)
            {
                Destroy(iconInstance);
            }
        }
    }
}
