using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyDamage))]
public class EnemyMovement : MonoBehaviour
{
    [SerializeField]
    private float moveDuration = 0.5f;

    [SerializeField]
    private float chaseRadius = 10f;

    private bool moving;
    private Transform player;
  //  private EnemyDamage enemyDamage;
    private Rigidbody2D rb;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
      //  enemyDamage = GetComponent<EnemyDamage>();
        rb = GetComponent<Rigidbody2D>();
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

            int randomInt = Random.Range(0, possibleDirections.Count);

            StartCoroutine(MoveDirection(possibleDirections[randomInt]));
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
            //transform.position = Vector3.Lerp(originalPosition, targetPosition, (elapsedTime / moveDuration));
            rb.MovePosition(Vector3.Lerp(originalPosition, targetPosition, (elapsedTime / moveDuration)));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPosition;

        moving = false;

       // enemyDamage.CheckForPlayer();
    }
}