using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossShoot : MonoBehaviour
{
    private Transform player;

    [SerializeField]
    private float shootTimerLength = 3f;
    private float timer = 0f;

    [SerializeField]
    private float shootOuterRadius = 9f;

    [SerializeField]
    private GameObject bossVenomPrefab;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;

        timer = shootTimerLength;
    }

    private void Update()
    {
        float distance = Vector3.Distance(transform.position, player.position);

        timer += Time.deltaTime;

        if (distance > 0.5f && distance < shootOuterRadius && timer > shootTimerLength)
        {
            timer = 0f;
            Shoot();
        }
    }

    private void Shoot()
    {
        GameObject bossVenom = Instantiate(bossVenomPrefab);
        bossVenom.transform.position = transform.position;
        bossVenom.gameObject.GetComponent<Venom>().Launch(player.position);
    }
}