using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class Bullet : MonoBehaviour
{
    private Rigidbody2D rb;

    [SerializeField]
    private float speed = 20f;

    [SerializeField]
    private float lifetime = 1.5f;
    private bool launched = false;

    private Shooter shooter;

    public static event Action onSpiderKilled;
    public static event Action onHitBoss;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        shooter = FindObjectOfType<Shooter>();
    }

    public void Launch(Vector2 targetPosition)
    {
        Vector2 direction = (targetPosition - (Vector2)transform.position).normalized;

        rb.AddForce(direction * speed, ForceMode2D.Impulse);

        launched = true;
    }

    private void Update()
    {
        if (launched)
        {
            lifetime -= Time.deltaTime;
            if (lifetime <= 0)
            {
                // Put back in pool
                lifetime = 1.5f;
                launched = false;
                gameObject.SetActive(false);
                transform.position += Vector3.down * 200;
                shooter.pooledBullets.Add(gameObject);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            // Kill spider
            Destroy(collision.gameObject);
            onSpiderKilled?.Invoke();

            // Put back in pool
            lifetime = 1.5f;
            launched = false;
            gameObject.SetActive(false);
            transform.position += Vector3.down * 200;
            shooter.pooledBullets.Add(gameObject);
        }
        else if (collision.CompareTag("Boss"))
        {
            // Hurt boss spider
            onHitBoss?.Invoke();

            // Put back in pool
            lifetime = 1.5f;
            launched = false;
            gameObject.SetActive(false);
            transform.position += Vector3.down * 200;
            shooter.pooledBullets.Add(gameObject);
        }
    }
}