using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap_Saw_Extended : Trap
{
    [SerializeField] private Transform[] movePoint;
    [SerializeField] private float speed = 10f;


    private Animator anim;
    private int movePointIndex;

    void Start()
    {
        anim = GetComponent<Animator>();
        anim.SetBool("isWorking", true);
    }

    // Update is called once per frame
    void Update()
    {

        transform.position = Vector3.MoveTowards(transform.position, movePoint[movePointIndex].position, speed * Time.deltaTime);


        if (Vector2.Distance(transform.position, movePoint[movePointIndex].position) < 0.15f)
        {
            Flip();
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
