using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Radish : Enemy
{
    private bool groundBelowDetected;
    private bool groundAboveDetected;

    [Header("- Radish Specifics")]
    [SerializeField] private float distanceToGroundBelow;
    [SerializeField] private float ciellingDistance;
    [SerializeField] private float aggresiveTime = 5.0f;
    [SerializeField] private float aggresiveTimeTimer;
    
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    private void Update()
    {
        OnXAxisMove();
        CheckAxistMovement();
        RadishStateManager();
        CollisionChecks();
        AnimatorCotroller();
        manageAgrroState();
    }

    private void RadishStateManager()
    {
        if (!aggresive) {
            Float();
        } else
        {
            if (groundDetected)
            {
                Wander();
                invincible = false;
            }
        }
    }

    private void Float()
    {
        if (groundBelowDetected && !groundAboveDetected) {
            rb.velocity = new Vector2(0, 1);
        }
    }
    private void AnimatorCotroller()
    {
        anim.SetFloat("xVelocity", rb.velocity.x);
        anim.SetBool("aggresive", aggresive);
    }

    private void manageAgrroState()
    {
        if (aggresive)
        {
            aggresiveTimeTimer -= Time.deltaTime;

            if (aggresiveTimeTimer < 0)
            {
                aggresive = false;
            }
        }
    }

    protected override void CollisionChecks()
    {
        base.CollisionChecks();
        groundAboveDetected = Physics2D.Raycast(transform.position, Vector2.up, ciellingDistance, whatIsGround);
        groundBelowDetected = Physics2D.Raycast(transform.position, Vector2.down, distanceToGroundBelow, whatIsGround);
    }



    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x,transform.position.y + ciellingDistance));
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x, transform.position.y - distanceToGroundBelow));

    }

    public override void Damaged()
    {
        if (!aggresive)
        {
            invincible = true;
            aggresive = true;
            rb.velocity = new Vector2(0, -5);
            aggresiveTimeTimer = aggresiveTime;
        }
        else
        {
            base.Damaged();
        }
    }
}
