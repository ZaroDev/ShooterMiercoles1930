using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Start is called before the first frame update
    public bool CanMove { get; private set; } = true; //Getter/Setter
    public bool ShouldCrouch => Input.GetKeyDown(crouchKey) && !duringCrouchingAnimation && controller.isGrounded;
    public bool ShouldJump => controller.isGrounded && Input.GetKey(jumpKey); //
    public bool IsSprinting => canSprint && Input.GetKey(sprintKey); //

    [Header("Opciones de funcionalidad")]
    [SerializeField] private bool canJump = true;
    [SerializeField] private bool canCrouch = true;
    [SerializeField] private bool canSprint = true; //

    [Header("Controles")]
    [SerializeField] private KeyCode crouchKey = KeyCode.LeftControl;
    [SerializeField] private KeyCode jumpKey = KeyCode.Space;
    [SerializeField] private KeyCode sprintKey = KeyCode.LeftShift; //

    [Header("Opciones de movimiento")]
    [SerializeField] private float walkSpeed = 10.0f;
    [SerializeField] private float sprintSpeed = 20.0f;
    [SerializeField] private float gravity = 10.0f;
    [SerializeField] private float jumpForce = 8.0f;
    [SerializeField] private float crouchSpeed = 5.0f;

    private CharacterController controller;

    private Vector3 moveDirection; //Direcion del movimiento
    private Vector2 currentInput;  //Input actual

    [Header("Opciones de agacharse")]
    [SerializeField] private float crouchHeight = 0.5f;
    [SerializeField] private float standingHeight = 2f;
    [SerializeField] private float timeToCrouch = 0.25f;
    [SerializeField] private Vector3 crouchCenter = new Vector3(0, 0.5f, 0);
    [SerializeField] private Vector3 standingCenter = new Vector3(0, 0, 0);
    public bool isCrouching = false;
    private bool duringCrouchingAnimation = false; //Decir si no estamos agachando / poniendo de pie

    [Header("Opciones de stamina")]
    [SerializeField] private float maxStamina = 100f;
    [SerializeField] private float staminaUseMultiplier = 5f;
    [SerializeField] private float timeBeforeStaminaRegenStarts = 5f;
    [SerializeField] private float staminaValueIncrement = 2f;
    [SerializeField] private float staminaTimeIncrement = 0.1f;
    private float currentStamina;
    private Coroutine regeneratingStamina;
    public static Action<float> OnStaminaChange;
    void Awake()
    {
        //Cachear
        controller = GetComponent<CharacterController>();
        currentStamina = maxStamina;
    }

    // Update is called once per frame
    void Update()
    {
        if (CanMove)
        {
            HandleInput();

            if (canJump)
                Jump();

            if (canCrouch)
                HandleCrouch();

            HandleStamina();

            Move();
        }
    }
    void HandleStamina()
    {
        //Consumir stamina
        if (IsSprinting && currentInput != Vector2.zero)
        {
            //Si estamos regenerando paramos de regenerar
            if (regeneratingStamina != null)
            {
                StopCoroutine(regeneratingStamina);
                regeneratingStamina = null;
            }
            //Restar stamina
            currentStamina -= staminaUseMultiplier * Time.deltaTime;

            if (currentStamina < 0)
            {
                currentStamina = 0;
            }

            OnStaminaChange?.Invoke(currentStamina);

            //Si nuestra stamina es más pequeña que 0 dejamos de correr
            if (currentStamina <= 0)
            {
                canSprint = false;
            }
        }
        if (!IsSprinting && currentStamina < maxStamina && regeneratingStamina == null)
        {
            regeneratingStamina = StartCoroutine(RegenerateStamina());
        }
    }
    IEnumerator RegenerateStamina()
    {
        yield return new WaitForSeconds(timeBeforeStaminaRegenStarts);
        while (currentStamina < maxStamina)
        {
            if (currentStamina > 0)
            {
                canSprint = true;
            }

            currentStamina += staminaValueIncrement;

            if (currentStamina > maxStamina)
            {
                currentStamina = maxStamina;
            }

            OnStaminaChange?.Invoke(currentStamina);

            yield return new WaitForSeconds(staminaTimeIncrement);
        }
        regeneratingStamina = null;
    }
    void HandleCrouch()
    {
        if (ShouldCrouch)
            StartCoroutine(CrouchStand());
    }
    private IEnumerator CrouchStand()
    {
        if (isCrouching && Physics.Raycast(Camera.main.transform.position, Vector3.up, 1f))
            yield break;

        duringCrouchingAnimation = true;

        float timeElapsed = 0;
        //Turnary operator
        float targetHeight = isCrouching ? standingHeight : crouchHeight;
        float currentHeight = controller.height;
        Vector3 targetCenter = isCrouching ? standingCenter : crouchCenter;
        Vector3 currentCenter = controller.center;

        while (timeElapsed < timeToCrouch)
        {
            controller.height = Mathf.Lerp(currentHeight, targetHeight, timeElapsed / timeToCrouch);
            controller.center = Vector3.Lerp(currentCenter, targetCenter, timeElapsed / timeToCrouch);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        controller.height = targetHeight;
        controller.center = targetCenter;

        isCrouching = !isCrouching;

        duringCrouchingAnimation = false;
    }
    void HandleInput() //Gestionar el input
    {
        float speed = isCrouching ? crouchSpeed : IsSprinting ? sprintSpeed : walkSpeed;
        currentInput = new Vector2(Input.GetAxis("Vertical") * speed, Input.GetAxis("Horizontal") * speed);

        float moveDirectionY = moveDirection.y;
        moveDirection = (transform.TransformDirection(Vector3.forward) * currentInput.x) + (transform.TransformDirection(Vector3.right) * currentInput.y);
        moveDirection.y = moveDirectionY;
    }
    void Jump()
    {
        if (ShouldJump)
        {
            moveDirection.y += jumpForce;
        }
    }
    void Move()
    {
        if (!controller.isGrounded)
            moveDirection.y -= gravity * Time.deltaTime;

        controller.Move(moveDirection * Time.deltaTime);
    }
}

