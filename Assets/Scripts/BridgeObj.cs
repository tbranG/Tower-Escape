using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeObj : MonoBehaviour
{
    public void SetupBridge()
    {
        gameObject.SetActive(true);
        GetComponentInChildren<SpriteRenderer>().enabled = true;
        GetComponent<Collider2D>().enabled = true;

        GetComponent<MoveObject>()?.ForceMovement();
    }

    public void Kill()
    {
        StartCoroutine(DestroyBridge());
    }

    private IEnumerator DestroyBridge()
    {
        GetComponentInChildren<SpriteRenderer>().enabled = false;
        GetComponent<Collider2D>().enabled = false;
        GetComponentInChildren<ParticleSystem>().Play();

        yield return new WaitForSeconds(1f);
        gameObject.SetActive(false);
    }
}
