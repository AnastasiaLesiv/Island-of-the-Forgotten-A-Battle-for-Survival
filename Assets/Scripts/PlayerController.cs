using System;
using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    float walkSpeed = 1f; //regular speed
    private float runSpeed = 1.5f;

    public Animator animator;
    public float jumpForse = 3.5f;
    private float animationInterpolation = 1f;

    private CharacterController characterController;
    private Camera camera;
    private Vector3 velocity;
    private Vector2 direction;
    private float gravity = -9.81f;
    float camSens = 0.25f;
    Vector3 lastMouse = new Vector3(255, 255, 255);
    private bool isMovementBlocked = false;
    public Transform aimTarget;
    public float maxVerticalAngle = -60f; 

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        camera = GetComponentInChildren<Camera>();
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (aimTarget == null)
        {
            aimTarget = new GameObject("AimTarget").transform;
            aimTarget.position = camera.transform.position + camera.transform.forward * 2f;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            animator.SetTrigger("hit");
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            isMovementBlocked = !isMovementBlocked; // Змінюємо стан блокування
            Cursor.visible = isMovementBlocked; // Відображення курсору, коли рух заблокований
            Cursor.lockState = isMovementBlocked ? CursorLockMode.None : CursorLockMode.Locked; // Змінюємо стан курсора

            if (isMovementBlocked)
            {
                // При блокуванні руху вивести курсор з центру екрану для більшої зручності
                lastMouse = new Vector3(Screen.width / 2, Screen.height / 2, 0);
                Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto); // За потреби, встановити кастомний курсор
            }
        }

        if (isMovementBlocked)
        {
            return; // Якщо рух заблокований, виходимо з Update
        }

        // Оновлюємо лише кут обертання по осі Y
        float mouseX = Input.GetAxis("Mouse X") * camSens * 2;
        float mouseY = Input.GetAxis("Mouse Y") * camSens * 4;

        transform.Rotate(0, mouseX, 0);
        camera.transform.Rotate(-mouseY, 0, 0); // Додаємо обертання камери по осі X

 // Оновлюємо позицію aimTarget для коректного напрямку погляду
        Ray desiredTargetRay = camera.GetComponent<Camera>()
            .ScreenPointToRay(new Vector2(Screen.width / 2, Screen.height / 2));
        Vector3 desiredTargetPosition = desiredTargetRay.origin + desiredTargetRay.direction * 2f;
        aimTarget.position = desiredTargetPosition;
        
        lastMouse = Input.mousePosition;

        characterController.Move(velocity * Time.deltaTime);
        direction = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        if (characterController.isGrounded)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                animator.SetTrigger("jump");
                velocity.y = jumpForse;
            }
            else
            {
                velocity.y = -0.1F;
            }
        }
        else
        {
            velocity.y += gravity * Time.deltaTime;
        }
    }

    void OnAnimatorIK(int layerIndex)
    {
        if (animator)
        {
            animator.SetLookAtWeight(1);
            animator.SetLookAtPosition(aimTarget.position);
        }
    }

    private void FixedUpdate()
    {
        if (isMovementBlocked)
        {
            return; // Якщо рух заблокований, виходимо з FixedUpdate
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            Run();
            direction *= runSpeed;
        }
        else
        {
            Walk();
            direction *= walkSpeed;
        }

        // Напрямок руху обчислюємо з урахуванням тільки осі Y обертання персонажа
        Vector3 move = Quaternion.Euler(0, camera.transform.eulerAngles.y, 0) *
                       new Vector3(direction.x, 0, direction.y);
        velocity = new Vector3(move.x, velocity.y, move.z);

        characterController.Move(velocity * Time.deltaTime);
    }

    private void Run()
    {
        animationInterpolation = Mathf.Lerp(animationInterpolation, 1.5f, Time.deltaTime * 3);
        animator.SetFloat("x", Input.GetAxis("Horizontal") * animationInterpolation);
        animator.SetFloat("y", Input.GetAxis("Vertical") * animationInterpolation);
    }

    private void Walk()
    {
        animationInterpolation = Mathf.Lerp(animationInterpolation, 1f, Time.deltaTime * 3);
        animator.SetFloat("x", Input.GetAxis("Horizontal") * animationInterpolation);
        animator.SetFloat("y", Input.GetAxis("Vertical") * animationInterpolation);
    }
}
