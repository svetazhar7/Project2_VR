using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            print("Entered");
            PlayerController player = collision.GetComponent<PlayerController>();
            player.KnockBack(transform);

        }
    }
}
