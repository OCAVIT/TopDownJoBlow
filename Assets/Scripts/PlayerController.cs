using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float sprintSpeed = 10f;
    public Camera mainCamera;
    public float rotationOffset = 0f;

    public float maxStamina = 100f;
    public float staminaDecreaseRate = 10f;
    public float staminaRecoveryRate = 5f;
    public float staminaRecoveryDelay = 2f;
    public Slider staminaSlider;
    public Image sliderBackground;

    private float currentStamina;
    private float recoveryTimer;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        currentStamina = maxStamina;
        UpdateStaminaUI();
    }

    void Update()
    {
        Move();
        RotateTowardsMouse();
        RecoverStamina();
    }

    void Move()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 move = new Vector3(moveX, 0, moveZ).normalized;
        Vector3 worldMove = mainCamera.transform.TransformDirection(move);

        bool isSprinting = Input.GetKey(KeyCode.LeftShift) && currentStamina > 0;
        float currentSpeed = isSprinting ? sprintSpeed : moveSpeed;

        if (isSprinting)
        {
            currentStamina -= staminaDecreaseRate * Time.deltaTime;
            if (currentStamina < 0) currentStamina = 0;
            recoveryTimer = 0f;
        }
        else
        {
            recoveryTimer += Time.deltaTime;
        }

        rb.velocity = new Vector3(worldMove.x * currentSpeed, rb.velocity.y, worldMove.z * currentSpeed);
        UpdateStaminaUI();
    }

    void RotateTowardsMouse()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hitInfo))
        {
            Vector3 target = hitInfo.point;
            Vector3 direction = (target - transform.position).normalized;
            direction.y = 0;

            Quaternion lookRotation = Quaternion.LookRotation(direction) * Quaternion.Euler(0, rotationOffset, 0);
            rb.rotation = Quaternion.Slerp(rb.rotation, lookRotation, Time.deltaTime * 10f);
        }
    }

    void RecoverStamina()
    {
        if (recoveryTimer >= staminaRecoveryDelay && currentStamina < maxStamina)
        {
            currentStamina += staminaRecoveryRate * Time.deltaTime;
            if (currentStamina > maxStamina) currentStamina = maxStamina;
            UpdateStaminaUI();
        }
    }

    void UpdateStaminaUI()
    {
        staminaSlider.value = currentStamina / maxStamina;

        if (currentStamina > maxStamina * 0.5f)
        {
            sliderBackground.color = Color.Lerp(Color.yellow, Color.green, (currentStamina - maxStamina * 0.5f) / (maxStamina * 0.5f));
        }
        else if (currentStamina > maxStamina * 0.2f)
        {
            sliderBackground.color = Color.Lerp(Color.red, Color.yellow, (currentStamina - maxStamina * 0.2f) / (maxStamina * 0.3f));
        }
        else
        {
            sliderBackground.color = Color.red;
        }
    }
}