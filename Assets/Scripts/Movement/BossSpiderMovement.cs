using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Maybe have a giant spider face as wide as the screen chase you downwards? Kinda contra-like.
public class BossSpiderMovement : MonoBehaviour
{
    [SerializeField]
    private float moveDuration = 0.3f;

    [SerializeField]
    private float chaseRadius = 20f;

    private bool moving = false;
    private Transform player;
    private Rigidbody2D rb;
    private LevelGenerator levelGenerator;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
        levelGenerator = FindObjectOfType<LevelGenerator>();
        chaseRadius = levelGenerator.height;
        Debug.Log("chaseRadius == " + chaseRadius.ToString());
    }

    private void Update()
    {
        float distance = Vector3.Distance(transform.position, player.position);
        
        if (!moving && distance < chaseRadius)
        {

        }

/*        
        
        if (!moving && distance > 0.5f && distance < chaseRadius)
        {
            List<Vector2> possibleDirections = new List<Vector2>();
            possibleDirections.Add(Vector2.zero);
            possibleDirections.Add(Vector2.down);

            if (transform.position.x >= player.position.x)
            {
                possibleDirections.Add(Vector2.right);
            }
            else if (transform.position.x <= player.position.x)
            {
                possibleDirections.Add(Vector2.left);
            }

            int randomInt = Random.Range(0, possibleDirections.Count);
            StartCoroutine(MoveDirection(possibleDirections[randomInt]));
        }*/
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
    }
}