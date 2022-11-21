using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveObject : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private Vector2 dir;
    private Rigidbody2D body;

    private void Awake() { body = GetComponent<Rigidbody2D>(); }
    private void Start() 
    { 
        //setup
        body.interpolation = RigidbodyInterpolation2D.Interpolate;
        body.drag = 0f;
        body.gravityScale = 0f;

        body.velocity = new Vector2(dir.x * moveSpeed, dir.y * moveSpeed);
    }

    public void ForceMovement()
    {
        body.velocity = new Vector2(dir.x * moveSpeed, dir.y * moveSpeed);
    }
}
