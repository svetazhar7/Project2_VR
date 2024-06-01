using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using TMPro; 
using UnityEngine.SceneManagement; 
public class PlayerController : MonoBehaviour
{
    // Components
    [SerializeField] private GameObject livesPanel;
    private Animator anim;
    private Rigidbody2D rb;
    private SceneTransition sceneTransition; // Ссылка на компонент SceneTransition
    [Header("- Move Info")]
    [SerializeField] float moveSpeed = 10f;
    private bool canDoubleJump;
    private bool canMove = true;

    [Header("- Jump")]
    [SerializeField] float jumpForce = 15f;
    [SerializeField] float doubleJumpForce = 13f;
    [SerializeField] Vector2 jumpDirection = new Vector2(5, 15);
    [SerializeField] private float jumpBufferTime;
    private float jumpBufferTimer;

    [Header("- Cayote Jump")]
    [SerializeField] private float cayoteJumpTime;
    private float cayoteJumpTimer;
    private bool canHaveCayoteJump;

    [Header("- Collision Info")]
    // Wall
    [SerializeField] private LayerMask whatIsWall;
    [SerializeField] private float wallCheckDistance;
    private bool isOnWall;
    private bool canWallSlide;
    private bool isWallSliding;
    // Ground
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private float groundCheckDistance;
    private bool isGrounded;
    // Enemy
    [SerializeField] private Transform enemyCheck;
    [SerializeField] private float enemyCheckRadius;

    [Header("- Facing Direction")]
    private bool facingRight = true;
    private int facingDirection = 1;

    [Header("- Knockback")]
    [SerializeField] private Vector2 knockbackDirection;
    [SerializeField] private float invincibilityDuration = 0.6f;
    private bool isKnocked;
    private bool canBeKnocked = true;

    [Header("- Lives")]
    [SerializeField] private int maxLives = 5;
    public int currentLives;
    [SerializeField] private TextMeshProUGUI livesText; // TextMeshProUGUI для отображения жизней

    void Start()
    {
        if (PlayerPrefs.HasKey("lives") && SceneManager.GetActiveScene().name != "Level")
        {
            currentLives = PlayerPrefs.GetInt("lives");
        }
        else
        {
            currentLives = maxLives;
        }
               sceneTransition = FindObjectOfType<SceneTransition>();
        SetupInitialComponents();
        SetupInitialSettings();
        
        UpdateLivesUI();
    }

    void Update()
    {
        AnimationController();

        if (isKnocked)
        {
            return;
        }

        FlipController();
        CollisionChecks();
        InputChecks();
        DetectEnemies();

        jumpBufferTimer -= Time.deltaTime;
        cayoteJumpTimer -= Time.deltaTime;

        if (isGrounded)
        {
            canDoubleJump = true;
            canMove = true;

            if (jumpBufferTimer > 0)
            {
                jumpBufferTimer = -1;
                Jump(jumpForce);
            }
            canHaveCayoteJump = true;
        }
        else
        {
            if (canHaveCayoteJump)
            {
                canHaveCayoteJump = false;
                cayoteJumpTimer = cayoteJumpTime;
            }
        }

        if (canWallSlide)
        {
            isWallSliding = true;
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.2f);
        }

