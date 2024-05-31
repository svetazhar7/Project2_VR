using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBlueBird : Enemy


{
    private bool ciellingDetected;

    [Header("- BlueBird Specifics")]
    [SerializeField] private float distanceToGroundBelow;
    [SerializeField] private float ciellingDistance;
    [SerializeField] private float flyUpForce;
    [SerializeField] private float flyDownForce;
                     private float flyForce;

    protected override void Start()
    {
        base.Start();
    }

    private void Update()
    {
        CollisionChecks();
        if (ciellingDetected)
        {
            flyForce = flyDownForce;
        }
        else if (groundDetected)
        {
            flyForce = flyUpForce;
        }

        if (wallDetected)
        {
            Flip();
        }
    }

    public void FlyUpEvent()
    {
        rb.velocity = new Vector2(speed * facingDirection,flyForce);
    }

    public override void Damaged()
    {
        base.Damaged();
        rb.velocity = new Vector2(0, 0);
        rb.gravityScale = 0;
    }

    protected override void CollisionChecks()
    {
        base.CollisionChecks();
        ciellingDetected = Physics2D.Raycast(transform.position, Vector2.up, ciellingDistance, whatIsGround);

    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

    }

    protected override void OnXAxisMove()
    {
        base.OnXAxisMove();
    }

    protected override void OnYAxisMove()
    {
        base.OnYAxisMove();
    }



    protected override void Wander()
    {
        base.Wander();
    }
}
