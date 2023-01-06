using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyShoot : MonoBehaviour
{
    private Transform player;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }



    private void KillPlayer()
    {
        Debug.Log("Killed by spider venom");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}