using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ItemState { Raw, Cut, Cooked }

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

    // Estado actual del ítem
    public ItemState itemState;

    // Prefab del ícono en el mundo
    public GameObject worldIconPrefab;

    // Instancia del ícono en el mundo
    private GameObject worldIconInstance;

    public Camera mainCamera;

    private void Start()
    {
        CreateWorldIcon();
        mainCamera = Camera.main;
        UpdateModel();
    }

    public void Update()
    {
        if (worldIconInstance != null)
        {
            worldIconInstance.transform.LookAt(mainCamera.transform);
        }
        UpdateWorldIcon();
    }

    // Crear el ícono sobre el ítem
    private void CreateWorldIcon()
    {
        if (worldIconPrefab != null && worldIconInstance == null)
        {
            worldIconInstance = Instantiate(worldIconPrefab, transform.position + Vector3.up, Quaternion.identity);
            worldIconInstance.transform.SetParent(transform);
            UpdateWorldIcon();
            UpdateModel();
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

    public void UpdateWorldIcon()
    {
        if (worldIconInstance != null)
        {
            // Obtener la referencia a la imagen del ícono en el prefab
            Image iconImage = worldIconInstance.GetComponentInChildren<Image>();

            // Cambiar el sprite de acuerdo al estado del ítem
            switch (itemState)
            {
                case ItemState.Raw:
                    iconImage.sprite = itemIcon;
                    break;
                case ItemState.Cut:
                    iconImage.sprite = cutIcon;
                    break;
                case ItemState.Cooked:
                    iconImage.sprite = cookedIcon;
                    break;
            }
        }
    }

    public void ChangeState(ItemState newState)
    {
        itemState = newState;    // Cambiar el estado
      //  UpdateWorldIcon();
           UpdateModel();
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

    //Destruir el icono del mundo al destruir el ítem
    private void OnDestroy()
    {
        if (worldIconInstance != null)
        {
            Destroy(worldIconInstance);
        }
    }
}
