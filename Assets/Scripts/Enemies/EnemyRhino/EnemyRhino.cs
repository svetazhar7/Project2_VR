using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRhino : Enemy
{
       
    [SerializeField] float agroSpeed = 8;
    [SerializeField] float stunTime;
                     private float stunTimeTimer;

    private RaycastHit2D playerDetection;

    protected override void Start()
    {
        base.Start();
        invincible = true;
    }

    private void Update()
    {
        OnXAxisMove();
        CollisionChecks();
        AnimatorCotroller();
        CheckAxistMovement();
        RhinoStateManager();

    }

    private void RhinoStateManager()
    {
        playerDetection = Physics2D.Raycast(wallCheck.position, Vector2.right * facingDirection, 35, ~whatToIgnore);
        if (playerDetection.collider.GetComponent<PlayerController>())
            aggresive = true;
        if (!aggresive)
            Wander();
        else
            OnAgrro();
    }

    private void OnAgrro()
    {
        rb.velocity = new Vector2((agroSpeed) * facingDirection, rb.velocity.y);
        if (wallDetected && invincible)
        {
            invincible = false;
            stunTimeTimer = stunTime;
        }

        if (stunTimeTimer <= 0 && !invincible)
        {
            invincible = true;
            Flip();
            aggresive = false;
        }
        stunTimeTimer -= Time.deltaTime;
    }

    private void AnimatorCotroller()
    {
        anim.SetFloat("xVelocity", rb.velocity.x);
        anim.SetBool("invincible", invincible);
    }

    protected override void OnXAxisMove()
    {
        base.OnXAxisMove();
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.DrawLine(wallCheck.position, new Vector2(wallCheck.position.x + playerDetection.distance * facingDirection, wallCheck.position.y));
    }
}
