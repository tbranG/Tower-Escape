using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RotationAnim : MonoBehaviour
{
    [SerializeField] private float animSpeed;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SetRotation());
    }

    private IEnumerator SetRotation()
    {
        yield return new WaitForSeconds(animSpeed);
        transform.rotation = Quaternion.Euler(0f, 0f, transform.localEulerAngles.z + 30f);

        float rotZ = transform.localEulerAngles.z;
        if(rotZ >= 360f)
        {
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }

        StartCoroutine(SetRotation());
    }
}
