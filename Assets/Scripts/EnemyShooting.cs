using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public class EnemyAI : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float shootingInterval = 2f;
    public float detectionRange = 10f;
    public float stopDistance = 2f;
    public float rotationSpeed = 5f;
    public Vector3 forwardDirection = Vector3.forward;
    public float memoryDuration = 5f;
    public float chaseDuration = 3f;

    private Transform player;
    private float timeSinceLastShot;
    private NavMeshAgent navMeshAgent;
    private Vector3 lastKnownPlayerPosition;
    private float timeSinceLastSeenPlayer;
    private float timeSinceLostPlayer;
    private Queue<Vector3> playerPositions = new Queue<Vector3>();
    private bool hasSeenPlayer = false;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        navMeshAgent = GetComponent<NavMeshAgent>();
        timeSinceLastShot = shootingInterval;
        navMeshAgent.isStopped = true;
    }

    void Update()
    {
        timeSinceLastShot += Time.deltaTime;

        if (CanSeePlayer())
        {
            hasSeenPlayer = true;
            lastKnownPlayerPosition = player.position;
            timeSinceLastSeenPlayer = 0f;
            timeSinceLostPlayer = 0f;
            playerPositions.Enqueue(player.position);
            if (playerPositions.Count > 10)
            {
                playerPositions.Dequeue();
            }

            RotateTowards(player.position);

            if (timeSinceLastShot >= shootingInterval)
            {
                Shoot();
                timeSinceLastShot = 0f;
            }

            if (Vector3.Distance(transform.position, player.position) > stopDistance)
            {
                navMeshAgent.isStopped = false;
                navMeshAgent.SetDestination(player.position);
            }
            else
            {
                navMeshAgent.isStopped = true;
            }
        }
        else if (hasSeenPlayer)
        {
            timeSinceLastSeenPlayer += Time.deltaTime;
            timeSinceLostPlayer += Time.deltaTime;

            if (timeSinceLostPlayer < chaseDuration)
            {
                navMeshAgent.isStopped = false;
                navMeshAgent.SetDestination(lastKnownPlayerPosition);
            }
            else
            {
                navMeshAgent.isStopped = true;
            }

            if (timeSinceLastSeenPlayer < memoryDuration && !navMeshAgent.isStopped)
            {
                navMeshAgent.SetDestination(lastKnownPlayerPosition);
            }
            else if (playerPositions.Count > 0 && !navMeshAgent.isStopped)
            {
                navMeshAgent.SetDestination(playerPositions.Dequeue());
            }
        }
    }

    bool CanSeePlayer()
    {
        if (player == null)
            return false;

        Vector3 directionToPlayer = player.position - transform.position;
        if (directionToPlayer.magnitude > detectionRange)
            return false;

        RaycastHit hit;
        if (Physics.Raycast(transform.position, directionToPlayer, out hit, detectionRange))
        {
            if (hit.transform.CompareTag("Player"))
            {
                return true;
            }
        }

        return false;
    }

    void RotateTowards(Vector3 targetPosition)
    {
        Vector3 direction = (targetPosition - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction, Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
    }

    void Shoot()
    {
        Vector3 directionToPlayer = (player.position - firePoint.position).normalized;
        Instantiate(bulletPrefab, firePoint.position, Quaternion.LookRotation(directionToPlayer));
    }
}