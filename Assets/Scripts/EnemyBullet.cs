using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public float speed = 10f;
    public float lifeTime = 5f;
    public LayerMask collisionLayers;
    private TrailRenderer trailRenderer;
    public float damage = 10;

    void Start()
    {
        Destroy(gameObject, lifeTime);

        trailRenderer = gameObject.AddComponent<TrailRenderer>();
        trailRenderer.time = 0.1f;
        trailRenderer.startWidth = 0.3f;
        trailRenderer.endWidth = 0.0f;
        trailRenderer.material = new Material(Shader.Find("Sprites/Default"));
        trailRenderer.startColor = new Color(0.3f, 0.15f, 0.0f); // Более тусклый коричневый цвет
        trailRenderer.endColor = new Color(0.5f, 0.45f, 0.0f); // Более тусклый желтый цвет

        // Убедитесь, что пуля направлена вперед
        //transform.rotation = Quaternion.LookRotation(Vector3.forward);
    }

    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("BulletDestroyer"))
        {
            Destroy(gameObject);
            return;
        }
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerHealth>().GetDamage(damage);
            Destroy(gameObject) ;   
            return;
        }

        if ((collisionLayers.value & (1 << other.gameObject.layer)) != 0)
        {
            Destroy(gameObject);
        }
    }
}