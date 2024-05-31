using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapMorgen : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Vector2 pushDirectionl;

    void Start()
    {
        rb.AddForce(pushDirectionl, ForceMode2D.Impulse);
    }

}
