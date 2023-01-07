using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    [SerializeField]
    private GameObject bulletPrefab;

    [SerializeField]
    private int numberToPool = 25;

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
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        Debug.Log("Shoot");

        GameObject bullet;

        if (pooledBullets.Count > 0)
        {
            Debug.Log("Pulled bullet from pool");
            bullet = pooledBullets[0];
            pooledBullets.RemoveAt(0);
            bullet.SetActive(true);
            bullet.transform.position = transform.position;
        }
        else
        {
            Debug.Log("Instantiated bullet");
            bullet = Instantiate(bulletPrefab);
            bullet.transform.position = transform.position;
        }

        // Shoot bullet. Deactivates itself onCollision or after timer and re-adds itself to pool.
        bullet.GetComponent<Bullet>().Launch(Camera.main.ScreenToWorldPoint(Input.mousePosition));
    }
}