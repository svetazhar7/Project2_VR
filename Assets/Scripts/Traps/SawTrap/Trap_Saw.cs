using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap_Saw : Trap
{
    [SerializeField] private bool isWorking;
    [SerializeField] private Transform[] movePoint;
    [SerializeField] private float speed = 10f;
    [SerializeField] private float cooldown = 1f;


    private Animator anim;
    private int movePointIndex;
    private float cooldownTimer;

    void Start()
    {
       anim = GetComponent<Animator>();
        anim.SetBool("isWorking", true);

    }

    // Update is called once per frame
    void Update()
    {
        cooldownTimer -= Time.deltaTime;

        bool isWorking = cooldownTimer < 0;

        if (isWorking)
        {
            transform.position = Vector3.MoveTowards(transform.position, movePoint[movePointIndex].position, speed * Time.deltaTime);

        }

        if (Vector2.Distance(transform.position, movePoint[movePointIndex].position) < 0.15f)
        {
            Flip();
            cooldownTimer = cooldown;
            movePointIndex++;

            if (movePointIndex >= movePoint.Length)
            {
                movePointIndex = 0;
            }
        }
    }
    private void Flip()
    {
        transform.localScale = new Vector3(1, transform.localScale.y * -1);
    }
}
