using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField]
    private float moveDuration = 0.5f;

    [SerializeField]
    private float chaseRadius = 10f;

    private bool moving;
    private Transform player;
    private Rigidbody2D rb;
    private Animator animator;

    public static event Action<string> onDeath;
    private bool playerAlive = true;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
       
        playerAlive = true;
    }

    private void Update()
    {
        float distance = Vector3.Distance(transform.position, player.position);

        if (!moving && distance > 0.5f && distance < chaseRadius)
        {
            List<Vector3> possibleDirections = new List<Vector3>();
            possibleDirections.Add(Vector3.zero);

            if ((transform.position.x - player.position.x) < -0.5f)
            {
                // transform.position.x < player.position.x
                possibleDirections.Add(Vector3.right);
            }
            else if ((transform.position.x - player.position.x) > 0.5f)
            {
                // transform.position.x > player.position.x
                possibleDirections.Add(Vector3.left);
            }

            if ((transform.position.y - player.position.y) < -0.5f)
            {
                // transform.position.y < player.position.y
                possibleDirections.Add(Vector3.up);
            }
            else if ((transform.position.y - player.position.y) > 0.5f)
            {
                // transform.position.y > player.position.y
                possibleDirections.Add(Vector3.down);
            }

            int randomInt = UnityEngine.Random.Range(0, possibleDirections.Count);

            StartCoroutine(MoveDirection(possibleDirections[randomInt]));
        }

        if (playerAlive)
            CheckForPlayer();
    }

    private void CheckForPlayer()
    {
        if (Mathf.Abs(player.position.y - transform.position.y) < 0.5f && 
            Mathf.Abs(player.position.x - transform.position.x) < 0.5f)
        {
            playerAlive = false;
            KillPlayer();
        }
    }

    private void KillPlayer()
    {
        Debug.Log("Killed by Spider");

        onDeath?.Invoke("Killed by Spider");
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