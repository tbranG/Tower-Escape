using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehavior : MonoBehaviour
{
    private static PlayerBehavior instance;
    private static bool isDead;

    [Header("Stats")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float recoilForce;

    private float axisH;
    private float axisV;

    private int ammo = 4;
    private int current_bullet = 0;
    private GameObject[] magazine;

    [Header("Componentes")]
    [SerializeField] private Transform hand;
    [SerializeField] private Transform muzzle;
    [SerializeField] private GameObject bullet;
    [SerializeField] private GameObject blood;
    [SerializeField] private ParticleSystem smoke;
    [SerializeField] private AudioSource canonSound;
    [SerializeField] private AudioSource reloadSound;
    private Rigidbody2D body;
    private Animator anim;

    [Header("Flags")]
    [SerializeField] private bool infiniteAmmo;
    private bool reloading;

    //events
    public delegate void GunFired();
    public event GunFired OnGunFired;
    public delegate void GunReloaded();
    public event GunReloaded OnGunReloaded;
    public delegate void PlayerDied();
    public event PlayerDied OnPlayerDeath;

    public static PlayerBehavior Instance { get => instance; }
    public static bool IsDead { get => isDead; }
    public int Ammo { get => ammo; }


    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        isDead = false;
        reloading = false;
        magazine = new GameObject[4];
        for(int i = 0; i < 4; i++)
        {
            GameObject b = Instantiate(bullet, Vector3.zero, Quaternion.identity);
            b.SetActive(false);

            magazine[i] = b;
        }
    }

    #region movement
    /// <summary>
    /// Is player pressing movement keys?
    /// </summary>
    /// <returns>Bool: yes/no</returns>
    public bool IsPressing() => (axisH != 0 || axisV != 0);

    private void GetMoveInput()
    {
        axisH = Input.GetAxisRaw("Horizontal");
        axisV = Input.GetAxisRaw("Vertical");
    }

    /// <summary>
    /// Moves the player object.
    /// <para>Keys: A/D </para>
    /// </summary>
    private void SetMovement()
    {
        Vector2 motion = new Vector2(axisH * moveSpeed, axisV * moveSpeed);
        body.AddForce(motion);
    }
    #endregion

    #region combat
    private void Aim()
    {
        Vector2 relativeMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        float rotZ = Mathf.Atan2(relativeMousePos.y, relativeMousePos.x) * Mathf.Rad2Deg;
        hand.transform.rotation = Quaternion.Euler(0f, 0f, rotZ + 90f);
    }

    private void MagazineControl()
    {
        if(!infiniteAmmo) ammo--;
        current_bullet++;
        if (current_bullet >= 4) current_bullet = 0;
    }

    private IEnumerator Reload()
    {
        reloading = true;
        anim.SetBool("Reloading", reloading);
        reloadSound.Play();
        yield return new WaitForSeconds(3f);
        reloading = false;
        anim.SetBool("Reloading", reloading);
        ammo = 4;
        reloadSound.Pause();
        OnGunReloaded?.Invoke();
    }

    /// <summary>
    /// Casts a bullet at mouse position
    /// </summary>
    public void Shoot()
    {
        //animation
        anim.SetTrigger("Shoot");
        smoke.Play();
        canonSound.Play();

        GameObject b = magazine[current_bullet];
        BulletBehavior behavior = b.GetComponent<BulletBehavior>();

        //bullet setup
        b.transform.position = muzzle.transform.position;

        float rotZ = (hand.eulerAngles.z - 90f) * Mathf.Deg2Rad;
        Vector2 dir = new Vector2(Mathf.Cos(rotZ), Mathf.Sin(rotZ));
        behavior.SetupBullet(dir);
        behavior.StartBullet();

        MagazineControl();

        //recoil
        Vector2 recoil = dir * -1;
        body.AddForce(recoil * recoilForce, ForceMode2D.Impulse);

        OnGunFired?.Invoke();
    }
    #endregion

    // Update is called once per frame
    void Update()
    {
        if (GameManager.IsPaused) return;

        GetMoveInput();

        if (!reloading)
        {
            Aim();
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (ammo > 0)
            {
                Shoot();
            }
        } 

        if(ammo <= 0)
        {
            if (Input.GetKeyDown(KeyCode.R) && !reloading)
            {
                StartCoroutine(Reload());
            }
        }
    }

    public void Kill()
    {
        GameObject splash = Instantiate(blood, transform.position, Quaternion.identity);
        splash.GetComponent<ParticleSystem>().Play();
        isDead = true;

        OnPlayerDeath?.Invoke();
        
        gameObject.GetComponent<Collider2D>().enabled = false;
        SpriteRenderer[] childs = gameObject.transform.GetComponentsInChildren<SpriteRenderer>();
        foreach(SpriteRenderer spr in childs)
        {
            spr.enabled = false;
        }
    }

    private void FixedUpdate()
    {
        if (GameManager.IsPaused) return;

        if (IsPressing())
            SetMovement();
       
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Traps"))
        {
            BridgeObj bridge = collision.gameObject.GetComponent<BridgeObj>();
            if(bridge != null)
            {
                bridge.Kill();
            }
        }
    }
}
