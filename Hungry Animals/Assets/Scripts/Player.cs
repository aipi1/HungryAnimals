using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles Player behaviour
/// </summary>
public class Player : GameCharacter
{
    public int currentHealth = 100;
    public int maxHealth = 100;
    public bool isDead = false;
    [SerializeField] private AudioClip hitSound;
    [SerializeField] private AudioClip deathSound;
    [SerializeField] private AudioClip throwSound;
    [SerializeField] private AudioClip healthSound;
    private SoundManager soundManager;
    private PlayerController playerController;
    private bool isAtacked = false;

    void Awake()
    {
        Speed = 25.0f;
        xRangeBound = 20.0f;
        zTopBound = 22.5f;
        zBotBound = 0f;
        characterRb = GetComponent<Rigidbody>();
        characterAnimator = GetComponent<Animator>();
        playerController = GetComponent<PlayerController>();
        soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();
    }

    void Update()
    {
        if (!isDead)
        {
            playerController.GetMousePosition();
            if (GameManager.isGameInProgress)
            {
                playerController.GetInput();
                ChangeRunningAnimation();
            }
        }
        CheckMapBounds();
    }

    void FixedUpdate()
    {
        if (!isAtacked)
        {
            Move(new Vector3(playerController.horizontalInput, 0, playerController.verticalInput));
        }
        if (isDead)
        {
            StopMoving();
        }
    }

    public void RotateTowardsMouse(Vector3 pointToLook)
    {
        transform.LookAt(new Vector3(pointToLook.x, transform.position.y, pointToLook.z));
    }

    public void ThrowFood()
    {
        GameObject food = ObjectPooler.SharedInstance.GetPooledObject();

        if (food != null)
        {
            food.transform.position = transform.position + transform.forward * 1.5f;
            food.transform.rotation = transform.rotation;
            food.SetActive(true);
            soundManager.PlaySound(throwSound);
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            var enemy = collision.gameObject;
            var damage = enemy.GetComponent<Enemy>().damage;
            var forceDirection = transform.position - enemy.transform.position;
            var time = 0.5f;

            if (!isAtacked)
            {
                isAtacked = true;
                StartCoroutine(ApplyDamagePerTime(damage, forceDirection, time));
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("HealthBonus"))
        {
            currentHealth = maxHealth;
            other.gameObject.SetActive(false);
            soundManager.PlaySound(healthSound);
        }
    }

    private IEnumerator ApplyDamagePerTime(int damage, Vector3 forceDirection, float time)
    {
        ApplyDamage(damage);
        soundManager.PlaySound(hitSound);
        characterRb.AddForce(forceDirection * 500.0f, ForceMode.Impulse);
        characterAnimator.SetBool("Grounded", false);
        yield return new WaitForSeconds(time);
        isAtacked = false;
        characterAnimator.SetBool("Grounded", true);
    }

    private void ApplyDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        int deathType = Random.Range(1, 3);

        soundManager.PlaySound(deathSound);
        isDead = true;
        playerController.StopGettingInput();
        characterAnimator.SetInteger("DeathType_int", deathType);
        characterAnimator.SetBool("Death_b", true);
    }

    private void StopMoving()
    {
        characterRb.angularVelocity = Vector3.zero;
        characterRb.velocity = Vector3.zero;
    }

    private void ChangeRunningAnimation()
    {
        if (playerController.horizontalInput != 0 || playerController.verticalInput != 0)
        {
            characterAnimator.SetFloat("Speed_f", 0.55f);
        }
        else
        {
            characterAnimator.SetFloat("Speed_f", 0f);
        }
    }
}