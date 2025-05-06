using UnityEngine;

//pickup class to handle all the pickup items in the game

public class PowerUp : MonoBehaviour
{
    public enum PickupType
    {
        Fish,
        Football,
        Thingy,
    }

    public PickupType type;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
           Destroy(gameObject);
        }
    }
}
