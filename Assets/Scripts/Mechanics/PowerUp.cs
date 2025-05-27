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

            switch
            (type)
            {
                case PickupType.Fish:
                    GameManager.Instance.Lives++;
                    Debug.Log("Lives = " + GameManager.Instance.Lives);
                    break;
                case PickupType.Football:

                    break;
                case PickupType.Thingy:

                    break;
            }

            Destroy(gameObject);
        }
    }
}
