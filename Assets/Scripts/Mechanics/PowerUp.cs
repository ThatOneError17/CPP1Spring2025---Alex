using UnityEngine;

//pickup class to handle all the pickup items in the game

public class PowerUp : MonoBehaviour
{
    public AudioClip pickupSound;
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
            if (pickupSound)
                GetComponent<AudioSource>().PlayOneShot(pickupSound); //Plays pickupsound corresponding to each object

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

            Physics2D.IgnoreCollision(collision, GetComponent<Collider2D>());
            GetComponent<SpriteRenderer>().enabled = false;

            if (pickupSound)
                Destroy(gameObject, pickupSound.length);

            else
                Destroy(gameObject);
        }
    }
}
