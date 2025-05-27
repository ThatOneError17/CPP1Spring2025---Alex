using UnityEngine;

[RequireComponent(typeof(Shoot))]
public class Kamek : Enemy
{
    [SerializeField] private float projectileFireRate = 2f;
    private float timeSinceLastFire = 0;
    public Transform player;

    [Range(1, 20)]
    public int range;
    
    private void Awake()
    {
        GameManager.Instance.OnPlayerControllerCreated += SetPlayerRef;
    }

    private PlayerController SetPlayerRef(PlayerController playerInstance)
    {
        player = playerInstance.transform;
        return playerInstance;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();

        if (projectileFireRate <= 0) 
            projectileFireRate = 2.0f;
    }

    // Update is called once per frame
    void Update()
    {
        AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);

        if (stateInfo.IsName("KamekIdle"))
            CheckFire();

        if (player.position.x > transform.position.x && player.position.x <= transform.position.x + range)
        {
            sr.flipX = false;
            //Debug.Log("Player is to the right of Kamek and is within range");
            anim.SetBool("InRange", true);
        }
        else if (player.position.x < transform.position.x && player.position.x >= transform.position.x - range)
        {
            sr.flipX = true;
            //Debug.Log("Player is to the left of Kamek and is within range");
            anim.SetBool("InRange", true);
        }
        else
        {
            //Debug.Log("Player is out of range");
            anim.SetBool("InRange", false);
        }


    }

    void CheckFire()
    {
        if (Time.time >= timeSinceLastFire + projectileFireRate)
        {
            anim.SetTrigger("Fire");
            timeSinceLastFire = Time.time;
            
        }
    }

  

}
