using System;
using System.Collections;
using UnityEngine;

public class Movement : MonoBehaviour
{
	[SerializeField]
	private float moveDuration = 0.2f;
	private bool moving;
    private Rigidbody2D rb;
    [SerializeField]
    private LayerMask webLayer;
    private LevelGenerator levelGenerator;
    private Animator animator;

    public static event Action onBossLevel;
    public static event Action<string> onDeath;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        levelGenerator = FindObjectOfType<LevelGenerator>();
        animator = GetComponentInChildren<Animator>();

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
               // if (animator.GetFloat("LastHoriz") > 0.5f || animator.GetFloat("LastHoriz") < -0.5f)
                    animator.SetFloat("LastHoriz", 0);
               // if (animator.GetFloat("LastVert") < 0.5f)
                    animator.SetFloat("LastVert", 1);
            }
            else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            {
                StartCoroutine(MoveDirection(Vector3.down));
               // if (animator.GetFloat("LastHoriz") > 0.5f || animator.GetFloat("LastHoriz") < -0.5f)
                    animator.SetFloat("LastHoriz", 0);
               // if (animator.GetFloat("LastVert") > -0.5f)
                    animator.SetFloat("LastVert", -1);
            }
            else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            {
                StartCoroutine(MoveDirection(Vector3.left));
               // if (animator.GetFloat("LastHoriz") > -0.5f)
                    animator.SetFloat("LastHoriz", -1);
               // if (animator.GetFloat("LastVert") > 0.5f || animator.GetFloat("LastVert") < -0.5f)
                    animator.SetFloat("LastVert", 0);
            }
            else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            {
                StartCoroutine(MoveDirection(Vector3.right));
                //if (animator.GetFloat("LastHoriz") < 0.5f)
                    animator.SetFloat("LastHoriz", 1);
               // if (animator.GetFloat("LastVert") > 0.5f || animator.GetFloat("LastVert") < -0.5f)
                    animator.SetFloat("LastVert", 0);
            }
            else
            {
                animator.SetBool("Moving", false);
            }
        }
    }

    private IEnumerator MoveDirection(Vector3 direction)
    {
        animator.SetBool("Moving", true);
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

            onDeath?.Invoke("Killed by web");
        }

        if (transform.position.y >= levelGenerator.height)
        {
            GoToBossLevel();
        }
    }

    private void GoToBossLevel()
    {
        // Warp to top of web
        transform.position = new Vector3(
            Mathf.RoundToInt(levelGenerator.width / 2),
            (-3 * levelGenerator.height) - 5,
            0);

        // turns on boss health hud in hudManager
        onBossLevel?.Invoke();
    }
}