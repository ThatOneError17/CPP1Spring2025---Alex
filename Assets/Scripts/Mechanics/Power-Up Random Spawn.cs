using Unity.VisualScripting;
using UnityEngine;
using static PowerUp;

public class FootballRandomSpawn : MonoBehaviour
{
    public GameObject[] PowerUpPreFabs;

    private void Start()
    {

        spawnPowerUps();

    }

    void spawnPowerUps()
    {
      
        int rand = Random.Range(0, PowerUpPreFabs.Length);
        Instantiate(PowerUpPreFabs[rand], transform.position, transform.rotation);
        
    }
}
