using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class Bullet : MonoBehaviour
{
    private Rigidbody2D rb;

    [SerializeField]
    private float speed = 50f;

    [SerializeField]
    private float lifetime = 1.5f;
    private bool launched = false;

    private Shooter shooter;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        shooter = FindObjectOfType<Shooter>();
    }

    public void Launch(Vector2 targetPosition)
    {
        //Debug.Log("Launch to " + targetPosition);

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

            // Put back in pool
            lifetime = 1.5f;
            launched = false;
            gameObject.SetActive(false);
            transform.position += Vector3.down * 200;
            shooter.pooledBullets.Add(gameObject);
        }
    }
}