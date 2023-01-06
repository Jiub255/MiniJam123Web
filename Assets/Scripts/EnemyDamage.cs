using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyDamage : MonoBehaviour
{
/*    private Transform player;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }*/

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            KillPlayer();
        }
    }

    private void KillPlayer()
    {
        Debug.Log("Killed by spider");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }



/*    public void CheckForPlayer()
    {
        if (Vector3.Distance(transform.position, player.position) < 0.5f)
        {
            KillPlayer();
        }
    }*/
}