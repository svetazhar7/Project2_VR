using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Trap_Fire_Switch : MonoBehaviour
{
    [SerializeField] Trap_Fire trap;
    [SerializeField] float trapStopTime;
    private Animator animator;
    
    private void Start()
    {
        animator = GetComponent<Animator>();        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerController>() != null) {

            animator.SetTrigger("pressed");
            trap.FireSwitchAfter(trapStopTime);

        }
    }
}
