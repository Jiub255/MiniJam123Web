using System;
using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    public static event Action<string> onDeath;

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

        onDeath?.Invoke("Killed by spider");
    }
}