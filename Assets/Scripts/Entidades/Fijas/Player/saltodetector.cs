using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class saltodetector : MonoBehaviour
{
    public Character_Movement cmove;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("BunnyJump"))
        {
            cmove.canJump = true;
        }
    }
}
