using UnityEngine;
[RequireComponent(typeof(Rigidbody2D), typeof(Animator))]

public class WalkerEnemy : Enemy
{
    private Rigidbody2D rb;
    [SerializeField] private float xVel = 0;
    //Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody2D>();
        rb.sleepMode = RigidbodySleepMode2D.NeverSleep;

        if (xVel <= 0) xVel = 3f;
    }

    public override void TakeDamage(int damageValue, DamageType damageType = DamageType.Default)
    {
        if (damageType == DamageType.JumpedOn)
        {
            anim.SetTrigger("Squish");
            Destroy(transform.parent.gameObject, 0.5f);
            return;
        }

        base.TakeDamage(damageValue, damageType);

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Barrier"))
        {
            anim.SetTrigger("Turn");
            sr.flipX = !sr.flipX;
        }
    }

    // Update is called once per frame
    private void Update()
    {
        AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);

        if (stateInfo.IsName("KoopaWalk"))
        {
            if (sr.flipX) rb.linearVelocity = new Vector2(-xVel, rb.linearVelocity.y);
            else rb.linearVelocity = new Vector2(xVel, rb.linearVelocity.y);
        }
    }
}
