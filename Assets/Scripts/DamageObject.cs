using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageObject : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            PlayerBehavior playerStats = collision.gameObject.GetComponent<PlayerBehavior>();
            if(playerStats != null)
            {
                playerStats.Kill();
            }
            else
            {
                Debug.LogError("Could not get reference: Player");
            }
        }
    }
}
