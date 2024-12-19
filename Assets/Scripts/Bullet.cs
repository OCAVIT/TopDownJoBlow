using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f;
    public float lifeTime = 5f;
    public LayerMask collisionLayers;
    private TrailRenderer trailRenderer;

    void Start()
    {
        Destroy(gameObject, lifeTime);

        trailRenderer = gameObject.AddComponent<TrailRenderer>();
        trailRenderer.time = 0.1f;
        trailRenderer.startWidth = 0.1f;
        trailRenderer.endWidth = 0.0f;
        trailRenderer.material = new Material(Shader.Find("Sprites/Default"));
        trailRenderer.startColor = Color.red;
        trailRenderer.endColor = Color.yellow;
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

        if ((collisionLayers.value & (1 << other.gameObject.layer)) != 0)
        {
            Destroy(gameObject);
        }
    }
}