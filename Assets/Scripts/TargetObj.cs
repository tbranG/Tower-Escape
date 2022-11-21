using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetObj : MonoBehaviour
{
    public void SetupTarget()
    {
        GetComponent<Collider2D>().enabled = true;
        gameObject.SetActive(true);
    }

    public void Kill()
    {
        StartCoroutine(DestroyTarget());
    }

    private IEnumerator DestroyTarget()
    {
        GameManager.AddScore(100);
        GetComponent<Animator>().SetTrigger("Destroy");
        GetComponent<Collider2D>().enabled = false;
        yield return new WaitForSeconds(1f);
        gameObject.SetActive(false);
    }
}
