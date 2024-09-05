using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject player;
    public float speed = 5.0f;
    public float horizontalInput;
    public float verticalInput;
    private CharacterController controller;

    public void Start()
    {
        player = GameObject.Find("Player");
        controller = GetComponent<CharacterController>();
    }

    public void Update()
    {
        Move();
    }

    public void Move()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        Vector3 move = new Vector3(horizontalInput, 0, verticalInput);
        controller.Move(move * speed * Time.deltaTime);
    }
}
