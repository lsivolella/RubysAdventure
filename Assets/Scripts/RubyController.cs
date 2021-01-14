using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RubyController : MonoBehaviour
{
    // Serialized Parameters
    [Header("Movement")]
    [SerializeField] float movementSpeed = 1f;
    [Header("Health")]
    [SerializeField] AudioClip hitSound;
    [SerializeField] int maxHealth = 1;
    [SerializeField] float timeInvincible = 1f;
    [Header("Projectile")]
    [SerializeField] AudioClip projectileSound;
    [SerializeField] GameObject projetilePrefab;
    [SerializeField] float projectilePadding = 1f;
    [SerializeField] float projetileForce = 1f;
    [Header("Raycast")]
    [SerializeField] float raycastPadding = 1f;
    [SerializeField] float raycastDistance = 1f;


    // Cached Variables
    private Rigidbody2D myRigidbody2D;
    private Animator myAnimator;
    private AudioSource myAudioSource;

    private Vector2 lookDirection = new Vector2(1, 0);
    private Vector2 move;
    private float horizontalInput;
    private float verticalInput;
    
    public int GetCurrentHealth { get { return currentHealth; } }
    public int GetMaxHealth { get { return maxHealth; } }
    private int currentHealth;
    private float invincibleTimer;

    // Cached States
    private bool isInvincible = false;

    private void Awake()
    {
        GetComponents();
        SetPlayerHealth();
    }

    private void GetComponents()
    {
        myRigidbody2D = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myAudioSource = GetComponent<AudioSource>();
    }

    private void SetPlayerHealth()
    {
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        PlayerMovementInput();
        CheckInvincibility();

        if(Input.GetKeyDown(KeyCode.C))
        {
            LaunchProjectile();
        }

        if(Input.GetKeyDown(KeyCode.X))
        {
            LaunchRaycast();
        }
    }

    private void LaunchRaycast()
    {
        RaycastHit2D hit = Physics2D.Raycast(myRigidbody2D.position + Vector2.up * raycastPadding, lookDirection, raycastDistance, LayerMask.GetMask("NPC"));

        if (hit.collider)
        {
            NonPlayerCharacter character = hit.collider.GetComponent<NonPlayerCharacter>();
            if (character)
            {
                character.DisplayDialog();
            }
        }
    }

    private void PlayerMovementInput()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        move = new Vector2(horizontalInput, verticalInput);

        if (!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
        {
            lookDirection.Set(move.x, move.y);
            lookDirection.Normalize();
        }

        myAnimator.SetFloat("Look X", lookDirection.x);
        myAnimator.SetFloat("Look Y", lookDirection.y);
        myAnimator.SetFloat("Speed", move.magnitude);
    }

    private void CheckInvincibility()
    {
        if (isInvincible)
        {
            invincibleTimer -= Time.deltaTime;
            if (invincibleTimer < 0)
            {
                isInvincible = false;
            }
        }
    }

    void FixedUpdate()
    {
        PlayerMovementOutput();
    }

    private void PlayerMovementOutput()
    {
        Vector2 position = myRigidbody2D.position;
        position += move * movementSpeed * Time.deltaTime;
        myRigidbody2D.MovePosition(position);
    }

    public void ChangeHealth(int amount)
    {
        if (amount < 0)
        {
            if (isInvincible)
            {
                return;
            }
            else
            {
                isInvincible = true;
                invincibleTimer = timeInvincible;
                PlaySound(hitSound);
                myAnimator.SetTrigger("Hit");
            }
        }
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        UIHealthBar.instance.SetValue(currentHealth / (float)  maxHealth);
    }

    private void LaunchProjectile()
    {
        GameObject newProjectile = Instantiate(projetilePrefab, myRigidbody2D.position + Vector2.up * projectilePadding, Quaternion.identity);
        ProjectileController projectileController = newProjectile.GetComponent<ProjectileController>();
        projectileController.LaunchProjectile(lookDirection, projetileForce);
        PlaySound(projectileSound);
        myAnimator.SetTrigger("Launch");
    }

    public void PlaySound(AudioClip clip)
    {
        myAudioSource.PlayOneShot(clip);
    }
}
