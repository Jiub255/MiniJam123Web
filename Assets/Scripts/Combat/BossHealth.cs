using System;
using UnityEngine;

public class BossHealth : MonoBehaviour
{
    public int maxHealth = 10;
	public int health;

    public static event Action onWin;

    private void Awake()
    {
        health = maxHealth;
    }

    private void OnEnable()
    {
        Bullet.onHitBoss += GetHurt;
    }

    private void OnDisable()
    {
        Bullet.onHitBoss -= GetHurt;
    }

    private void GetHurt()
    {
        health--;
        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        onWin?.Invoke();
    }
}