using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    // Serialized Parameters
    [Header("Damage")]
    [SerializeField] AudioClip[] damageSound;
    [SerializeField] int enemyDamage = -1;
    // Serialized Parameters
    [Header("Movement")]
    [SerializeField] float movementSpeed = 1f;
    [SerializeField] float movementTimer = 1f;
    [Header("Particle System")]
    [SerializeField] ParticleSystem smokeEffect;

    // Cached Variables
    private Rigidbody2D myRigidbody2D;
    private Animator myAnimator;

    private float timer;
    private int randomDirection;

    // Cached States
    private bool isVertical = false;
    private bool isBroken = true;

    private void Awake()
    {
        GetComponents();
        StartMovementTimer();
        RandomizeMovement();
    }
    private void GetComponents()
    {
        myRigidbody2D = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
    }

    private void StartMovementTimer()
    {
        timer = movementTimer;
    }

    private void RandomizeMovement()
    {
        int[] options = new int[2];
        options[0] = -1;
        options[1] = 1;
        var randomIndex = UnityEngine.Random.Range(0, 2);
        randomDirection = options[randomIndex];
        isVertical = !isVertical;
    }

    private void Update()
    {
        if (!isBroken) return;
        MovementTimerController();
    }

    private void MovementTimerController()
    {
        timer -= Time.deltaTime;

        if (timer < 0)
        {
            RandomizeMovement();
            StartMovementTimer();
        }
    }

    private void FixedUpdate()
    {
        if (!isBroken) return;
        MoveUpAndDown();
    }

    private void MoveUpAndDown()
    {
        Vector2 position = myRigidbody2D.position;

        if (isVertical)
        {
            position.y += Time.deltaTime * movementSpeed * randomDirection;
            myAnimator.SetFloat("MoveX", 0);
            myAnimator.SetFloat("MoveY", randomDirection);
        }
        else
        {
            position.x += Time.deltaTime * movementSpeed * randomDirection;
            myAnimator.SetFloat("MoveY", 0);
            myAnimator.SetFloat("MoveX", randomDirection);
        }
        myRigidbody2D.MovePosition(position);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        RubyController player = collision.gameObject.GetComponent<RubyController>();

        if (player)
        {
            player.ChangeHealth(enemyDamage);
            player.PlaySound(PickRandomSound());
        }
    }

    private AudioClip PickRandomSound()
    {
        int randomIndex = UnityEngine.Random.Range(0, damageSound.Length + 1);
        AudioClip randomSong = damageSound[randomIndex];
        return randomSong;
    }

    public void FixBot()
    {
        isBroken = false;
        myRigidbody2D.simulated = false;
        myAnimator.SetBool("isIdle", true);
        smokeEffect.Stop();
    }
}
