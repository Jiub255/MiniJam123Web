using UnityEngine;
using UnityEngine.SceneManagement;

public class Venom : MonoBehaviour
{
	// Shot by spiders. Either kills or temporarily freezes player.

	[SerializeField]
	private float speed = 10f;

    [SerializeField]
    private float lifetime = 3f;

    private Rigidbody2D rb;
    private bool launched = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (launched)
        {
            lifetime -= Time.deltaTime;
            if (lifetime <= 0)
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            KillPlayer();
        }
    }

    private void KillPlayer()
    {
        Debug.Log("Killed by venom");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Launch(Vector2 targetPosition)
    {
        //Debug.Log("Launch to " + targetPosition);

        Vector2 direction = (targetPosition - (Vector2)transform.position).normalized;

        rb.AddForce(direction * speed, ForceMode2D.Impulse);

        launched = true;
    }
}