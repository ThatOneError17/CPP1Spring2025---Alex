using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]

public class Projectile : MonoBehaviour
{
    public LayerMask isGroundLayer;

    [SerializeField] private ProjectileType type;
    [SerializeField, Range(1, 20)] private float lifetime = 1.0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() => Destroy(gameObject, lifetime);
    
    public void SetVelocity(Vector2 velocity) => GetComponent<Rigidbody2D>().linearVelocity = velocity;
        

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            Destroy(gameObject);
        }

        if (type == ProjectileType.Player)
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(10);
                Destroy(gameObject);
            }
        }

        if (type == ProjectileType.Enemy)
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            if (collision.gameObject.CompareTag("Player"))
            {
                //GameManager.Instance.Lives--
                Destroy(gameObject);
            }
        }
    }


}

public enum ProjectileType
{
    Player,
    Enemy,
}
