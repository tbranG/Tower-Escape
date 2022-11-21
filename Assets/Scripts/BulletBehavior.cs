using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehavior : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float maxDistance;
    private ParticleSystem splash;

    private Vector2 direction;
    private Vector3 initialPos;

    private Rigidbody2D body;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        splash = GetComponentInChildren<ParticleSystem>();
    }

    private void Start()
    {
        initialPos = transform.position;
    }

  
    public void SetupBullet(Vector2 dir)
    {
        GetComponentInChildren<SpriteRenderer>().enabled = true;
        GetComponent<Collider2D>().enabled = true;
        gameObject.SetActive(true);

        direction = dir;
    }

    //initialize movement
    public void StartBullet()
    {
        body.velocity = direction * speed;
    }

    private void Update()
    {
        if(Vector2.Distance(transform.position, initialPos) >= maxDistance)
        {
            StartCoroutine(Kill());
        }
    }

    //disable this object
    public IEnumerator Kill()
    {
        splash.Play();
        GetComponentInChildren<SpriteRenderer>().enabled = false;
        GetComponent<Collider2D>().enabled = false;
        yield return new WaitForSeconds(0.4f);
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Targets"))
        {
            collision.gameObject.GetComponent<TargetObj>().Kill();
            StartCoroutine(Kill());
        }
        else
        {
            BridgeObj bridge = collision.gameObject.GetComponent<BridgeObj>();
            if(bridge != null)
            {
                bridge.Kill();
                GameManager.AddScore(100);
                StartCoroutine(Kill());
            }
        }
    }
}
