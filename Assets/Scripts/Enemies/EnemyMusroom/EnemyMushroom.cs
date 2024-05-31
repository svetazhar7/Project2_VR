using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMushroom : Enemy
{

    protected override void Start()
    {
        base.Start();
    }

    private void Update()
    {
        CheckAxistMovement();
        CollisionChecks();
        OnXAxisMove();
        Wander();
    }
    protected override void OnXAxisMove()
    {
        base.OnXAxisMove();
        anim.SetBool("isMoving", isMovingOnX);
    }

}
