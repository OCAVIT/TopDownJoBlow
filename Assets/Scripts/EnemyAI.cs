using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public float detectionRange = 10f;
    public float shootingRange = 5f;
    public float rotationSpeed = 5f;
    public GameObject bulletPrefab;
    public Transform bulletSpawnPoint;
    public float fireRate = 1f;

    private NavMeshAgent agent;
    private Transform player;
    private float nextFireTime = 0f;
    private Animator animator;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>(); // Получаем компонент Animator

        // Находим игрока по тегу
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform;
        }
        else
        {
            Debug.LogError("Player not found! Make sure the player object has the 'Player' tag.");
        }
    }

    void Update()
    {
        if (player == null)
            return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectionRange)
        {
            // Поворачиваем врага в сторону игрока
            Vector3 direction = (player.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);

            if (distanceToPlayer <= shootingRange)
            {
                // Останавливаемся и стреляем
                agent.isStopped = true;
                animator.SetBool("isWalking", false); // Останавливаем анимацию ходьбы
                Shoot();
            }
            else
            {
                // Двигаемся к игроку
                agent.isStopped = false;
                agent.SetDestination(player.position);
                animator.SetBool("isWalking", true); // Включаем анимацию ходьбы
            }
        }
        else
        {
            // Если игрок вне зоны видимости, враг может патрулировать или оставаться на месте
            agent.isStopped = true;
            animator.SetBool("isWalking", false); // Останавливаем анимацию ходьбы
        }
    }

    void Shoot()
    {
        if (Time.time > nextFireTime)
        {
            nextFireTime = Time.time + 1f / fireRate;
            Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
        }
    }
}