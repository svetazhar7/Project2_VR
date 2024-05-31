using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    protected Rigidbody2D rb;
    protected Animator anim;
    
    [Header("- Detectors")]
    [SerializeField] protected LayerMask whatToIgnore;
    [SerializeField] protected LayerMask whatIsGround;
    [SerializeField] protected float groundCheckDistance;
    [SerializeField] protected float wallCheckDistance;
    [SerializeField] protected Transform groundCheck;
    [SerializeField] protected Transform wallCheck;
    protected bool wallDetected;
    protected bool groundDetected;

    [Header("- Movement")]
    [SerializeField] protected int facingDirection = 1;
    [SerializeField] protected float speed;
    [SerializeField] protected float idleTime = 2;
                     protected float idleTimeTimer;
    protected bool canMove = true;


    protected bool isMovingOnX;
    protected bool isMovingOnY;
    [HideInInspector] public bool invincible;
    protected bool aggresive;

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        FreezOnZ();
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerController>() != null)
        {
            PlayerController player = collision.GetComponent<PlayerController>();
            player.KnockBack(transform);
        }
    }

    public virtual void Damaged()
    {
        if (!invincible)
        {
            canMove = false;
            anim.SetTrigger("isHitted");

        }
    }

    protected virtual void Flip()
    {
        facingDirection = facingDirection * -1;
        transform.Rotate(0, 180, 0);
    }

    protected virtual void CollisionChecks()
    {
        groundDetected = Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround);
        wallDetected = Physics2D.Raycast(wallCheck.position, Vector2.right * facingDirection, wallCheckDistance, whatIsGround);
    }

    protected virtual void OnDrawGizmos()
    {
        if (groundCheck != null)
            Gizmos.DrawLine(groundCheck.position, new Vector2(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));
        if (wallCheck != null)
            Gizmos.DrawLine(wallCheck.position, new Vector2(wallCheck.position.x + wallCheckDistance * facingDirection, wallCheck.position.y));

    }

    public void DestroyMe()
    {
        Destroy(gameObject);
    }
    

    protected virtual void FreezOnZ()
    {
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    protected virtual void CheckAxistMovement()
    {
        isMovingOnX = rb.velocity.x != 0;
        isMovingOnY = rb.velocity.y != 0;

    }

    protected virtual void Wander()
    {
        if (idleTimeTimer < 0 && canMove)
            rb.velocity = new Vector2(speed * facingDirection, rb.velocity.y);
        else
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
        }


        idleTimeTimer -= Time.deltaTime;

        if (wallDetected || !groundDetected)
        {
            idleTimeTimer = idleTime;
            Flip();
        }
    }
    protected virtual void OnXAxisMove(){}

    protected virtual void OnYAxisMove(){}
}
