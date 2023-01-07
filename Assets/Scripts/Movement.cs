using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Movement : MonoBehaviour
{
	[SerializeField]
	private float moveDuration = 0.2f;
	private bool moving;
    private Rigidbody2D rb;
    [SerializeField]
    private LayerMask webLayer;
    private LevelGenerator levelGenerator;
    private HUDManager hudManager;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        levelGenerator = FindObjectOfType<LevelGenerator>();
        hudManager = FindObjectOfType<HUDManager>();

        // Start at midpoint on the bottom for now
        transform.position = new Vector3(Mathf.RoundToInt(levelGenerator.width / 2), 0, 0);
    }

    private void Update()
    {
        if (!moving)
        {
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            {
                StartCoroutine(MoveDirection(Vector3.up));
            }
            else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            {
                StartCoroutine(MoveDirection(Vector3.down));
            }
            else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            {
                StartCoroutine(MoveDirection(Vector3.left));
            }
            else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            {
                StartCoroutine(MoveDirection(Vector3.right));
            }
        }
    }

    private IEnumerator MoveDirection(Vector3 direction)
    {
        moving = true;

        float elapsedTime = 0f;

        Vector3 originalPosition = transform.position;
        Vector3 targetPosition = originalPosition + direction;

        while (elapsedTime < moveDuration)
        {
            rb.MovePosition(Vector3.Lerp(originalPosition, targetPosition, (elapsedTime / moveDuration)));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPosition;

        moving = false;

        // Did you just run through a sticky web?
        RaycastHit2D hit = Physics2D.Raycast(transform.position, -direction, 1f, webLayer); 
        if (hit.collider == null)
        {
            // Die from sticky web/getting eaten by spiders.
            Debug.Log("Killed by web");
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        if (transform.position.y >= levelGenerator.height)
        {
            Win();
        }
    }

    private void Win()
    {
        Debug.Log("WIN!! Score: " + Mathf.RoundToInt(100 * (hudManager.spidersKilled + 1) / hudManager.timer));
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}