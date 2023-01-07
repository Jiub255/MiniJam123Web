using UnityEngine;

public class EnemyShoot : MonoBehaviour
{
    private Transform player;

    [SerializeField]
    private float shootTimerLength = 2f;
    private float timer = 0f;

    [SerializeField]
    private float shootInnerRadius = 5f;
    [SerializeField]
    private float shootOuterRadius = 9f;

    [SerializeField]
    private GameObject venomPrefab;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;

        timer = shootTimerLength;
    }

    private void Update()
    {
        float distance = Vector3.Distance(transform.position, player.position);

        timer += Time.deltaTime;

        if (distance > shootInnerRadius && distance < shootOuterRadius && timer > shootTimerLength)
        {
            timer = 0f;
            Shoot();
        }
    }

    private void Shoot()
    {
        GameObject venom = Instantiate(venomPrefab);
        venom.transform.position = transform.position;
        venom.gameObject.GetComponent<Venom>().Launch(player.position);
    }
}