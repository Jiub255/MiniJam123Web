using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Movement : MonoBehaviour
{
	[SerializeField]
	private float moveDuration = 0.2f;
	private bool moving;

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
            transform.position = Vector3.Lerp(originalPosition, targetPosition, (elapsedTime/moveDuration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPosition;

        moving = false;

        // Did you just run through a sticky web?
        RaycastHit2D hit = Physics2D.Raycast(transform.position, -direction, 1f); 
        if (hit.collider == null)
        {
            // Die from sticky web/getting eaten by spiders.
            Debug.Log("Killed by web");
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}