using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Camera mainCamera;
    public float rotationOffset = 0f; // Смещение поворота в градусах

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        Move();
        RotateTowardsMouse();
    }

    void Move()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 move = new Vector3(moveX, 0, moveZ).normalized;

        Vector3 worldMove = mainCamera.transform.TransformDirection(move);

        rb.velocity = new Vector3(worldMove.x * moveSpeed, rb.velocity.y, worldMove.z * moveSpeed);
    }

    void RotateTowardsMouse()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hitInfo))
        {
            Vector3 target = hitInfo.point;
            Vector3 direction = (target - transform.position).normalized;
            direction.y = 0;

            // Применяем смещение поворота
            Quaternion lookRotation = Quaternion.LookRotation(direction) * Quaternion.Euler(0, rotationOffset, 0);
            rb.rotation = Quaternion.Slerp(rb.rotation, lookRotation, Time.deltaTime * 10f);
        }
    }
}