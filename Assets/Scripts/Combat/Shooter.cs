using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    [SerializeField]
    private float shootTimerLength = 1f;
    private float timer = 1.01f;

    [SerializeField]
    private GameObject bulletPrefab;

    [SerializeField]
    private int numberToPool = 10;

    public List<GameObject> pooledBullets = new List<GameObject>();

    private void Start()
    {
        for (int i = 0; i < numberToPool; i++)
        {
            GameObject instance = Instantiate(bulletPrefab);
            instance.transform.position = new Vector3(500, i, 0);
            instance.SetActive(false);
            pooledBullets.Add(instance);
        }
    }

    private void Update()
    {  
        timer += Time.deltaTime;

        if (Input.GetKey(KeyCode.Mouse0) && timer > shootTimerLength)
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        timer = 0f;

        GameObject bullet;

        if (pooledBullets.Count > 0)
        {
            bullet = pooledBullets[0];
            pooledBullets.RemoveAt(0);
            bullet.SetActive(true);
            bullet.transform.position = transform.position;
        }
        else
        {
            bullet = Instantiate(bulletPrefab);
            bullet.transform.position = transform.position;
        }

        // Shoot bullet. Deactivates itself onCollision or after timer and re-adds itself to pool.
        bullet.GetComponent<Bullet>().Launch(Camera.main.ScreenToWorldPoint(Input.mousePosition));
    }
}