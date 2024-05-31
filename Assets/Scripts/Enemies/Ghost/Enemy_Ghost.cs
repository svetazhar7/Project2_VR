using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class Enemy_Ghost : Enemy
{
    [Header("Ghost Specifics")]
    [SerializeField] private float activeTime;
                     private float activeTimeTimer;
    [SerializeField] private float[] xOffset;

    private Transform player;
    private SpriteRenderer sr;
    protected override void Start()
    {
        base.Start();
        player = GameObject.Find("Player").transform;
        sr = GetComponent<SpriteRenderer>();
        activeTimeTimer = activeTime;
        aggresive = true;
        invincible = true;
    }

    private void Update()
    {
        activeTimeTimer -= Time.deltaTime;
        idleTimeTimer -= Time.deltaTime;

        if (activeTimeTimer > 0)
        {
            transform.position = Vector2.MoveTowards(transform.position,player.transform.position, speed * Time.deltaTime);
        }

        if (activeTimeTimer < 0  && idleTimeTimer  < 0 && aggresive)
        {
            anim.SetTrigger("disappear");
            aggresive = false;
            idleTimeTimer = idleTime;
        }
        if (activeTimeTimer < 0 && idleTimeTimer < 0 && !aggresive)
        {
            anim.SetTrigger("appear");
            aggresive = true;
            activeTimeTimer = activeTime;

        }
        OnXAxisMove();
    }

    public void Desappear()
    {
        sr.enabled = false;
    }

    public void Appear()
    {
        AppearPosition();
        sr.enabled = true;
    }

    public override void Damaged()
    {
        base.Damaged();
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (aggresive)
        {
            base.OnTriggerEnter2D(collision);
        }
    }

    private void AppearPosition()
    {
        float yOffset = Random.Range(-7, 7);
        float xoffsetValue = xOffset[Random.Range(0, xOffset.Length)];
        transform.position = new Vector2(player.transform.position.x + xoffsetValue, player.transform.position.y + yOffset);
    }

    protected override void OnXAxisMove()
    {
        if (facingDirection == -1 && transform.position.x < player.transform.position.x)
        {
            Flip();
            Debug.Log("Flip!");
        }
        else if (facingDirection == 1 && transform.position.x > player.transform.position.x)
        {
            Flip();
            Debug.Log("Flip!2");

        }
    }
}
