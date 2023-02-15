using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float rotacionX = 0;
    public float velY = 2.0f;
    public float velX = 2.0f;
    public float limiteSuperior = 80.0f;
    public float limiteInferior = 80.0f;
    public Camera camera;

    [Header("Paremetros de headbob")]
    public float walkBobSpeed = 14f;
    public float walkBobAmount = 0.05f;
    public float sprintBobSpeed = 18f;
    public float sprintBobAmount = 0.1f;
    public float crouchBobSpeed = 8f;
    public float crouchBobAmount = 0.025f;
    float defaultYPos = 0;
    float timer = 0;

    CharacterController controller;
    PlayerMovement player;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        controller = GetComponent<CharacterController>();
        player = GetComponent<PlayerMovement>();
        defaultYPos = camera.transform.localPosition.y;
    }

    // Update is called once per frame
    void Update()
    {
        MovimientoCamara();
        HandleHeadBob();
    }
    void HandleHeadBob()
    {
        if (!controller.isGrounded)
        {
            return;
        }
        if (Mathf.Abs(controller.velocity.x) > 0.1f || Mathf.Abs(controller.velocity.z) > 0.1f)
        {
            timer += Time.deltaTime *
            (player.isCrouching ? crouchBobSpeed : player.IsSprinting ? sprintBobSpeed : walkBobSpeed);
            camera.transform.localPosition = new Vector3(
                camera.transform.localPosition.x,
                defaultYPos + Mathf.Sin(timer) *
                (player.isCrouching ? crouchBobAmount : player.IsSprinting ? sprintBobAmount : walkBobAmount),
                camera.transform.localPosition.z
            );
        }
    }
    void MovimientoCamara()
    {
        rotacionX -= Input.GetAxis("Mouse Y") * velY;
        rotacionX = Mathf.Clamp(rotacionX, -limiteSuperior, limiteInferior);
        camera.transform.localRotation = Quaternion.Euler(rotacionX, 0, 0);
        transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * velX, 0);
    }
}