        HandleMove();
    }

    private void AnimationController()
    {
        anim.SetBool("isMoving", GetXVelocityAxis() != 0);
        anim.SetFloat("yVelocity", rb.velocity.y);
        anim.SetBool("isGrounded", isGrounded);
        anim.SetBool("isWallSliding", isWallSliding);
        anim.SetBool("isKnocked", isKnocked);
    }

    private float GetXVelocityAxis(){
        return Input.GetAxisRaw("Horizontal");
    }

    private void InputChecks() {
        if (Input.GetKeyDown(KeyCode.R)) {
            Flip();
        }
        if (Input.GetKeyDown(KeyCode.Space)){
            OnJump();
        }
    }

    private void WallJump()
    {
        rb.velocity = new Vector2(jumpDirection.x * -facingDirection, jumpDirection.y);
        canDoubleJump = true;
    }

    private void HandleMove()
    {
        if (canMove)
        {
            rb.velocity = new Vector2(moveSpeed * GetXVelocityAxis(), rb.velocity.y);
        }
    }

    private void Jump(float force){
        rb.velocity = new Vector2(rb.velocity.x,force);
    }

    private void FlipController()
    {
        if (facingRight && rb.velocity.x < 0) {
            Flip();
        } else if (!facingRight && rb.velocity.x > 0) {
            Flip();
        }
    }

    private void Flip()
    {
        facingDirection *= -1;
        facingRight = !facingRight;
        transform.Rotate(0,180,0);
    }

    private void OnJump(){ 
        if (!isGrounded)
            jumpBufferTimer = jumpBufferTime;

        if (isWallSliding)
        {
            WallJump();
            canDoubleJump = true;
        }
        else if (isGrounded || cayoteJumpTimer > 0)
        {
            Jump(jumpForce);
        }
        else if(canDoubleJump) {
            canMove = true;
            canDoubleJump = false;
            Jump(doubleJumpForce);
        }

        canWallSlide = false;
    }

    public void KnockBack(Transform damageTransform)
    {   
        if (!canBeKnocked)
        {
            return;
        }
        canBeKnocked = false;

        isKnocked = true;

        int hDirection = 0;

        #region Define horizontal direction for knockback
        if (transform.position.x > damageTransform.position.x)
        {
            hDirection = 1;
        }
        else if (transform.position.x < damageTransform.position.x)
        {
            hDirection = -1;
        }
        #endregion

        rb.velocity = new Vector2(knockbackDirection.x * hDirection, knockbackDirection.y);
        
        Invoke("CancelKnockback", 0.5f);
        Invoke("AllowKnockback", invincibilityDuration);

        // Уменьшаем количество жизней
        LoseLife();
    }

    private void LoseLife()
    {
        currentLives--;
        UpdateLivesUI();
        StartCoroutine(FlashRed());

        if (currentLives <= 0)
        {
           livesPanel.SetActive(false);
            sceneTransition.LoadScene("Lose");
        }
    }
   private IEnumerator FlashRed()
{
    Color originalColor = livesText.color;
    livesText.color = Color.red;
    yield return new WaitForSeconds(0.25f);
    livesText.color = originalColor;
}

    private void UpdateLivesUI()
    {
        if (livesText != null)
        {
            livesText.text = currentLives.ToString();
        }
    }

    private void CancelKnockback()
    {
        isKnocked = false;
    }

    private void AllowKnockback()
    {
        canBeKnocked = true;
    }

    private void CollisionChecks(){
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down,groundCheckDistance,whatIsGround);
        if (isGrounded)
        {
            canMove = true;
            canWallSlide = true;
            canDoubleJump = true;
        }
        CheckWallCollision();
    }

    private void CheckWallCollision()
    {
        isOnWall = Physics2D.Raycast(transform.position, Vector2.right * facingDirection, wallCheckDistance, whatIsWall);
        canWallSlide = CheckIfCanWallSlide();
        isWallSliding = canWallSlide;
        if (canWallSlide)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.1f);
        }
    }

    private bool CheckIfCanWallSlide()
    {
        if (Input.GetAxis("Vertical") < 0)
        {
            return false;
        }
        return isOnWall && rb.velocity.y < 0 ;
    }

    private void DetectEnemies()
    {
        Collider2D[] hitedColliders = Physics2D.OverlapCircleAll(enemyCheck.position, enemyCheckRadius);

        foreach (var enemy in hitedColliders)
        {
            if (enemy.GetComponent<Enemy>() != null)
            {
                Enemy newEnemy = enemy.GetComponent<Enemy>();

                if (newEnemy.invincible) {
                    return;
                }

                if (rb.velocity.y < 0)
                {
                    enemy.GetComponent<Enemy>().Damaged();
                    Jump(jumpForce);
                    canDoubleJump = true;
                    newEnemy.Damaged();
                }
            }
        }
    }

    private void SetupInitialComponents()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    private void SetupInitialSettings()
    {
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    private void OnDrawGizmos() {
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x, transform.position.y - groundCheckDistance));
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x + wallCheckDistance * facingDirection, transform.position.y));
        Gizmos.DrawWireSphere(enemyCheck.position, enemyCheckRadius);
    }
}
