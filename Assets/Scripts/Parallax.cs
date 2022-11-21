using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    public enum Effects
    {
        VERTICAL,
        HORIZONTAL
    }

    [SerializeField] private Effects thisEffect;
    [Range(0f, 1f)][SerializeField] private float effectSpeed;
    [SerializeField] private Vector2 moveDirection;

    private Vector3 inititalPos;

    // Start is called before the first frame update
    void Start()
    {
        inititalPos = transform.position;
    }

    private void MoveBackground()
    {
        Vector3 newPos = new Vector3(
            transform.position.x + (moveDirection.x * 1f),
            transform.position.y + (moveDirection.y * 1f),
            0f
            );

        transform.position = Vector3.Lerp(transform.position, newPos, effectSpeed);
    }

    /// <summary>
    /// Set the background to it's original position. Called when the background current
    /// position it's greater or equals it's size.
    /// </summary>
    private void SetBackgroundBack()
    {
        SpriteRenderer r = GetComponentInChildren<SpriteRenderer>();

        float sizeY = r.bounds.size.y;
        float sizeX = r.bounds.size.x;

        if(thisEffect == Effects.VERTICAL)
        {
            if(Mathf.Abs(transform.position.y - inititalPos.y) >= sizeY)
            {
                transform.position = inititalPos;
            }
        }
        else
        {
            //TODO: implementar parallax horizontal
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        MoveBackground();
        SetBackgroundBack();
    }
}
