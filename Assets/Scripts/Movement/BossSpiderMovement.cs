using System;
using System.Collections;
using UnityEngine;

// Maybe have a giant spider face as wide as the screen chase you downwards? Kinda contra-like.
public class BossSpiderMovement : MonoBehaviour
{
    [SerializeField]
    private float moveDuration = 1f;

    [SerializeField]
    private float chaseRadius = 20f;

    private bool moving = false;
    private Transform player;
    private Rigidbody2D rb;
    private Animator animator;
    private LevelGenerator levelGenerator;

    public static event Action<string> onDeath;
    private bool playerAlive = true;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        levelGenerator = FindObjectOfType<LevelGenerator>();
        chaseRadius = levelGenerator.height;

        playerAlive = true;
    }

    private void Update()
    {
        float distance = Vector3.Distance(transform.position, player.position);
        
        if (!moving && distance < chaseRadius)
        {
            StartCoroutine(MoveDirection(Vector3.down));
        }

        if (playerAlive)
            CheckForPlayer();
    }

    private void CheckForPlayer()
    {
        if (player.position.y > transform.position.y - 6.5f &&
            player.position.y < transform.position.y + 2f)
        {
            playerAlive = false;
            KillPlayer();
        }
    }

    private void KillPlayer()
    {
        Debug.Log("Killed by Boss Spider");

        onDeath?.Invoke("Killed by Boss Spider");
    }

    private IEnumerator MoveDirection(Vector3 direction)
    {
        moving = true;
        if (direction != Vector3.zero)
            animator.SetBool("Moving", true);

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
        if (direction != Vector3.zero)
            animator.SetBool("Moving", false);
    }
}